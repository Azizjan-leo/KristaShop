using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Module.Media.Business.UnitOfWork;
using P.Pager;

namespace Module.Media.Business.Services {
    public class GalleryService : IGalleryService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GalleryService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<OperationResult> SwitchGalleryVisabilityAsync(Guid id) {
            var item = await _uow.GalleryItems.GetByIdAsync(id);
            if (item == null) return OperationResult.Failure();
            item.IsVisible = !item.IsVisible;
            await _uow.SaveAsync();
            return OperationResult.Success();
        }

        public async Task<List<GalleryItemDTO>> GetGalleriesAsync() {
            return await _uow.GalleryItems.All
                .ProjectTo<GalleryItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IPager<GalleryItemDTO>> GetGalleryItemsForPageAsync(int page, int modelInPage) {
            return await _uow.GalleryItems.All
                .OrderBy(x => x.Order)
                .ProjectTo<GalleryItemDTO>(_mapper.ConfigurationProvider)
                .ToPagerListAsync(page, modelInPage);
        }

        public async Task<List<GalleryItemDTO>> GetTopGalleryItemsAsync(int quantity) {
            return await _uow.GalleryItems.All
                .OrderBy(x => x.Order)
                .Take(quantity)
                .ProjectTo<GalleryItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<GalleryItemDTO>> GetTopNRandomItemsAsync(int quantity) {
            return await _uow.GalleryItems.All
                .OrderBy(x => CustomMysqlFunctions.Random())
                .Take(quantity)
                .ProjectTo<GalleryItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<GalleryItemDTO> GetGalleryItemAsync(Guid id) {
            var entity = await _uow.GalleryItems.GetByIdAsync(id);
            return _mapper.Map<GalleryItemDTO>(entity);
        }

        public async Task<GalleryItemDTO> GetGalleryItemNoTrackAsync(Guid id) {
            var entity = await _uow.GalleryItems.All
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return _mapper.Map<GalleryItemDTO>(entity);
        }

        public async Task<OperationResult> InsertGalleryAsync(GalleryItemDTO galleryItem) {
            var entity = _mapper.Map<GalleryItem>(galleryItem);
            var lastOrder = _uow.GalleryItems.All.OrderByDescending(s => s.Order).FirstOrDefault()?.Order ?? 0;
            entity.Order = lastOrder + 1;

            await _uow.GalleryItems.AddAsync(entity, true);

            return (await _uow.SaveAsync()) ? OperationResult.Success() : OperationResult.Failure();
        }

        public async Task<OperationResult> UpdateGalleryAsync(GalleryItemDTO galleryItem) {
            var entity = await _uow.GalleryItems.GetByIdAsync(galleryItem.Id);
            entity.ImagePath = galleryItem.ImagePath ?? entity.ImagePath;
            entity.IsVisible = galleryItem.IsVisible;
            entity.Title = galleryItem.Title;
            entity.Description = galleryItem.Description;
            entity.LinkText = galleryItem.LinkText;
            entity.Link = galleryItem.Link;
            entity.Order = galleryItem.Order;
            _uow.GalleryItems.Update(entity);

            return (await _uow.SaveAsync()) ? OperationResult.Success() : OperationResult.Failure();
        }

        public async Task<OperationResult> DeleteGalleryAsync(Guid id) {
            await _uow.GalleryItems.DeleteAsync(id);

            return (await _uow.SaveAsync()) ? OperationResult.Success() : OperationResult.Failure();
        }
    }
}
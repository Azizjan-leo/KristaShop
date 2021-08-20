using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Module.Media.Business.UnitOfWork;
using P.Pager;

namespace Module.Media.Business.Services {
    public class BlogService : IBlogService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BlogService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<OperationResult> SwitchBlogVisabilityAsync(Guid id) {
            var item = await _uow.BlogItems.GetByIdAsync(id);
            if (item == null) return OperationResult.Failure();
            item.IsVisible = !item.IsVisible;
            await _uow.SaveAsync();
            return OperationResult.Success();
        }

        public async Task<List<BlogItemDTO>> GetBlogsAsync() {
            return await _uow.BlogItems.All
                .OrderBy(x => x.Order)
                .ProjectTo<BlogItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IPager<BlogItemDTO>> GetBlogsForPageAsync(int page, int modelInPage) {
            return await _uow.BlogItems.All.OrderBy(x => x.Order)
                .ProjectTo<BlogItemDTO>(_mapper.ConfigurationProvider)
                .ToPagerListAsync(page, modelInPage);
        }

        public async Task<List<BlogItemDTO>> GetTopBlogsAsync(int count) {
            return await _uow.BlogItems.All.OrderBy(x => x.Order).Take(count)
                .ProjectTo<BlogItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<BlogItemDTO> GetBlogAsync(Guid id) {
            var blog = await _uow.BlogItems.GetByIdAsync(id);
            return _mapper.Map<BlogItemDTO>(blog);
        }

        public async Task<BlogItemDTO> GetBlogNoTrackAsync(Guid id) {
            var entity = await _uow.BlogItems.All
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return _mapper.Map<BlogItemDTO>(entity);
        }

        public async Task<OperationResult> InsertBlogAsync(BlogItemDTO blogItem) {
            var entity = _mapper.Map<BlogItem>(blogItem);
            var lastOrder = _uow.BlogItems.All.OrderByDescending(s => s.Order).FirstOrDefault()?.Order ?? 0;
            entity.Order = lastOrder + 1;

            await _uow.BlogItems.AddAsync(entity, true);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateBlogAsync(BlogItemDTO blogItem) {
            var entity = await _uow.BlogItems.GetByIdAsync(blogItem.Id);
            entity.ImagePath = blogItem.ImagePath ?? entity.ImagePath;
            entity.IsVisible = blogItem.IsVisible;
            entity.LinkText = blogItem.LinkText;
            entity.Link = blogItem.Link;
            entity.Title = blogItem.Title;
            entity.Description = blogItem.Description;
            entity.Order = blogItem.Order;
            _uow.BlogItems.Update(entity);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteBlogAsync(Guid id) {
            await _uow.BlogItems.DeleteAsync(id);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }
    }
}
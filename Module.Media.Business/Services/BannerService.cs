using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Module.Media.Business.UnitOfWork;

namespace Module.Media.Business.Services {
    public class BannerService : IBannerService {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public BannerService(IUnitOfWork uow, IFileService fileService, IMapper mapper) {
            _uow = uow;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<OperationResult> SwitchBannerVisabilityAsync(Guid id) {
            var item = await _uow.Banners.GetByIdAsync(id);
            if (item == null) return OperationResult.Failure();
            item.IsVisible = !item.IsVisible;
            await _uow.SaveAsync();
            return OperationResult.Success();
        }

        public async Task<List<BannerItemDTO>> GetBannersAsync(bool visibleOnly = false) {
            var query = _uow.Banners.All;
            if (visibleOnly) {
                query = query.Where(x => x.IsVisible);
            }

            return await query.OrderBy(x => x.Order)
                .ProjectTo<BannerItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<BannerItemDTO> GetBannerAsync(Guid id) {
            var entity = await _uow.Banners.GetByIdAsync(id);
            return _mapper.Map<BannerItemDTO>(entity);
        }

        public async Task<BannerItemDTO> GetBannerNoTrackAsync(Guid id) {
            var entity = await _uow.Banners.All
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return _mapper.Map<BannerItemDTO>(entity);
        }

        public async Task<OperationResult> InsertBannerAsync(BannerItemDTO banner) {
            var entity = _mapper.Map<BannerItem>(banner);
            var lastOrder = _uow.Banners.All.OrderByDescending(s => s.Order).FirstOrDefault()?.Order ?? 0;
            entity.Order = lastOrder + 1;

            if (banner.Image != null) {
                entity.ImagePath = await _fileService.SaveFileAsync(banner.Image);
                if (string.IsNullOrEmpty(entity.ImagePath)) {
                    return _fileService.GetLastError();
                }
            }

            await _uow.Banners.AddAsync(entity, true);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateBannerAsync(BannerItemDTO banner) {
            var entity = await _uow.Banners.GetByIdAsync(banner.Id);

            var oldFile = entity.ImagePath;
            entity.ImagePath = banner.ImagePath ?? entity.ImagePath;

            entity.IsVisible = banner.IsVisible;
            entity.Link = banner.Link;
            entity.Title = banner.Title;
            entity.Caption = banner.Caption;
            entity.Description = banner.Description;
            entity.Order = banner.Order;
            entity.TitleColor = banner.TitleColor;


            if (banner.Image != null) {
                if (!string.IsNullOrEmpty(entity.ImagePath)) {
                    if (!_fileService.RemoveFile(banner.Image.FilesDirectoryPath, entity.ImagePath)) {
                        return _fileService.GetLastError();
                    }
                }

                if (!string.IsNullOrEmpty(oldFile)) {
                    if (!_fileService.RemoveFile(banner.Image.FilesDirectoryPath, oldFile)) {
                        return _fileService.GetLastError();
                    }
                }

                entity.ImagePath = await _fileService.SaveFileAsync(banner.Image);
                if (string.IsNullOrEmpty(entity.ImagePath)) {
                    return _fileService.GetLastError();
                }
            }

            _uow.Banners.Update(entity);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteBanner(Guid id, string fileDirectoryPath) {
            var banner = await _uow.Banners.DeleteAsync(id);
            if (banner != null) {
                _fileService.RemoveFile(fileDirectoryPath, banner.ImagePath);
            }

            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }
    }
}
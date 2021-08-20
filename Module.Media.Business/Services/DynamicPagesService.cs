using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Module.Media.Business.UnitOfWork;

namespace Module.Media.Business.Services {
    public class DynamicPagesService : IDynamicPagesService {
        private readonly IMapper _mapper;
        private readonly IDynamicPagesManager _manager;
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;

        public DynamicPagesService(IMapper mapper, IUnitOfWork uow, IFileService fileService, IDynamicPagesManager manager) { 
            _mapper = mapper;
            _uow = uow;
            _fileService = fileService;
            _manager = manager;
        }

        public async Task<List<DynamicPageDTO>> GetPagesAsync(bool openOnly = false, bool visibleInMenuOnly = false) {
            return await _uow.DynamicPage.GetAllOrderedOpenAndVisibleInMenuOnly(openOnly, visibleInMenuOnly)
                .AsNoTracking()
                .ProjectTo<DynamicPageDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<DynamicPageDTO>> GetPageByControllerAsync(string controller, bool openOnly = false, bool visibleInMenuOnly = false) {
            return await _uow.DynamicPage.GetAllOrderedOpenAndVisibleInMenuOnly(openOnly, visibleInMenuOnly)
                .Where(x => EF.Functions.Like(x.Url, $"%{controller}/%"))
                .ProjectTo<DynamicPageDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<DynamicPageDTO>> GetPageByUrlsAsync(List<string> urls) {
            return await _uow.DynamicPage.AllOrdered
                .Where(x => urls.Contains(x.Url))
                .ProjectTo<DynamicPageDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<DynamicPageDTO> GetPageByIdAsync(Guid id) {
            return _mapper.Map<DynamicPageDTO>(await _uow.DynamicPage.GetByIdAsync(id));
        }

        public async Task<DynamicPageDTO> GetPageByUrlAsync(string url) {
            return await _uow.DynamicPage.All
                .Where(x => x.Url.Equals(url))
                .ProjectTo<DynamicPageDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<OperationResult> InsertPageAsync(DynamicPageDTO dynamicPage) {
            var entity = _mapper.Map<DynamicPage>(dynamicPage);
            entity.Order = await _uow.DynamicPage.GetNewOrderValueAsync();

            if (dynamicPage.TitleIcon != null) {
                entity.TitleIconPath = await _fileService.SaveFileAsync(dynamicPage.TitleIcon);
                if (string.IsNullOrEmpty(entity.TitleIconPath)) {
                    return _fileService.GetLastError();
                }
            }

            if (dynamicPage.Image != null) {
                entity.ImagePath = await _fileService.SaveFileAsync(dynamicPage.Image);
                if (string.IsNullOrEmpty(entity.ImagePath)) {
                    return _fileService.GetLastError();
                }
            }

            await _uow.DynamicPage.AddAsync(entity);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            await _manager.ReloadAsync(entity.Id);
            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdatePageAsync(DynamicPageDTO dynamicPage) {
            var entity = await _uow.DynamicPage.GetByIdAsync(dynamicPage.Id);
            var oldUrl = entity.Url;

            entity = _mapper.Map(dynamicPage, entity);

            if (dynamicPage.TitleIcon != null) {
                if (!string.IsNullOrEmpty(entity.TitleIconPath)) {
                    if (!_fileService.RemoveFile(dynamicPage.TitleIcon.FilesDirectoryPath, entity.TitleIconPath)) {
                        return _fileService.GetLastError();
                    }
                }

                entity.TitleIconPath = await _fileService.SaveFileAsync(dynamicPage.TitleIcon);
                if (string.IsNullOrEmpty(entity.TitleIconPath)) {
                    return _fileService.GetLastError();
                }
            }

            if (dynamicPage.Image != null) {
                if (!string.IsNullOrEmpty(entity.ImagePath)) {
                    if (!_fileService.RemoveFile(dynamicPage.Image.FilesDirectoryPath, entity.ImagePath)) {
                        return _fileService.GetLastError();
                    }
                }

                entity.ImagePath = await _fileService.SaveFileAsync(dynamicPage.Image);
                if (string.IsNullOrEmpty(entity.ImagePath)) {
                    return _fileService.GetLastError();
                }
            }

            _uow.DynamicPage.Update(entity);
            if (!await _uow.SaveAsync())
                return OperationResult.Failure();

            await _manager.ReloadAsync(entity.Id, oldUrl.Equals(entity.Url) ? string.Empty : oldUrl);
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeletePageAsync(Guid id, string fileDirectoryPath) {
            try {
                var entity = await _uow.DynamicPage.GetByIdAsync(id);

                if (!string.IsNullOrEmpty(entity.TitleIconPath)) {
                    if (!_fileService.RemoveFile(fileDirectoryPath, entity.TitleIconPath)) {
                        return _fileService.GetLastError();
                    }
                }

                if (!string.IsNullOrEmpty(entity.ImagePath)) {
                    if (!_fileService.RemoveFile(fileDirectoryPath, entity.ImagePath)) {
                        return _fileService.GetLastError();
                    }
                }

                await _uow.BeginTransactionAsync();
                _uow.DynamicPage.Delete(entity);
                await _restorePagesOrderFromPositionAsync(entity.Order);

                if (!await _uow.SaveAsync()) {
                    return OperationResult.Failure();
                }
                _uow.CommitTransaction();

                await _manager.ReloadAsync(entity.Id, entity.Url);
                return OperationResult.Success();
            } catch (Exception) {
                _uow.RollbackTransaction();
                throw;
            }
        }

        public async Task<OperationResult> UpdatePageOrderAsync(Guid id, int newOrder) {
            try {
                await _uow.BeginTransactionAsync();
                var entity = await _uow.DynamicPage.GetByIdAsync(id);
                entity.Order = newOrder;

                if (!await _uow.SaveAsync()) {
                    return OperationResult.Failure();
                }

                await _manager.ReloadAsync(entity.Id);
                _uow.CommitTransaction();
                return OperationResult.Success();
            } catch (Exception ) {
                _uow.RollbackTransaction();
                throw;
            }
        }

        public async Task<OperationResult> RestorePageOrderAsync() {
            try {
                await _uow.BeginTransactionAsync();
                await _restorePagesOrderFromPositionAsync(0);

                if (!await _uow.SaveAsync()) {
                    return OperationResult.Failure();
                }
                _uow.CommitTransaction();

                await _manager.ReloadAsync();
                return OperationResult.Success();
            } catch (Exception ex) {
                _uow.RollbackTransaction();
                throw;
            }
          
        }

        private async Task _restorePagesOrderFromPositionAsync(int orderPosition) {
            var items = await _uow.DynamicPage.All.Where(x => x.Order > orderPosition).ToListAsync();
            var order = orderPosition - 1;
            foreach (var item in items) {
                item.Order = ++order;
            }

            _uow.DynamicPage.UpdateRange(items);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Catalogs.Business.UnitOfWork;
using Module.Common.Business.Interfaces;

namespace Module.Catalogs.Business.Services {
    public class CatalogService : ICatalogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CatalogService(IUnitOfWork uow, IMapper mapper, IFileService fileService) {
            _uow = uow;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<List<CatalogDTO>> GetCatalogsAsync(bool includeDiscounts = false) {
            return await _uow.Catalogs.All
                .Select(x => new CatalogDTO {
                    Id = x.Id,
                    Name = x.Name,
                    Uri = x.Uri,
                    OrderFormName = x.OrderForm.GetDisplayName(),
                    Order = x.Order,
                    IsVisible = x.IsVisible,
                    IsDisableDiscount = x.IsDiscountDisabled,
                    NomCount = x.CatalogItems.Select(c => c.Articul).Distinct().Count()
                }).ToListAsync();
        }

        public async Task<List<CatalogDTO>> GetCatalogsAsync(List<CatalogType> catalogIds) {
            var resultList = await _uow.Catalogs.All.Where(x =>
                    catalogIds.Contains(x.Id) && x.IsVisible &&
                    (x.CloseTime == null || x.CloseTime > DateTimeOffset.Now))
                .OrderBy(x => x.Order)
                .ProjectTo<CatalogDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (resultList.Count == 0) {
                resultList = await _uow.Catalogs.All.Where(x => x.Id == (int) CatalogType.Open)
                    .OrderBy(x => x.Order)
                    .ProjectTo<CatalogDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }

            return resultList;
        }

        public async Task<CatalogDTO> GetCatalogDetailsAsync(CatalogType catalogId) {
            return _mapper.Map<CatalogDTO>(await _uow.Catalogs.GetByCatalogTypeAsync(catalogId));
        }
        
        public async Task<CatalogDTO> GetCatalogDetailsWithExtraChargesAsync(CatalogType catalogId) {
            return _mapper.Map<CatalogDTO>(await _uow.Catalogs.GetByCatalogTypeWithExtraChargesAsync(catalogId));
        }

        public async Task<CatalogDTO> GetCatalogByUriOrOpenCatalogAsync(string uri) {
            var catalog = await _uow.Catalogs.All.Where(x => x.Uri == uri).FirstOrDefaultAsync();
            if (catalog == null || catalog.HasClosedByTime() || (!catalog.IsVisible)) {
                return null;
            }

            return _mapper.Map<CatalogDTO>(catalog);
        }

        public async Task<OperationResult> InsertCatalog(CatalogDTO dto) {
            var entity = _mapper.Map<Catalog>(dto);
            var lastOrder = _uow.Catalogs.All.OrderByDescending(s => s.Order)
                .FirstOrDefault()?.Order;
            if (lastOrder == null)
                entity.Order = 1;
            else
                entity.Order = lastOrder.Value + 1;
            entity.IsVisible = true;

            if (dto.Preview != null) {
                entity.PreviewPath = await _fileService.SaveFileAsync(dto.Preview);
                if (string.IsNullOrEmpty(entity.PreviewPath)) {
                    return _fileService.GetLastError();
                }
            }

            await _uow.Catalogs.AddAsync(entity, true);
            await _uow.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateCatalog(CatalogDTO dto) {
            var entity = _mapper.Map(dto, await _uow.Catalogs.GetByCatalogTypeAsync(dto.Id));

            if (dto.Preview != null) {
                if (!string.IsNullOrEmpty(entity.PreviewPath)) {
                    if (!_fileService.RemoveFile(dto.Preview.FilesDirectoryPath, entity.PreviewPath)) {
                        return _fileService.GetLastError();
                    }
                }

                entity.PreviewPath = await _fileService.SaveFileAsync(dto.Preview);
                if (string.IsNullOrEmpty(entity.PreviewPath)) {
                    return _fileService.GetLastError();
                }
            }

            _uow.Catalogs.Update(entity);
            await _uow.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteCatalog(CatalogType id, string filesDirectoryPath) {
            var entity = await _uow.Catalogs.GetByCatalogTypeAsync(id);
            _fileService.RemoveFile(filesDirectoryPath, entity.PreviewPath);
            _uow.Catalogs.Delete(entity);
            await _uow.SaveChangesAsync();
            return OperationResult.Success();
        }
        
        public List<Catalog1CDTO> GetAllCatalogs() {
            
            return CatalogTypeExtensions.GetAllCatalogs().Select(x => new Catalog1CDTO(x)).ToList();
        }

        public async Task UpdateCatalogPosition(CatalogType id, int position) {
            var catalog = await _uow.Catalogs.GetByCatalogTypeAsync(id);
            if (catalog == null) throw new EntityNotFoundException($"Catalog {id} not found");

            catalog.Order = position;
            await _uow.SaveChangesAsync();
        }

        public async Task AddOrSetExtraChargeToCatalog(CatalogType id, ExtraChargeType extraChargeType, double sum) {
            var catalog = await _uow.Catalogs.GetByCatalogTypeWithExtraChargesAsync(id);
            if (catalog == null) throw new EntityNotFoundException($"Catalog {id} not found");

            var catalogExtraCharge = catalog.CatalogExtraCharges.FirstOrDefault(x => x.Type == extraChargeType);
            if(catalogExtraCharge == null) {
                catalogExtraCharge = new CatalogExtraCharge() {
                    CatalogId = id,
                    Type = extraChargeType,
                    IsActive = true,
                    Price = sum
                };
                catalog.CatalogExtraCharges.Add(catalogExtraCharge);
            } else {
                catalogExtraCharge.Price = sum;
            }

            await _uow.SaveChangesAsync();
        }

        public async Task<List<CatalogExtraChargeDTO>> GetCatalogExtraCharges(CatalogType id) {
            var catalogExtraCharges = await _uow.CatalogExtraCharges.All.Where(x => x.CatalogId == id).ToListAsync();
            return _mapper.Map<List<CatalogExtraChargeDTO>>(catalogExtraCharges);
        }

        public async Task DeleteCatalogExtraCharge(Guid id) {
            await _uow.CatalogExtraCharges.DeleteAsync(id);
            await _uow.SaveChangesAsync();
        }
    }
}
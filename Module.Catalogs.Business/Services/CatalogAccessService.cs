using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models.Session;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Module.Catalogs.Business.Common;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Catalogs.Business.UnitOfWork;

namespace Module.Catalogs.Business.Services {
    public class CatalogAccessService : ICatalogAccessService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CatalogAccessService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        public async Task ChangeCatalogAccessAsync(int userId, CatalogType catalogId, bool hasAccess) {
            await _uow.Users.SetCatalogVisibilityAsync(userId, catalogId, hasAccess);
            await _uow.SaveChangesAsync();
        }

        public async Task<Dictionary<CatalogType, bool>> GetAllUserCatalogs(int userId) {
            return await _uow.Users.GetUserCatalogsAsync(userId);
        }

        public async Task<List<Catalog1CDTO>> GetAvailableUserCatalogsAsync(int userId) {
            var catalogs = await _uow.Users.GetUserAvailableCatalogsOrOpenCatalogAsync(userId);
            return catalogs.Select(x => new Catalog1CDTO(x)).ToList();
        }

        public async Task<List<Catalog1CDTO>> GetAvailableUserCatalogsAsync(UserSession userSession) {
            return await userSession.GetAvailableCatalogsAsync(_uow);
        }

        public async Task<List<CatalogDTO>> GetAvailableUserCatalogsFullAsync(UserSession userSession) {
            var catalogs = (await userSession.GetAvailableCatalogsAsync(_uow)).Select(x => x.Id.ToProductCatalog1CId());
            return _mapper.Map<List<CatalogDTO>>(await _uow.Catalogs.All.Where(x => catalogs.Contains(x.Id))
                .ToListAsync());
        }
        
        public async Task<CatalogDTO> GetCatalogByIdOrUriIfAvailableAsync(UserSession userSession, string uri, CatalogType catalogId) {
            Catalog catalog = null;
            if (!string.IsNullOrEmpty(uri)) {
                catalog = await _uow.Catalogs.GetByUriAsync(uri);
            } else if (catalogId != CatalogType.All) {
                catalog = await _uow.Catalogs.GetByCatalogTypeAsync(catalogId);
            }

            if (catalog == null) throw new EntityNotFoundException($"Catalog not found by uri: {uri} or id: {catalogId}");         
            if (catalog.IsOpen) {
                return _mapper.Map<CatalogDTO>(catalog);
            }
            
            if (!await _uow.Users.HasAccessToCatalogAsync(userSession.UserId, catalog.Id)) {
                throw new CatalogAccessException(userSession.UserId, catalog.Id);
            }

            return _mapper.Map<CatalogDTO>(catalog);
        }

        public async Task<CatalogDTO> GetFirstAvailableCatalogAsync(UserSession userSession) {
            var availableCatalogs = await userSession.GetAvailableCatalogsAsync(_uow);
            if (availableCatalogs.Any(x => x.Id == (int) CatalogType.Preorder)) {
                return _mapper.Map<CatalogDTO>(await _uow.Catalogs.GetByCatalogTypeAsync(CatalogType.Preorder));
            }
            
            var catalog = await _uow.Catalogs.GetByCatalogTypeAsync(availableCatalogs.First().Id.ToProductCatalog1CId());
            return _mapper.Map<CatalogDTO>(catalog);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Session;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.Business.Interfaces {
    public interface ICatalogAccessService {
        Task ChangeCatalogAccessAsync(int userId, CatalogType catalogId, bool hasAccess);
        Task<Dictionary<CatalogType, bool>> GetAllUserCatalogs(int userId);
        Task<List<Catalog1CDTO>> GetAvailableUserCatalogsAsync(int userId);
        Task<List<Catalog1CDTO>> GetAvailableUserCatalogsAsync(UserSession userSession);
        Task<List<CatalogDTO>> GetAvailableUserCatalogsFullAsync(UserSession userSession);
        Task<CatalogDTO> GetCatalogByIdOrUriIfAvailableAsync(UserSession userSession, string uri, CatalogType catalogId);
        Task<CatalogDTO> GetFirstAvailableCatalogAsync(UserSession userSession);
    }
}
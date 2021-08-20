using System;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface ICatalogRepository : IRepository<Catalog, Guid> {
        Task<Catalog?> GetByCatalogTypeAsync(CatalogType type);
        Task<Catalog?> GetByCatalogTypeWithExtraChargesAsync(CatalogType type);
        Task<Catalog?> GetByUriAsync(string uri);
    }
}
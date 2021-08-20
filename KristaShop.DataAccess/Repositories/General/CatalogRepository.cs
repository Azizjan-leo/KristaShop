using System;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class CatalogRepository : Repository<Catalog, Guid>, ICatalogRepository {
        public CatalogRepository(DbContext context) : base(context) { }

        public async Task<Catalog?> GetByCatalogTypeAsync(CatalogType type) {
            return await All.FirstOrDefaultAsync(x => x.Id == type);
        }

        public async Task<Catalog?> GetByCatalogTypeWithExtraChargesAsync(CatalogType type) {
            return await All.Include(x => x.CatalogExtraCharges).FirstOrDefaultAsync(x => x.Id == type);
        }

        public async Task<Catalog?> GetByUriAsync(string uri) {
            return await All.FirstOrDefaultAsync(x => x.Uri == uri);
        }
    }
}
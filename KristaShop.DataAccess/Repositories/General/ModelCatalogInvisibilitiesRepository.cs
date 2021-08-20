using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class ModelCatalogInvisibilitiesRepository : Repository<ModelCatalogsInvisibility, int>, IModelCatalogInvisibilitiesRepository {
        public ModelCatalogInvisibilitiesRepository(DbContext context) : base(context) { }

        public async Task<List<int>> GetInvisibilityInCatalogIdsByArticulAsync(string articul) {
            return await All
                .Where(x => x.Articul == articul)
                .Distinct()
                .Select(x => (int)x.CatalogId).ToListAsync();
        }

        public async Task UpdateInvisibilityAsync(string articul, IEnumerable<CatalogType> catalogs) {
            var existsRecs = await All.Where(x => x.Articul == articul).ToListAsync();
            DeleteRange(existsRecs);
             await AddRangeAsync(catalogs.Select(catalog => new ModelCatalogsInvisibility(articul, catalog)));
        }
    }
}
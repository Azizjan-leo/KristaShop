using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface IModelCatalogInvisibilitiesRepository : IRepository<ModelCatalogsInvisibility, int> {
        Task<List<int>> GetInvisibilityInCatalogIdsByArticulAsync(string articul);
        Task UpdateInvisibilityAsync(string articul, IEnumerable<CatalogType> catalogs);
    }
}
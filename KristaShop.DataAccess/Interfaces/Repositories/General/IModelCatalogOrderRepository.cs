using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface IModelCatalogOrderRepository : IRepository<ModelCatalogOrder, object> {
        Task<ModelCatalogOrder> CreateOrUpdateAsync(string articul, CatalogType catalogId, int position);
        Task CheckAllModels();
    }
}
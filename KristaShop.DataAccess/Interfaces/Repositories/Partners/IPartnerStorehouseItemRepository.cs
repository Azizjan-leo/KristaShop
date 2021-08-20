using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Views.Partners;

namespace KristaShop.DataAccess.Interfaces.Repositories.Partners {
    public interface IPartnerStorehouseItemRepository : IRepository<PartnerStorehouseItem, Guid> {
        Task<PartnerStorehouseItem> GetStorehouseItemAsync(int modelId, int colorId, string sizeValue, int userId);
        IQueryable<PartnerStorehouseItemSqlView> GetStorehouseItems(int userId);
        Task<PartnerStorehouseItemSqlView> GetStorehouseItemAsync(string barcode, int userId);
    }
}

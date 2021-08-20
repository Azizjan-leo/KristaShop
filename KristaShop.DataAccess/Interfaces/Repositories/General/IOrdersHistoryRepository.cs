using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface IOrdersHistoryRepository : IRepository {
        Task<IEnumerable<OrderHistorySqlView>> GetUserItemsAsync(int userId);
        Task<IEnumerable<OrderHistorySqlView>> GetUserItemsAsync(int userId, IEnumerable<int> collectionIds);
        Task<IEnumerable<OrderHistorySqlView>> GetAllItemsAsync(ReportsFilter filter);
        Task<List<LookUpItem<int, string>>> GetAllOrderersAsync();
    }
}
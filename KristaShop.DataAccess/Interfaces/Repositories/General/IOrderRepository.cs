using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface IOrderRepository : IRepository {
        Task<IEnumerable<OrderDetailsFull>> GetUserUnprocessedOrderItemsAsync(int userId);
    }
}

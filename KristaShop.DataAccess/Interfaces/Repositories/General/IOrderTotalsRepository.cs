using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Views;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface IOrderTotalsRepository : IRepository {
        Task<IEnumerable<OrderTotalsSqlView>> GetUserTotalsAsync(int userId);
    }
}

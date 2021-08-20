using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Interfaces.Repositories {
    public interface IMoneyDocumentsTotalsRepository : IRepository {
        Task<MoneyDocumentsTotalSqlView> GetUserTotals(int userId);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Common.Business.Models;
using Module.Order.Business.Models;

namespace Module.Order.Business.Interfaces {
    public interface IOrderReportService {
        Task<ReportTotalsDTO> GetOrderTotals(bool unprocessedOnly = true, List<string> articuls = null,
            List<int> catalogIds = null, List<int> userIds = null, List<int> colorIds = null,
            List<int> cityIds = null, List<int> managerIds = null);

        Task<List<ClientOrdersTotalsDTO>> GetClientOrdersTotals(bool unprocessedOnly,
            bool onlyNotEmptyUserCarts, List<string> articuls = null,
            List<int> catalogIds = null, List<int> userIds = null, List<int> colorIds = null,
            List<int> cityIds = null, List<int> managerIds = null);

        Task<List<string>> GetUnprocessedOrdersArticulsAsync(bool processedOnly);
        Task<List<LookUpItem<int, string>>> GetUnprocessedOrdersUsersAsync(bool processedOnly);
    }
}
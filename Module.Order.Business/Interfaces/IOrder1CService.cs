using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Module.Common.Business.Models;
using Module.Order.Business.Models;

namespace Module.Order.Business.Interfaces {
    public interface IOrder1CService {
        Task<OperationResult> CreateOrderAsync(CreateOrderDTO createOrder, UserSession user);
        Task<OperationResult> CreateGuestOrderAsync(string fullName, string phone, GuestSession userSession);
        Task<OrderDTO> GetOrderAsync(int userId, int orderId);
        Task<List<OrderAdminDTO>> GetAllOrdersAsync(int orderId = 0, int managerId = 0, int modelId = 0, int colorId = 0,
            int[] ordersIDs = null, string catalogsMode = null, bool unprocessedOnly = false, bool includeUserCartCheck = false,
            bool onlyNotEmptyUserCarts = false, List<string> articuls = null,
            List<int> catalogIds = null, List<int> userIds = null, List<int> colorIds = null, List<int> cityIds = null,
            List<int> managerIds = null, bool onlyVisibleForManager = false);
        Task<OrderAdminDTO> GetOrderAdminAsync(int orderId);
        Task<OperationResult> DeletePositionAsync(int orderId, int id, bool needTransaction = true);
        Task<OperationResult> DeleteOrderAsync(int id, string mode = "");
        Task<OperationResult> CombineOrdersAsync(int[] ordersIDs);
        Task<int> GetMaxItemAmountAsync(OrderDetailsDTO orderItem);
        Task<OperationResult> UpdateOrderItemAmountAsync(OrderDetailsDTO orderItem, int linesCount);
        Task<OperationResult> AddModelToOrderAsync(int orderId, int catalogId, int modelId, int colorId, int nomenclatureId, int amount);
        Task<List<OrderDetailsDTO>> GetPreorderTotalReportAsync();
        Task<List<string>> GetAllSizesValuesFromOrdersAsync();
        Task<List<Color1CDTO>> GetAllColorsValuesFromOrdersAsync();
        Task<List<OrderDetailsDTO>> GetOrdersTotalReportAsync(OrdersTotalFilterDTO filterValues);

        Task<List<InvoiceDTO>> GetAllInvoicesListAsync();
        Task<List<InvoiceDTO>> GetInvoicesListByUserIdAsync(int userId, bool includePayedInvoices = true);
        Task<InvoiceDTO> GetInvoiceByIdAsync(int id);

        Task<string> GetModelDescriptionAsync(int modelId, int colorId, string mode = null);

        Task<List<RequestAdminDTO>> getAllRequestsAsync();
        Task<List<ManufactureAdminDTO>> getAllManufactureAsync();
        Task<List<ReservationAdminDTO>> getAllReservationsAsync();
        Task<List<ShipingAdminDTO>> getAllShipingsAsync();
        Task<(double pCount, double pSum, double sCount, double sSum, int oCount)> getUnprocessedOrdersStatsAsync(int? managerId = null);
        Task<bool> CheckAsReviewedAsync(int orderId, UserSession user);
    }
}

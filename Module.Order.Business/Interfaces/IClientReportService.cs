using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using Module.Common.Business.Models;
using Module.Order.Business.Models;

namespace Module.Order.Business.Interfaces {
    public interface IClientReportService {
        Task<ItemsGrouped<ItemsGroupedWithDate<RequestsItemDTO>>> GetGroupedUserRequestItemsAsync(int userId);
        Task<ItemsGrouped<ItemsGrouped<ManufacturingItemDTO>>> GetGroupedUserManufactureItemsAsync(int userId);
        Task<ItemsGrouped<ItemsGroupedWithDate<ReservationsItemDTO>>> GetGroupedUserReservationsAsync(int userId);
        Task<ItemsGrouped<ItemsGroupedWithDateName<ShipmentsItemDTO>>> GetGroupedUserShipmentsAsync(int userId, DateTime shipmentDate = default);
        Task<UnprocessedItemsGrouped<UnprocessedOrderItemDTO>> GetUserUnprocessedItemsAsync(int userId);
        Task<ItemsGrouped<ItemsGroupedWithDate<OrderHistoryItemDTO>>> GetUserHistoryItemsAsync(int userId);
        Task<IEnumerable<MoneyDocumentDTO>> GetUserMoneyReportsAsync(int userId);
        Task<MoneyDocumentsTotalDTO> GetUserMoneyTotalsAsync(int userId);
        Task<UserComplexOrderDTO> GetUserComplexOrderAsync(int userId);
        Task<List<SimpleGroupedModelDTO<RequestsItemDTO>>> GetAllOrdersAsync(ReportsFilter filter);
        Task<Dictionary<OrderTotalsReportType, TotalSum>> GetUserOrdersTotalsAsync(int userId);
        Task<List<LookUpItem<int, string>>> GetOrderersLookupAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Structs;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;

namespace Module.Partners.Business.Interfaces {
    public interface IPartnerStorehouseService {
        Task<List<PartnerStorehouseItemDTO>> GetStorehouseItemsAsync(int userId);
        Task<ItemsGroupedBase<ModelGroupedDTO, PaymentTotalInfo>> GetStorehouseItemsGroupedAsync(int userId);
        Task<PartnerStorehouseItemDTO> GetStorehouseItemAsync(string barcode, int userId);
        Task<List<PartnerStorehouseItemDTO>> GetHistoryItems(int? userId, DateTimeOffset date = default, MovementDirection movementDirection = MovementDirection.None, MovementType movementType = MovementType.None, bool isAmountPositive = false);
        Task<List<BarcodeShipmentItemDTO>> GetShipmentsAsync(int userId);
        Task<List<ItemsWithTotalsDTO<ModelGroupedDTO>>> GetShipmentsGroupedAsync(int userId);
        Task AutoIncomeShipmentItemsAsync(int userId, DateTime reservationDate);
        Task IncomeShipmentItemsAsync(BarcodeAmountDTO barcodeAmount);
        Task SellStorehouseItemAsync(SellingDTO selling);
        Task AuditStorehouseItemsAsync(BarcodeAmountDTO revision);
    }
}
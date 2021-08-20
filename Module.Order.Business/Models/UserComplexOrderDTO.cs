using Module.Common.Business.Models;

namespace Module.Order.Business.Models {
    public class UserComplexOrderDTO {
        public ItemsGrouped<ItemsGroupedWithDateName<BasicOrderItemDTO>> ProcessedItems { get; set; }
        public ItemsGrouped<ItemsGroupedWithName<OrderHistoryItemDTO>> OrderedItems { get; set; }
        public ItemsGrouped<ManufactureItemsGrouped<ManufacturingItemDTO>> ManufactureItems { get; set; }
        public ItemsGrouped<ItemsGroupedWithName<ReservationsItemDTO>> Reservations { get; set; }
        public ItemsGrouped<ItemsGroupedWithName<ItemsGroupedWithDateName<ShipmentsItemDTO>>> Shipments { get; set; }
        
        public bool HasAnyItems() {
            return OrderedItems.Totals.TotalAmount > 0
                   || ManufactureItems.Totals.TotalAmount > 0
                   || Reservations.Totals.TotalAmount > 0
                   || Shipments.Totals.TotalAmount > 0;
        }
    }
}
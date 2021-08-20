namespace Module.Order.Business.Models {
    public class OrderHistoryItemDTO : BasicOrderItemDTO {
        public override string ItemStatus => "Заказано";
    }
}

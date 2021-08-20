namespace Module.Order.Business.Models {
    public class ReservationsItemDTO : BasicOrderItemDTO {
        public override string ItemStatus => "В резерве";
    }
}
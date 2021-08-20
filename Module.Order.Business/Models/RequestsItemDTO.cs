namespace Module.Order.Business.Models {
    public class RequestsItemDTO : BasicOrderItemDTO {
        public override string ItemStatus => "Предзаказ в обработке";
    }
}
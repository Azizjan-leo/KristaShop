namespace Module.Order.Business.Models {
    public class ShipmentsItemDTO : BasicOrderItemDTO {
        public string DocumentsFolder { get; set; }
        public override string ItemStatus => "Отправлено";
    }
}
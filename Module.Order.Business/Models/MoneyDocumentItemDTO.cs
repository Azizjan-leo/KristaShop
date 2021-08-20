namespace Module.Order.Business.Models {
    public class MoneyDocumentItemDTO : BasicOrderItemDTO {
        public int UserId { get; set; }
        public int DocumentId { get; set; }
    }
}
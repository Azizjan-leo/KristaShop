using KristaShop.Common.Enums;

namespace Module.Order.Business.Models {
    public class UnprocessedOrderItemDTO : BasicOrderItemDTO {
        public int CatalogId { get; set; }
        public string CatalogName { get; set; }

        public override string ItemStatus {
            get {
                return $"Ожидает обработки ({(CatalogId == (int)CatalogType.Preorder ? "предзаказ" : "наличие")})";
            }
        }
    }
}
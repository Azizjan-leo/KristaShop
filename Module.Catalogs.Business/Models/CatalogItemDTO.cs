using KristaShop.Common.Models.Structs;
using Module.Common.Business.Models;

namespace Module.Catalogs.Business.Models {
    public class CatalogItemDTO {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public string ModelName { get; set; }
        public SizeValue Size { get; set; }
        public string Material { get; set; }
        public ColorDTO Color { get; set; }
        public int Amount { get; set; }
        public int RealAmount { get; set; }
        public int Status { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public bool IsVisible { get; set; }
        public int CatalogId { get; set; }
        public double LinePrice => Size.Parts * Price;
        public double LinePriceInRub => Size.Parts * PriceInRub;
    }
}
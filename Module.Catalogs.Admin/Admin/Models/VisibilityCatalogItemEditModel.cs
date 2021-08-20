using KristaShop.Common.Enums;

namespace Module.Catalogs.Admin.Admin.Models {
    public class VisibilityCatalogItemEditModel {
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string SizeValue { get; set; }
        public string SizeLine { get; set; }
        public int ColorId { get; set; }
        public CatalogType CatalogId { get; set; }
        public bool IsVisible { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Enums;

namespace Module.Catalogs.Admin.Admin.Models {
    public class EditModelCatalogOrderViewModel {
        public CatalogType CatId { get; set; }
        public string Articul { get; set; }
        public int ToPosition { get; set; }
    }
}

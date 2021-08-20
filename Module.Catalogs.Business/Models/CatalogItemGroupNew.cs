using System.Collections.Generic;
using Module.Common.Business.Models;

namespace Module.Catalogs.Business.Models {
    public class CatalogItemGroupNew {
        public CatalogItemDescriptorDTO Descriptor { get; set; }
        public int CatalogId { get; set; }
        public int Order { get; set; }
        public double CommonPrice { get; set; }
        public double CommonPriceInRub { get; set; }
        public List<SizeGroup> CatalogItems { get; set; }
        public List<string> Sizes { get; set; }
        public List<ColorDTO> Colors { get; set; }
        public List<Catalog1CDTO> InCatalogs { get; set; }
        public List<Category1CDTO> InCategories { get; set; }
        public List<ModelPhotoDTO> Photos { get; set; }
    }
}
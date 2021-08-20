using System.Collections.Generic;
using Module.Catalogs.Business.Models;
using Module.Common.Business.Models;

namespace Module.Catalogs.Admin.Admin.Models {
    public class CatalogItem1CBriefViewModel {
        public string Articul { get; set; }
        public string Name { get; set; }
        public int CatalogId { get; set; }
        public int Order { get; set; }
        public bool IsVisible { get; set; }
        public string MainPhoto { get; set; }
        public List<CatalogItemDTO> CatalogItems { get; set; }
        public List<Catalog1CDTO> AllCatalogs { get; set; }
    }
}
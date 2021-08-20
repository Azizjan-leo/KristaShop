using System.Collections.Generic;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.WebUI.Models {
    public class CatalogWithItemsViewModel {
        public CatalogDTO Catalog { get; set; }
        public IEnumerable<CatalogItemBriefDTO> Items { get; set; }
    }
}

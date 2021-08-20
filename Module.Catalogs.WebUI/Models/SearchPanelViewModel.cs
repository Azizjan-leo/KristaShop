using System.Collections.Generic;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.WebUI.Models {
    public class SearchPanelViewModel {
        public SearchLookupsViewModel SearchLookups { get; set; }
        public NomFilterDTO NomFilter { get; set; }
        public SelectList Catalogs { get; set; }
    }

    public class SearchLookupsViewModel {
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public IEnumerable<LookUpItem<int, string>> Colors { get; set; }
        public IEnumerable<string> Sizes { get; set; }
        public IEnumerable<string> SizeLines { get; set; }
    }
}
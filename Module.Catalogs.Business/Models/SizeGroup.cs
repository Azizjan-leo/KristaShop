using System.Collections.Generic;
using System.Linq;
using Module.Common.Business.Models;

namespace Module.Catalogs.Business.Models {
    public class SizeGroup {
        public string SizeValue { get; set; }
        public List<ColorDTO> Colors { get; set; }
        public List<CatalogItemDTO> Items { get; set; }

        public SizeGroup(string sizeValue, List<CatalogItemDTO> items) {
            SizeValue = sizeValue;
            Items = items;
            Colors = items.Select(x => x.Color).Distinct().ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;

namespace KristaShop.Common.Models.Session {
    public class GuestAccessInfo {
        public bool IsPreoderVisible { get; set; }
        public bool IsInstockByLinesVisible { get; set; }
        public bool IsInstockByPartsVisible { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string Hash { get; set; }

        public List<CatalogType> GetOnlyAvailableCatalogsOrOpenCatalog() {
            var result = new List<CatalogType>();

            if (IsInstockByLinesVisible) {
                result.Add(CatalogType.InStockLines);
            }

            if (IsInstockByPartsVisible) {
                result.Add(CatalogType.InStockParts);
            }

            if (IsPreoderVisible) {
                result.Add(CatalogType.Preorder);
            }

            if (!result.Any()) {
                result.Add(CatalogType.Open);
            }

            return result;
        } 
    }
}
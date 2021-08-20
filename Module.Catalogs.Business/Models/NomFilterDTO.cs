using System.Collections.Generic;
using KristaShop.Common.Enums;

namespace Module.Catalogs.Business.Models
{
    public class NomFilterDTO {
        private string _articul;
        
        public CatalogType CatalogId { get; set; }
        public string CatalogUri { get; set; }
        public bool IsSet { get; set; }

        public string Articul {
            get => _articul;
            set => _articul = value ?? string.Empty;
        }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public List<int> Categories { get; set; }
        public List<int> Colors { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> SizeLines { get; set; }
        public CatalogOrderType OrderType { get; set; }
        public CatalogOrderDir OrderDirection { get; set; }

        public NomFilterDTO() {
            CatalogId = CatalogType.All;
            Articul = string.Empty;
            Categories = new List<int>();
            Colors = new List<int>();
            Sizes = new List<string>();
            SizeLines = new List<string>();
            OrderType = CatalogOrderType.OrderByPosition;
            OrderDirection = CatalogOrderDir.Asc;
        }

        public void SetDataFromCatalog(CatalogDTO catalog) {
            CatalogId = catalog.Id;
            CatalogUri = catalog.Uri;
            IsSet = catalog.IsSet;
        }
    }
}
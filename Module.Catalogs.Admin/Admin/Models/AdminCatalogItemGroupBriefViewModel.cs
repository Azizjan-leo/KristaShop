using System;

namespace Module.Catalogs.Admin.Admin.Models {
    public class AdminCatalogItemBriefViewModel {
        public string Articul { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Price { get; set; }
        public string PriceInRub { get; set; }
        public int Amount { get; set; }
        public bool IsVisible { get; set; }
        public string MainPhoto { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string[] Colors { get; set; }
        public string[] Sizes { get; set; }
        public string[] Catalogs { get; set; }
        public string[] Categories { get; set; }
    }
}
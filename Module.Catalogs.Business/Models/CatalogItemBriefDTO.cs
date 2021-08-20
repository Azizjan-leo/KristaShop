using System;

namespace Module.Catalogs.Business.Models {
    public class CatalogItemBriefDTO {
        public string Articul { get; set; }
        public bool IsVisible { get; set; }
        public int CatalogId { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }       
        public int Order { get; set; }
        public string MainPhoto { get; set; }
        public string AltText { get; set; }
        public DateTime? AddDate { get; set; }
        public string Description { get; set; }
        public bool IsLimited { get; set; }
    }
}

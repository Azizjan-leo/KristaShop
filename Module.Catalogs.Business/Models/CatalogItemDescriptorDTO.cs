using System;

namespace Module.Catalogs.Business.Models {
    public class CatalogItemDescriptorDTO {
        public string Articul { get; set; }
        public string MainPhoto { get; set; }
        public bool? IsVisible { get; set; }
        public DateTime? AddDate { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public string AltText { get; set; }
        public string VideoLink { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public bool IsLimited { get; set; }
    }
}
using System;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.CatalogItemDescriptorConfiguration"/>
    /// </summary>
    public class CatalogItemDescriptor {
        public string Articul { get; set; }
        public bool? IsVisible { get; set; }
        public string MainPhoto { get; set; }
        public DateTime AddDate { get; set; }
        public string Description { get; set; }
        public string Matherial { get; set; }
        public string AltText { get; set; }
        public string VideoLink { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public bool IsLimited { get; set; }

        public CatalogItemDescriptor() { }

        public CatalogItemDescriptor(string articul) {
            Articul = articul;
            Description = "";
            Matherial = "";
            AddDate = DateTime.Now;
            MainPhoto = "";
        }
    }
}
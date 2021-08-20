using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.Models;

namespace Module.Catalogs.Business.Models {
    public class CatalogDTO {
        public CatalogType Id { get; set; }
        public string Catalog1CName { get; set; }
        public string Name { get; set; }
        public int OrderForm { get; set; }
        public int NomCount { get; set; }
        public string OrderFormName { get; set; }
        public string Uri { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string AdditionalDescription { get; set; }
        public int Order { get; set; }
        public bool IsDisableDiscount { get; set; }
        public bool IsVisible { get; set; }
        public bool IsOpen { get; set; }
        public bool IsSet { get; set; }
        public IFileDataProvider Preview { get; set; }
        public string PreviewPath { get; set; }
        public string VideoPath { get; set; }
        public DateTimeOffset? CloseTime { get; set; }
        public ICollection<CatalogExtraChargeDTO> CatalogExtraCharges { get; set; }

        public void AssignCatalog1CName(IEnumerable<Catalog1CDTO> allCatalogs) {
            var catalog1C = allCatalogs.FirstOrDefault(i => i.Id == (int) Id);
            Catalog1CName = catalog1C == null ? "---" : catalog1C.Name;
        }
    }
}
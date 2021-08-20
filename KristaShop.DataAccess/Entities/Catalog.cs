using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.CatalogConfiguration"/>
    /// </summary>
    public class Catalog : IEntityKeyGeneratable {
        private Guid _id { get; set; }
        public CatalogType Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public OrderFormType OrderForm { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string AdditionalDescription { get; set; }
        public int Order { get; set; }
        public bool IsDiscountDisabled { get; set; }
        public bool IsVisible { get; set; }
        public bool IsOpen { get; set; }
        public bool IsSet { get; set; }
        public string PreviewPath { get; set; } = string.Empty;
        public string VideoPath { get; set; } = string.Empty;
        public DateTimeOffset? CloseTime { get; set; }
        public ICollection<CatalogExtraCharge> CatalogExtraCharges { get; set; }
        public ICollection<CatalogItem> CatalogItems { get; set; }

        public bool HasClosedByTime() => CloseTime != null && CloseTime < DateTimeOffset.UtcNow;

        public bool NeedCheckStorehouseRests() => OrderForm == OrderFormType.InStock;

        public Catalog() { }

        public Catalog(Guid catalogId) {
            _id = catalogId;
        }
        
        public void GenerateKey() {
            _id = Guid.NewGuid();
        }
    }
}
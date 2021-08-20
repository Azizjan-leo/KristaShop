using System;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models.Structs;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.CatalogItemVisibilityConfiguration"/>
    /// </summary>
    public class CatalogItemVisibility : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string SizeValue { get; set; }
        public int ColorId { get; set; }
        public CatalogType CatalogId { get; }
        public bool IsVisible { get; set; }
        public SizeValue Size => new(SizeValue);
        
        public CatalogItemVisibility() { }

        public CatalogItemVisibility(string articul, int modelId, SizeValue size, int colorId, CatalogType catalogId, bool isVisible) {
            Articul = articul;
            ModelId = modelId;
            SizeValue = size.Value;
            ColorId = colorId;
            CatalogId = catalogId;
            IsVisible = isVisible;
        }

        public void GenerateKey() {
            Id = Guid.Empty;
        }
    }
}
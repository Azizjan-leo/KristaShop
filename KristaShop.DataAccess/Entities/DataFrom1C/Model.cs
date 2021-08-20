#nullable enable
using System.Collections.Generic;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Configurations.DataFrom1C;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="ModelConfiguration"/>
    /// </summary>
    public class Model : EntityBase<int> {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Articul { get; set; }
        public int Status { get; set; }
        public int Parts { get; set; }
        public float Weight { get; set; }
        public string SizeLine { get; set; }
        public int? MaterialId { get; set; }
        public int Discount { get; set; }
        public double Price { get; set; }
        public int? CollectionId { get; set; }
        public Collection? Collection { get; set; }
        public CatalogItemDescriptor? Descriptor { get; set; }
        public Material? Material { get; set; }
        public ICollection<ModelCategory>? Categories { get; set; }
        public ICollection<CatalogItem> CatalogItems { get; set; }
        public ICollection<Barcode> Barcodes { get; set; }

        public override int GetId() {
            return Id;
        }
    }
}
using System;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.CatalogItemConfiguration"/>
    /// </summary>
    public class CatalogItem {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Articul { get; set; }
        public int ColorId { get; set; }
        public string SizeValue { get; set; }
        public int NomenclatureId { get; set; }
        public int Amount { get; set; }
        public CatalogType CatalogId { get; set; }
        public double Price { get; set; }
        public double PriceRub { get; set; }
        public DateTime ExecutionDate { get; set; }
        public ModelCatalogOrder ModelCatalogOrder { get; set; }
        public ModelCatalogsInvisibility Invisibility { get; set; }
        public CatalogItemVisibility ItemVisibility { get; set; }
        
        public Model Model { get; set; }
        public Color Color { get; set; }
        public Catalog Catalog { get; set; }
    }
}
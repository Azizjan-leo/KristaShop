using System;
using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.OrderDetailsConfiguration"/>
    /// </summary>
    public class OrderDetails : ICloneable {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public CatalogType CatalogId { get; set; }
        public int NomenclatureId { get; set; }
        public int ModelId { get; set; }
        public string SizeValue { get; set; }
        public int ColorId { get; set; }
        public int StorehouseId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        
        public string Articul { get; set; }
        public string ColorName { get; set; }
        public int? CollectionId { get; set; }

        public Order Order { get; set; }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}

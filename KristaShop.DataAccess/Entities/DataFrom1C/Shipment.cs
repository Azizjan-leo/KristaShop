using System;
using KristaShop.DataAccess.Configurations.DataFrom1C;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="ShipmentConfiguration"/>
    /// </summary>
    public class Shipment {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public string SizeValue { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int AttachmentsNumber { get; set; }
        public int? CollectionId { get; set; }

        public User User { get; set; }
        public Model Model { get; set; }
        public Color Color { get; set; }
        public Collection? Collection { get; set; }
    }
}
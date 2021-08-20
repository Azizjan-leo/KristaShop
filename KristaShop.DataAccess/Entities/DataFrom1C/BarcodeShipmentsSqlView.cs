using System;
using System.Collections.Generic;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.Interfaces.DataFrom1C;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.BarcodeShipmentsSqlViewConfiguration"/>
    /// </summary>
    public class BarcodeShipmentsSqlView : IBarcodeShipmentsSqlView {
        public int Id { get; set; }
        public string Articul { get; set; }
        public string Name { get; set; }
        public int ModelId { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }
        public int UserId { get; set; }
        public string PhotoByColor { get; set; }
        public string MainPhoto { get; set; }
        public string ColorName { get; set; }
        public string ColorPhoto { get; set; }
        public string ColorValue { get; set; }
        public int PartsCount { get; set; }
        public DateTime SaleDate { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public List<string> Barcodes { get; set; }
        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
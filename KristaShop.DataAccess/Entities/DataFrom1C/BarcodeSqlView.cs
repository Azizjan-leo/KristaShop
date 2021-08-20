using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;
using System.Collections.Generic;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.BarcodeSqlViewConfiguration"/>
    /// </summary>
    public class BarcodeSqlView : ISingleBarcodeCatalogItem {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public SizeValue Size { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}

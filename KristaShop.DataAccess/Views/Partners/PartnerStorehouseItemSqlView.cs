using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;

namespace KristaShop.DataAccess.Views.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Partners.PartnerStorehouseItemSqlViewConfiguration"/>
    /// </summary>
    public class PartnerStorehouseItemSqlView : IBarcodesCountableCatalogItem {
        public int UserId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string Name{ get; set; }
        public string MainPhoto { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorGroupName { get; set; }
        public string ColorCode { get; set; }
        public string ColorImage { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public PartnerOrderType OrderType { get; set; }
        public List<string> Barcodes { get; set; }
        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
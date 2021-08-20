using System;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Views.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="PartnerStorehouseHistoryItemSqlViewConfiguration"/>
    /// </summary>
    public class PartnerStorehouseHistoryItemSqlView : ICountableCatalogItem {
        public int UserId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string MainPhoto { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public string ColorImage { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public MovementDirection Direction { get; set; }
        public MovementType MovementType { get; set; }
        public DateTime CreateDate { get; set; }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}

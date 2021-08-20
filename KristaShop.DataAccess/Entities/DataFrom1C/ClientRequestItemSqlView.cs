using System;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.ClientRequestItemSqlViewConfiguration"/>
    /// </summary>
    public class ClientRequestItemSqlView : ICountableCatalogItem {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string PhotoByColor { get; set; }
        public string MainPhoto { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorPhoto { get; set; }
        public string ColorValue { get; set; }
        public SizeValue Size { get; set; }
        public int Amount { get; set; }
        public DateTime RequestDate { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}

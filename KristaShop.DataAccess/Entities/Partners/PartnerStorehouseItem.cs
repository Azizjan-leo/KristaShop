using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Partners.PartnerStorehouseItemConfiguration"/>
    /// </summary>
    public class PartnerStorehouseItem : IBarcodesCountableCatalogItem {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string SizeValue { get; set; }
        public SizeValue Size {
            get => new SizeValue(SizeValue);
            set => SizeValue = value.Value;
        }

        public int ColorId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public PartnerOrderType OrderType { get; set; }
        public DateTimeOffset IncomeDate { get; set; }
        public List<string> Barcodes { get; set; }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}

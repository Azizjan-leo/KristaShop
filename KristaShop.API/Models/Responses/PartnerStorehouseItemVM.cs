using System;
using System.Collections.Generic;

namespace KristaShop.API.Models.Responses {
    public class PartnerStorehouseItemVM {
        public List<string> Barcodes { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string SizeValue { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int Amount { get; set; }
        public int PartsCount { get; set; }
        public int ActualAmount => Amount / PartsCount;
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public DateTime Date { get; set; }
    }
}

using System;
using System.Collections.Generic;
using KristaShop.Common.Models.Structs;

namespace Module.Common.Business.Models {
    public class BarcodeShipmentItemDTO {
        public int Id { get; set; }
        public List<string> Barcodes { get; set; }
        public int ModelId { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }
        public int UserId { get; set; }
        public string Articul { get; set; }
        public string Name => Articul;
        public string PhotoByColor { get; set; }
        public string MainPhoto { get; set; }
        public string ColorName { get; set; }
        public string ColorPhoto { get; set; }
        public string ColorValue { get; set; }
        public int PartsCount { get; set; }
        public DateTime SaleDate { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }

        public string ItemGroupingKey => $"{Articul.Replace(" ", "_")}_{Size.Line}";
        public string ItemGroupingKey2 => $"{Articul}_{Size.Value}_{ColorId}";
    }
}

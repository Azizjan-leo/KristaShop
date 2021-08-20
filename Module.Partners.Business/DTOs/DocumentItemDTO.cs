using System;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;

namespace Module.Partners.Business.DTOs {
    public class DocumentItemDTO : ICountableCatalogItem {
        public Guid Id { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        
        public object Clone() {
            return MemberwiseClone();
        }
    }
}
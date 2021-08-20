using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Structs;

namespace Module.Partners.Business.DTOs {
    /// <summary>
    /// Mapping for this DTO <see cref="Mappings.PartnersMappingProfile"/>
    /// </summary>
    public class PartnerStorehouseItemDTO {
        public List<string> Barcodes { get; set; }
        public int UserId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string MainPhoto { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public string ColorCode { get; set; }
        public SizeValue Size { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public PartnerOrderType OrderType { get; set; }
        public string ItemGroupingKey => $"{Articul}_{Size.Line}";
        public string ItemGroupingKey2 => $"{Articul}_{Size.Value}_{ColorId}";
    }
}
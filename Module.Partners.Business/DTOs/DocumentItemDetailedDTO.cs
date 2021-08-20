using System;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models.Structs;

namespace Module.Partners.Business.DTOs {
    public class DocumentItemDetailedDTO {
        public Guid Id { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public string ColorCode { get; set; }
        public string MainPhoto { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public DateTimeOffset Date { get; set; }
        public string DateFormatted => Date.ToBasicString();
        
        public string FromDocumentName { get; set; }
        public double TotalPrice => Price * Amount;
        
        public virtual string ItemGroupingKey => $"{Articul}_{Size.Line}";
    }
}
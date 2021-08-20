using System;
using KristaShop.Common.Extensions;

namespace Module.Partners.Business.DTOs {
    public class DocumentMoneyItemsDetailedDTO : DocumentItemDetailedDTO {
        public DateTime Date { get; set; }
        public string DateFormatted => Date.ToBasicString();
        public string DocumentType { get; set; }
        public override string ItemGroupingKey => $"{Articul}_{Size.Line}_{DateFormatted}";
    }
}
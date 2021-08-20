using System;
using KristaShop.Common.Extensions;

namespace Module.Partners.Business.DTOs {
    public class DocumentMovementAmountsDTO {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ulong Number { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public int IncomeAmount { get; set; }
        public int WriteOffAmount { get; set; }
        public string FormattedNumber => Number.ToString("D5");
        public string CreateDateAsString => CreateDate.ToBasicString();
        public bool CanHaveGroupedItems { get; set; }
    }
}
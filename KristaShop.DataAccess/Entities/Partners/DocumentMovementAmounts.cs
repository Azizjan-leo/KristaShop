using System;

namespace KristaShop.DataAccess.Entities.Partners {
    public class DocumentMovementAmounts {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ulong Number { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public int IncomeAmount { get; set; }
        public int WriteOffAmount { get; set; }
        public bool CanHaveGroupedItems { get; set; }
    }
}
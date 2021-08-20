using System;
using KristaShop.Common.Models.Structs;
using Module.Common.Business.Models;

namespace Module.Partners.Business.DTOs {
    public class StorehouseMovementItemDTO {
        public int InitialAmount { get; set; }
        public int IncomeAmount { get; set; }
        public int WriteOffAmount { get; set; }
        public int CurrentAmount { get; set; }
        public ColorDTO Color { get; set; }
        public SizeValue Size { get; set; }
    }

    public class DocumentMovementItemDTO {
        public DateTimeOffset CreateDate { get; set; }
        public string Name { get; set; }
        public ulong Number { get; set; }
        public int InitialAmount { get; set; }
        public int IncomeAmount { get; set; }
        public int WriteOffAmount { get; set; }
        public int CurrentAmount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;

namespace Module.Order.Business.Models {
    public class MoneyDocumentDTO {
        public int Id { get; set; }
        public string Number { get; set; }
        public MoneyDocumentType Name { get; set; }
        public int UserId { get; set; }
        public double InitialBalance { get; set; }
        public double FinalBalance { get; set; }
        public double ToPay { get; set; }
        public double Income { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<MoneyDocumentItemDTO> Items { get; set; }
    }
}
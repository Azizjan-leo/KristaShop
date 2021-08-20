using System;

namespace Module.Order.Business.Models {
    public class MoneyDocumentsTotalDTO {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double InitialBalance { get; set; }
        public double FinalBalance { get; set; }
        public double ToPay { get; set; }
        public double Income { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
    }
}
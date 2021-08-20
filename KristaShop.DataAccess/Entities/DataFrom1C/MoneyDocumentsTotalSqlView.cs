using System;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.MoneyDocumentsTotalSqlViewConfiguration"/>
    /// </summary>
    public class MoneyDocumentsTotalSqlView {
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
using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.MoneyDocumentSqlViewConfiguration"/>
    /// </summary>
    public class MoneyDocumentSqlView {
        public int Id { get; set; }
        public string Number { get; set; }
        public MoneyDocumentType Name { get; set; }
        public int UserId { get; set; }
        public double InitialBalance { get; set; } // balance the client has before payment
        public double FinalBalance { get; set; }  // balance the client has after the payment
        public double ToPay { get; set; } // sum the client should pay
        public double Income { get; set; } // sum the client paid
        public DateTime CreateDate { get; set; }
        public IEnumerable<MoneyDocumentItemSqlView> Items { get; set; }
    }
}
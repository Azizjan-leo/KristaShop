using System;
using System.Collections.Generic;
using System.Text;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.InvoiceSqlConfiguration"/>
    /// </summary>
    public class InvoiceSql {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string CityName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerFullName { get; set; }
        public string InvoiceClientTitle { get; set; }
        public DateTime CreateDate { get; set; }
        public string InvoiceNum { get; set; }
        public double PrePay { get; set; }
        public double TotPay { get; set; }
        public string Currency { get; set; }
        public double ExchangeRate { get; set; }
        public bool WasPayed { get; set; }
        public int IsPrepay { get; set; }
    }
}

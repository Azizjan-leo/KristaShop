using System;
using KristaShop.DataAccess.Configurations.Views;

namespace KristaShop.DataAccess.Views {
    /// <summary>
    /// Configuration file for this entity <see cref="ReportTotalsConfiguration"/>
    /// </summary>

    public class UserCartTotals {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int TotalItemsCount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceRub { get; set; }
    }
}
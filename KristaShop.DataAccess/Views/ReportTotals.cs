using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Views {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Views.ReportTotalsConfiguration"/>
    /// </summary>
    public class ReportTotals {
        public int TotalAmount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceInRub { get; set; }
        public CatalogType CatalogId { get; set; }
    }
}

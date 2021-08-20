using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Views {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Views.OrderTotalsSqlViewConfiguration"/>
    /// </summary>
    public class OrderTotalsSqlView {
        public double Sum { get; set; }
        public double SumInRub { get; set; }
        public int PrepayPercent { get; set; }
        public OrderTotalsReportType Type { get; set; }
    }
}

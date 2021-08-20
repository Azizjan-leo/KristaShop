using KristaShop.Common.Models.Structs;

namespace Module.Common.Business.Models {
    public class ReportTotalsDTO {
        public ReportTotalInfo Totals { get; set; }
        public ReportTotalInfo PreorderTotals { get; set; }
        public ReportTotalInfo InStockLinesTotals { get; set; }
        public ReportTotalInfo InStockPartsTotals { get; set; }
    }
}

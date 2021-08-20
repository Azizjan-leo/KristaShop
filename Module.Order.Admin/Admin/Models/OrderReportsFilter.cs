using KristaShop.Common.Models.Filters;

namespace Module.Order.Admin.Admin.Models {
    public class OrderReportsFilter: ReportsFilter {
        public bool OnlyNotEmptyUserCarts { get; set; }
        public bool OnlyUnprocessedOrders { get; set; }
    }
}
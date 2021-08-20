using System.Threading.Tasks;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Models;
using Module.Order.Business.Interfaces;

namespace Module.Order.Admin.Admin.ViewComponents {
    public class NewOrdersViewComponent : ViewComponentBase {
        private readonly IOrder1CService _orderService;

        public NewOrdersViewComponent(IOrder1CService orderService) {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentOutValue<int> itemsCount) {
            var (pCount, pSum, sCount, sSum, ordersCount) = UserSession.IsRoot
                ? await _orderService.getUnprocessedOrdersStatsAsync()
                : await _orderService.getUnprocessedOrdersStatsAsync(UserSession.ManagerId);

            itemsCount.Value = ordersCount; 

            var preorderTotals = new ReportTotalInfo((int) pCount, pSum, 0);
            var inStockTotals = new ReportTotalInfo((int) sCount, sSum, 0);
            return View((preorderTotals, inStockTotals, ordersCount));
        }
    }
}
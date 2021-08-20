using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Order.Business.Interfaces;
using Serilog;

namespace Module.Order.WebUI.ViewComponents {
    public class MoneyReportTotals : ViewComponentBase {
        private readonly IClientReportService _reportService;
        private readonly ILogger _logger;

        public MoneyReportTotals(IClientReportService reportService, ILogger logger) {
            _reportService = reportService;
            _logger = logger;
        }
        
        public async Task<IViewComponentResult> InvokeAsync() {
            try {
                return View(await _reportService.GetUserMoneyTotalsAsync(UserSession.UserId));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get profile data. {message}", ex.Message);
                return Content(string.Empty);
            }
        }
    }
}
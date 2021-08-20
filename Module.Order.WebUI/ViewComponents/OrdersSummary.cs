using System;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Order.Business.Interfaces;
using Serilog;

namespace Module.Order.WebUI.ViewComponents {
    public class OrdersSummary : ViewComponentBase {
        private readonly IClientReportService _reportService;
        private readonly ILogger _logger;

        public OrdersSummary(IClientReportService reportService, ILogger logger) {
            _reportService = reportService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            try {
                if (!User.Identity.IsAuthenticated || User.IsGuest()) {
                    return Content(string.Empty);
                }

                return View(await _reportService.GetUserOrdersTotalsAsync(UserSession.UserId));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get profile data. {message}", ex.Message);
                return Content(string.Empty);
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Partners.Business.Interfaces;
using Serilog;

namespace Module.Partners.WebUI.Partners.ViewComponents {
    public class SidebarViewComponent : ViewComponentBase {
        private readonly IPartnerStorehouseReportService _reportService;
        private readonly ILogger _logger;

        public SidebarViewComponent(IPartnerStorehouseReportService reportService, ILogger logger) {
            _reportService = reportService;
            _logger = logger;
        }
        
        public async Task<IViewComponentResult> InvokeAsync() {
            try {
                return View(await _reportService.GetTotalsAsync(UserSession.UserId));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner sidebar data. UserId: {userId}. {message}", UserSession.UserId, ex.Message);
                return Content(string.Empty);
            }
        }
    }
}
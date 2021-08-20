using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Models;
using Module.Partners.Business.Interfaces;
using Serilog;

namespace Module.Partners.Admin.Admin.ViewComponents {
    public class NewPartnershipRequestsViewComponent : ViewComponentBase {
        private readonly IPartnerService _partnerService;
        private readonly ILogger _logger;

        public NewPartnershipRequestsViewComponent(IPartnerService partnerService, ILogger logger) {
            _partnerService = partnerService;
            _logger = logger;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(ComponentOutValue<int> itemsCount) {
            try {
                var result = await _partnerService.GetNewRequests();
                itemsCount.Value = result.Count;
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partnership requests. {message}", ex.Message);
            }

            return Content(string.Empty);
        }
    }
}
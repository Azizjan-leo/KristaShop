using System;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Module.Common.WebUI.Base;
using Module.Partners.Business.Interfaces;
using Serilog;

namespace Module.Partners.WebUI.Controllers {
    [FeatureGate(GlobalConstant.FeatureFlags.FeaturePartnersPromo)]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class PartnershipController : AppControllerBase {
        private readonly ILogger _logger;

        public PartnershipController(ILogger logger) {
            _logger = logger;
        }
        
        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply([FromServices]IPartnerService partnerService) {
            try {
                return Json(await partnerService.ApplyAsync(UserSession.UserId));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to accept partnership application");
                return StatusCode(500);
            }
        }
    }
}
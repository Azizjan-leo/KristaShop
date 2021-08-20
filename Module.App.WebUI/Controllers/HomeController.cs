using System;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Infrastructure;
using Module.Common.WebUI.Models;
using Serilog;
using SmartBreadcrumbs.Attributes;

namespace Module.App.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    [DefaultBreadcrumb("Главная")]
    public class HomeController : AppControllerBase {
        private readonly ILogger _logger;

        public HomeController(ILogger logger) {
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> RedirectToCatalog(string guestAccessCode) {
            try {
                if (TempData["RedirectToCatalog"] != null) {
                    TempData["RedirectToCatalog"] = null;
                    
                    return View((object)"/Catalog/");
                }
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to redirect to catalog with the guest access code: {code}. {message}", guestAccessCode, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string id) {
            if (!string.IsNullOrEmpty(id) && id.ToLower() == "showloginform") {
                ViewBag.ShowLoginForm = true;
            } else {
                ViewBag.ShowLoginForm = false;
            }

            SetMetaInfo(new MetaViewModel { Description = "Платья оптом Krista Unique", Keywords = "krista unique, платья оптом Бишкек, платья оптом"});
            return View();
        }
    }
}
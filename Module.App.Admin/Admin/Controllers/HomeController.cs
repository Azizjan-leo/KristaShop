using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Admin.Admin.Filters;
using Module.Common.Admin.Admin.Models;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.App.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class HomeController : AppControllerBase {
        private readonly ILogger _logger;

        public HomeController(ILogger logger) {
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
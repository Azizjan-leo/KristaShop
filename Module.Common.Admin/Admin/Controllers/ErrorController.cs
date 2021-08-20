using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Module.Common.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    public class ErrorController : Controller {
        public IActionResult Error403() {
            return View();
        }

        public IActionResult Error404() {
            return View();
        }
    }
}
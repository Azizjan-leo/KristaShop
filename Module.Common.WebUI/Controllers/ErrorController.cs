using System.Net;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Infrastructure;

namespace Module.Common.WebUI.Controllers {
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class ErrorController : Controller {
        [Route("error/{statusCode:int}")]
        public IActionResult Index(int statusCode) {
            HttpContext.Response.StatusCode = statusCode;
            
            (string Image, string Message) data = statusCode switch {
                403 => ("commonError.svg", "В доступе отказано"),
                404 => ("common-404.svg", "Страница не найдена"),
                _ =>   ("commonError.svg", "Произошла ошибка")
            };
            ViewBag.Data = data;
            return View();
        }

        public IActionResult Common403() {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View();
        }

        public IActionResult Common404(int statusCode) {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        public IActionResult Common500() {
            HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            return View();
        }

        public IActionResult Model404() {
            HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return View();
        }

        [Permission]
        [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme + "," + GlobalConstant.Session.FrontendGuestScheme)]
        public IActionResult CartEmpty() {
            return View();
        }
    }
}
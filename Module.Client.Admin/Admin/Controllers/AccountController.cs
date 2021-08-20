using System;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Client.Business.Interfaces;
using Module.Client.WebUI.Models;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.Client.Admin.Admin.Controllers {
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Route("[area]/[controller]/[action]")]
    [Route("[area]/identity/[action]")] // support old route
    public class AccountController : AppControllerBase {
        private readonly ILogger _logger;

        public AccountController(ILogger logger) {
            _logger = logger;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() {
            if (User.Identity.IsAuthenticated) {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, [FromServices] ISignInService signInService) {
            try {
                var result = await signInService.SignIn(model.UserName, model.Password);
                SetNotification(result);
                if (!result.IsSuccess) {
                    HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return View();
                }

                return RedirectToAction("Index", "Home");
            } catch (Exception ex) {
                _logger.Error(ex, "Login failed for user {login}. Return url: {url}", model.UserName, model.ReturnUrl);
                SetNotification(OperationResult.Failure("Во время авторизации произошла ошибка"));
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Logout([FromServices] ISignInService signInService) {
            await signInService.SignOut();
            return RedirectToAction(nameof(Login));
        }
    }
}
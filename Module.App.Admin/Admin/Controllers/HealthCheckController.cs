using System;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Module.Common.Admin.Admin.Filters;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.App.Admin.Admin.Controllers {
    [Area("Admin")]
    [PermissionFilter(ForRootOnly = true)]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class HealthCheckController : AppControllerBase {
        private readonly ILogger _logger;

        public HealthCheckController(ILogger logger) {
            _logger = logger;
        }
        
        public IActionResult Index() {

            return View();
        }
        
        public IActionResult EmailCheck(string email, [FromServices] IEmailService emailService) {
            try {
                emailService.SendEmailAsync(new EmailMessage(email, "krista.fashion email notifications health check",
                    "If you received this email then email notifications for krista.fashion are working"), "Test");
                SetNotification(OperationResult.Success("Email sent successfully"));
            } catch (Exception ex) {
                _logger.Error($"Email check failed for email {email}");
                SetNotification(OperationResult.Failure(ex.Message));
            }
            return RedirectToAction("Index");
        }

        public IActionResult DropCache([FromServices] IMemoryCache memoryCache) {
            try {
                ((MemoryCache)memoryCache).Compact(1.0);
                SetNotification(OperationResult.Success("Кэш успешно сброшен"));
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                _logger.Error(ex, "Faild to drop cache. {message}", ex.Message);
                return BadRequest();
            }
        }
    }
}
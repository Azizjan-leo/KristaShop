using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.FeatureManagement.Mvc;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Infrastructure;
using Module.Partners.Business.Interfaces;
using Serilog;
using SmartBreadcrumbs.Nodes;

namespace Module.Partners.WebUI.Partners.Controllers {
    [FeatureGate(GlobalConstant.FeatureFlags.FeaturePartners)]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    [Permission(ForPartnersOnly = true)]
    [Area(GlobalConstant.PartnersArea)]
    public class PaymentsController : AppControllerBase {
        private readonly IPartnerDocumentsService _documentsService;
        private readonly ILogger _logger;

        public PaymentsController(IPartnerDocumentsService documentsService, ILogger logger) {
            _documentsService = documentsService;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            try {
                var result = await _documentsService.GetNotPaidPaymentDocumentsAsync(UserSession.UserId);
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин"));

                ViewBag.Documents = new SelectList(result.SelectMany(x => x.Items)
                    .GroupBy(x => x.FromDocumentName)
                    .Select(x => x.Key));

                ViewBag.Sizes = new SelectList(result.SelectMany(x => x.Items)
                    .GroupBy(x => x.Size.Value)
                    .Select(x => x.Key)
                    .OrderBy(x => x, new SizeStringComparer()));
                
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner reports index. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        public async Task<IActionResult> History() {
            try {
                var result = await _documentsService.GetPaidPaymentDocumentsAsync(UserSession.UserId);
                
                ViewBag.Documents = new SelectList(result.SelectMany(x => x.Items)
                    .GroupBy(x => x.FromDocumentName)
                    .Select(x => x.Key));

                ViewBag.Sizes = new SelectList(result.SelectMany(x => x.Items)
                    .GroupBy(x => x.Size.Value)
                    .Select(x => x.Key)
                    .OrderBy(x => x, new SizeStringComparer()));
                
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин"));
                return View(nameof(Index), result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load active payments. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.FeatureManagement.Mvc;
using Module.Common.Business.Interfaces;
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
    public class ReportsController : AppControllerBase {
        private readonly IPartnerStorehouseReportService _storehouseReportService;
        private readonly ILookupsService _lookupsService;
        private readonly ILogger _logger;
        
        public ReportsController(IPartnerStorehouseReportService storehouseReportService, ILookupsService lookupsService, ILogger logger) {
            _storehouseReportService = storehouseReportService;
            _lookupsService = lookupsService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> Movements() {
            return await Movements(new ModelsFilter());
        }
        
        [HttpPost]
        public async Task<IActionResult> Movements(ModelsFilter filter) {
            try {
                var result = await _storehouseReportService.GetModelsMovementAsync(UserSession.UserId, filter);
                ViewBag.Sizes = new SelectList(await _lookupsService.GetSizesListAsync());
                ViewBag.IsDetailsView = false;
                
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View((result, filter));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner reports index. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }
        
        [Route("[area]/[controller]/[action]/{modelId:int}")]
        public async Task<IActionResult> ModelMovement(int modelId, string fromDate, string toDate) {
            try {
                var dateFrom = DateTime.SpecifyKind(DateTimeExtension.TryParseBasicString(fromDate), DateTimeKind.Utc);
                var dateTo = DateTime.SpecifyKind(DateTimeExtension.TryParseBasicString(toDate).AddDays(1).AddSeconds(-1), DateTimeKind.Utc);
                var result = await _storehouseReportService.GetModelMovementAsync(UserSession.UserId, modelId, dateFrom, dateTo);
        
                ViewBag.Sizes = new SelectList(result.SelectMany(x => x.Items.Select(c => c.Size.Value))
                    .Concat(result.Select(c => c.ModelInfo.Size.Value))
                    .Distinct()
                    .OrderBy(c => c, new SizeStringComparer())
                    .ToList());
                
                ViewBag.IsDetailsView = true;
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View(nameof(Movements), (result, new ModelsFilter { DateFrom = dateFrom, DateTo = dateTo}));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner reports index. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }
    }
}
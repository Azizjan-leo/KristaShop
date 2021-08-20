using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class DocumentsController : AppControllerBase {
        private readonly IPartnerDocumentsService _documentsService;
        private readonly ILookupsService _lookupsService;
        private readonly ILogger _logger;

        public DocumentsController(IPartnerDocumentsService documentsService, ILookupsService lookupsService, ILogger logger) {
            _documentsService = documentsService;
            _lookupsService = lookupsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            return await Index(new DocumentsFilter());
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(DocumentsFilter filter) {
            try {
                var result = await _documentsService.GetStorehouseDocumentsAsync(UserSession.UserId, filter);

                filter.DocumentTypesSelect = new SelectList(_documentsService.GetStorehouseDocumentsLookup(), "Key", "Value");
                filter.SizesSelect = new SelectList(await _lookupsService.GetSizesListAsync());

                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View((result, filter));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner reports index. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> IncomeDocuments() {
            return await IncomeDocuments(new DocumentsFilter());
        }
        
        [HttpPost]
        public async Task<IActionResult> IncomeDocuments(DocumentsFilter filter) {
            try {
                filter.DocumentTypesSelect = new SelectList(_documentsService.GetStorehouseDocumentsLookup(), "Key", "Value");
                filter.SizesSelect = new SelectList(await _lookupsService.GetSizesListAsync());
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View(nameof(Index), (await _documentsService.GetIncomeDocumentsAsync(UserSession.UserId, filter), filter));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner reports index. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> SellingDocuments() {
            return await SellingDocuments(new DocumentsFilter());
        }
        
        [HttpPost]
        public async Task<IActionResult> SellingDocuments(DocumentsFilter filter) {
            try {
                filter.DocumentTypesSelect = new SelectList(_documentsService.GetStorehouseDocumentsLookup(), "Key", "Value");
                filter.SizesSelect = new SelectList(await _lookupsService.GetSizesListAsync());
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View(nameof(Index), (await _documentsService.GetSellingDocumentsAsync(UserSession.UserId, filter), filter));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner reports index. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RevisionDocuments() {
            return await RevisionDocuments(new DocumentsFilter());
        }
        
        public async Task<IActionResult> RevisionDocuments(DocumentsFilter filter) {
            try {
                filter.DocumentTypesSelect = new SelectList(_documentsService.GetStorehouseDocumentsLookup(), "Key", "Value");
                filter.SizesSelect = new SelectList(await _lookupsService.GetSizesListAsync());
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View(nameof(Index), (await _documentsService.GetRevisionDocumentsAsync(UserSession.UserId, filter), filter));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner reports index. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        [Route("[area]/[controller]/[action]/{number:long}")]
        public async Task<IActionResult> Document(ulong number, bool grouped) {
            try {
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                if (!grouped) {
                    var model = await _documentsService.GetDocumentAsync(number, UserSession.UserId);
                    return View(model);
                } else {
                    var model = await _documentsService.GetDocumentWithGroupedItemsAsync(number, UserSession.UserId);
                    ViewBag.Sizes = new SelectList(model.Items
                        .SelectMany(x => x.SizesInfo.TotalAmountBySize.Where(c => c.Value > 0).Select(c => c.Key))
                        .Concat(model.Items.Select(x => x.ModelInfo.Size.Line))
                        .Distinct()
                        .OrderBy(x => x, new SizeStringComparer()));
                    
                    return View("DocumentGrouped", model);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get document. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
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
    [FeatureGate(GlobalConstant.FeatureFlags.FeatureAdvancedFunctionality)]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    [Permission(ForPartnersOnly = true)]
    [Area(GlobalConstant.PartnersArea)]
    public class SellingRequestsController : AppControllerBase {
        private readonly IPartnerStorehouseService _partnerStorehouseService;
        private readonly ISellingRequestsService _sellingRequestsService;
        private readonly ILookupsService _lookupsService;
        private readonly ILogger _logger;

        public SellingRequestsController(IPartnerStorehouseService partnerStorehouseService,
            ISellingRequestsService sellingRequestsService, ILookupsService lookupsService, ILogger logger) {
            _partnerStorehouseService = partnerStorehouseService;
            _sellingRequestsService = sellingRequestsService;
            _lookupsService = lookupsService;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            try {
                var requests = await _sellingRequestsService.GetRequestsAsync(UserSession.UserId);
                var storehouseItems = await _partnerStorehouseService.GetStorehouseItemsGroupedAsync(UserSession.UserId);

                ViewBag.Sizes = new SelectList(await _lookupsService.GetSizesListAsync());

                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View((requests, storehouseItems));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner storehouse view. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        public async Task<IActionResult> Create([FromBody] List<BarcodeAmountDTO> items) {
            try {
                await _sellingRequestsService.UpdateSellingRequestAsync(UserSession.UserId, items);
                return Ok(items);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create selling request. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> Update([FromBody] List<BarcodeAmountDTO> items) {
            try {
                await _sellingRequestsService.UpdateSellingRequestAsync(UserSession.UserId, items);
                return Ok();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to edit selling request. {message}", ex.Message);
                return BadRequest();
            }
        }
    }
}
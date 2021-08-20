using System;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.Common.Admin.Admin.Filters;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Order.Business.Interfaces;
using Serilog;

namespace Module.Cart.Admin.Admin.Controllers {
    [Area("Admin")]
    [PermissionFilter]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class CartController : AppControllerBase {
        private readonly ICartService _cartService;
        private readonly ICityService _cityService;
        private readonly ILookupsService _lookupsService;
        private readonly ILogger _logger;

        public CartController(ICartService cartService,
            ICityService cityService, ILookupsService lookupsService,
            ILogger logger) {
            _cartService = cartService;
            _cityService = cityService;
            _lookupsService = lookupsService;
            _logger = logger;
        }

        public async Task<IActionResult> CartsReport(ReportsFilter filter) {
            try {
                var totals = await _cartService.GetCartsTotalsAsync(filter);
                filter.Articuls = new SelectList(await _cartService.GetArticulsAsync());
                filter.Colors = new SelectList(await _lookupsService.GetColorsLookupListAsync(), "Key", "Value");
                filter.Cities = new SelectList(await _cityService.GetCitiesLookupListAsync(), "Key", "Value");
                filter.Users = new SelectList(await _cartService.GetUsersAsync(), "Key", "Value");
                filter.Managers = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();
                filter.Catalogs = LookUpItem.FromEnum<CatalogType>().AsSelectList();

                var result = await _cartService.GetCartsItemsGroupedAsync(filter);

                return View((filter, totals, result));
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to get load carts report. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> UserCartsReport(ReportsFilter filter) {
            try {
                var totals = await _cartService.GetCartsTotalsAsync(filter);
                filter.Articuls = new SelectList(await _cartService.GetArticulsAsync());
                filter.Colors = new SelectList(await _lookupsService.GetColorsLookupListAsync(), "Key", "Value");
                filter.Cities = new SelectList(await _cityService.GetCitiesLookupListAsync(), "Key", "Value");
                filter.Users = new SelectList(await _cartService.GetUsersAsync(), "Key", "Value");
                filter.Managers = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();
                filter.Catalogs = LookUpItem.FromEnum<CatalogType>().AsSelectList();
                return View((filter, totals));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get cart totals for the report. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> LoadUserCartsReport(ReportsFilter filter) {
            try {
                var result = await _cartService.GetCartTotalsGroupedByUsersAsync(filter);
                return Ok(result);
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to get carts report. {message}", ex.Message);
                return BadRequest();
            }
        }
    }
}
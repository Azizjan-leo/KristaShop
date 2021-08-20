using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Session;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using Module.Catalogs.Business.Interfaces;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.Catalogs.WebUI.ViewComponents {
    public class Recommended : ViewComponentBase {
        private readonly ICatalogItemsService _catalogItemsService;
        private readonly ILogger _logger;

        public Recommended(ICatalogItemsService catalogItemsService, ILogger logger) {
            _catalogItemsService = catalogItemsService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<int> categoryIds) {
            try {
                var filter = new ProductsFilterExtended {
                    CatalogId = CatalogType.All, OrderDirection = CatalogOrderDir.Asc, OrderType = CatalogOrderType.OrderRandom,
                    HideEmptySlots = false, Articul = "", MinPrice = -1D, MaxPrice = -1D,
                    ColorIds = null, CategoriesIds = categoryIds, Sizes = null, SizeLines = null,
                    IncludeDescription = true, IncludeCategoriesMap = false
                };
                
                var result = await _catalogItemsService.GetFullModelsAllListAsync(filter, UserSession ?? new UnauthorizedSession(), Page.Create(0, 4));
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get recommended models. {message}", ex.Message);
                return Content(string.Empty);
            }
        }
    }
}

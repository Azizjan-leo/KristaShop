using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Catalogs.WebUI.Models;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.Catalogs.WebUI.ViewComponents {
    public class Catalog : ViewComponentBase {
        private readonly ICatalogItemsService _catalogItemsService;
        private readonly ICatalogService _catalogService;
        private readonly ICatalogAccessService _catalogAccessService;
        private readonly IFeatureManager _featureManager;
        private readonly ILogger _logger;

        public Catalog(ICatalogItemsService catalogItemsService, ICatalogService catalogService,
            ICatalogAccessService catalogAccessService, IFeatureManager featureManager, ILogger logger) {
            _catalogItemsService = catalogItemsService;
            _catalogService = catalogService;
            _catalogAccessService = catalogAccessService;
            _featureManager = featureManager;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            try {
                CatalogDTO catalog;

                if (UserSession != null && User.Identity.IsAuthenticated) {
                    var catalogIds = (await _catalogAccessService.GetAvailableUserCatalogsAsync(UserSession))
                        .Select(x => x.Id.ToProductCatalog1CId()).ToList();

                    if (!await _featureManager.IsEnabledAsync(GlobalConstant.FeatureFlags.FeatureAdvancedFunctionality)) {
                        catalogIds.RemoveAll(x => x == CatalogType.RfInStockLines || x == CatalogType.RfInStockParts);
                    }
                    
                    if (!catalogIds.Any()) catalogIds.Add(CatalogType.Open);

                    var catalogs = await _catalogService.GetCatalogsAsync(catalogIds);
                    catalog = catalogs.FirstOrDefault();
                    if (catalog == null) catalog = await _catalogService.GetCatalogDetailsAsync(CatalogType.Open);
                } else {
                    catalog = await _catalogService.GetCatalogDetailsAsync(CatalogType.Open);
                }

                var products =
                    await _catalogItemsService.GetCatalogTopModelItemsAsync(catalog.Id);
                var result = new CatalogWithItemsViewModel {Catalog = catalog, Items = products};
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load catalog ViewComponent. {message}", ex.Message);
            }

            return Content(string.Empty);
        }
    }
}
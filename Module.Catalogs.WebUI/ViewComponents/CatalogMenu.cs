using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Module.Catalogs.Business.Interfaces;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.Catalogs.WebUI.ViewComponents {
    public class CatalogMenu : ViewComponentBase {
        private readonly ICatalogService _catalogService;
        private readonly ICatalogAccessService _catalogAccessService;
        private readonly ILogger _logger;
        private readonly IFeatureManager _featureManager;

        public CatalogMenu(ICatalogService catalogService, ICatalogAccessService catalogAccessService,
            ILogger logger, IFeatureManager featureManager) {
            _catalogService = catalogService;
            _catalogAccessService = catalogAccessService;
            _logger = logger;
            _featureManager = featureManager;
        }

        public async Task<IViewComponentResult> InvokeAsync() {

            if(UserSession is null && (HttpContext.User.HasClaim("Type", GlobalConstant.Session.FrontendScheme) || HttpContext.User.HasClaim(GlobalConstant.Session.FrontendUserClaimName, GlobalConstant.Session.FrontendScheme))) {
                var frontendSchemeClaimName = HttpContext.User.FindFirstValue(GlobalConstant.Session.FrontendScheme);
                var frontendUserClaimName = HttpContext.User.FindFirstValue(GlobalConstant.Session.FrontendUserClaimName);
                _logger.Error("Couldn't get Session.FrontendScheme or Session.FrontendUserClaimName {FrontendScheme}, {FrontendUserClaimName}", frontendSchemeClaimName, frontendUserClaimName);
            }
            else if (UserSession is null && (HttpContext.User.HasClaim("Type", GlobalConstant.Session.FrontendGuestScheme) || HttpContext.User.HasClaim(GlobalConstant.Session.FrontendGuestClaimName, GlobalConstant.Session.FrontendGuestScheme))) {
                var frontendGuestScheme = HttpContext.User.FindFirstValue(GlobalConstant.Session.FrontendGuestScheme);
                var frontendGuestClaimName = HttpContext.User.FindFirstValue(GlobalConstant.Session.FrontendGuestClaimName);
                _logger.Error("Couldn't get Session.FrontendGuestScheme or Session.FrontendGuestClaimName {FrontendGuestScheme}, {FrontendGuestClaimName}", frontendGuestScheme, frontendGuestClaimName);
            }

            var catalogIds = (await _catalogAccessService.GetAvailableUserCatalogsAsync(UserSession)).Select(x => x.Id.ToProductCatalog1CId()).ToList();

            if (!await _featureManager.IsEnabledAsync(GlobalConstant.FeatureFlags.FeatureAdvancedFunctionality)) {
                catalogIds.RemoveAll(x => x is CatalogType.RfInStockLines or CatalogType.RfInStockParts);
            }
            
            var catalogs = await _catalogService.GetCatalogsAsync(catalogIds);
            return View(catalogs);
        }
    }
}
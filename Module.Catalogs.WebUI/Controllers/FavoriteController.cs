using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Catalogs.Business.Interfaces;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.Catalogs.WebUI.Controllers
{
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class FavoriteController : AppControllerBase
    {
        private readonly IFavorite1CService _favoriteService;
        private readonly ICatalogService _catalogService;
        private readonly ILogger _logger;

        public FavoriteController(IFavorite1CService favoriteService, ILogger logger, ICatalogService catalogService)
        {
            _favoriteService = favoriteService;
            _catalogService = catalogService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try {
                var models = await _favoriteService.getUserFavoritesAsync(UserSession.UserId);

                List<CatalogType> catalogIds = new List<CatalogType>();
                for (int i = 0; i <= (int)CatalogType.Preorder; i++) {
                    catalogIds.Add(i.ToProductCatalog1CId());
                }

                var catalogsList = await _catalogService.GetCatalogsAsync(catalogIds);

                var catalogsDict = new Dictionary<int, string>();
                for (int i = 0; i <= (int)CatalogType.Preorder; i++) {
                    var catBy1CId = catalogsList.FirstOrDefault(x => x.Id == i.ToProductCatalog1CId());
                    catalogsDict.Add(i, catBy1CId != null ? catBy1CId.Name : i.ToProductCatalog1CId().AsString());
                }

                ViewBag.CatalogNames = catalogsDict;

                return View(models);
            } catch (Exception ex) {
                _logger.Fatal(ex, "Failed to display catalog models in favorites. {message}. User: {@user}", ex.Message, UserSession);
                return RedirectToAction("Common500", "Error");
            }
        }

        public async Task<IActionResult> AddOrDeleteFavorite(string articul, int catalogId) {
            if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) {
                return BadRequest(OperationResult.Failure("В доступе отказано."));
            }

            try {
                var result = await _favoriteService.AddOrDeleteFavoriteAsync(UserSession.UserId, articul, catalogId);
                if (result.IsSuccess) {
                    return Ok(result);
                }

                return BadRequest(result);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return BadRequest(OperationResult.Failure());
            }
        }

        public async Task<IActionResult> DeleteFavorite(string articul, int catalogId) {
            if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) {
                return BadRequest(OperationResult.Failure("В доступе отказано."));
            }

            try {
                var result = await _favoriteService.DeleteFavoriteAsync(UserSession.UserId, articul, catalogId);
                if (result.IsSuccess) {
                    return Ok(result);
                }

                return BadRequest(result);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return BadRequest(OperationResult.Failure());
            }
        }
    }
}
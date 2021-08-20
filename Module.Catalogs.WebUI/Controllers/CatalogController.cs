using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Session;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.Catalogs.WebUI.Models;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Infrastructure;
using Module.Common.WebUI.Models;
using Serilog;
using Microsoft.FeatureManagement;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.AllFrontSchemes)]
    public class CatalogController : AppControllerBase {
        private readonly ICatalogService _catalogService;
        private readonly ICatalogAccessService _catalogAccessService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogItemsService _catalogItemsService;
        private readonly IFavorite1CService _favorite1CService;
        private readonly ILogger _logger;
        private readonly ISettingsManager _settingsManager;
        private readonly IFeatureManager _featureManager;

        public CatalogController(ICategoryService categoryService, ICatalogItemsService catalogItemsService,
            ICatalogService catalogService, ICatalogAccessService catalogAccessService,
            IMapper mapper, ILogger logger, IFavorite1CService favorite1CService, ISettingsManager settingsManager,
            IFeatureManager featureManager) {
            _categoryService = categoryService;
            _catalogItemsService = catalogItemsService;
            _catalogService = catalogService;
            _catalogAccessService = catalogAccessService;
            _favorite1CService = favorite1CService;
            _mapper = mapper;
            _logger = logger;
            _settingsManager = settingsManager;
            _featureManager = featureManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(NomFilterDTO filter, int page = 1, bool? showRegistrationForm = null) {
            try {
                ViewBag.ShowRegistartionFrom = showRegistrationForm != null && UserSession is UnauthorizedSession;
                
                CatalogDTO currentCatalog = null;
                if (string.IsNullOrEmpty(filter.CatalogUri) && filter.CatalogId == CatalogType.All) {
                    currentCatalog = await _catalogAccessService.GetFirstAvailableCatalogAsync(UserSession);
                } else {
                    currentCatalog = await _catalogAccessService.GetCatalogByIdOrUriIfAvailableAsync(UserSession,
                        filter.CatalogUri, filter.CatalogId);
                }

                if (currentCatalog.Id == CatalogType.Preorder &&
                    !_settingsManager.Settings.PreorderParts.IsEmpty()) {
                    return await _groupedCatalogItemsIndex(currentCatalog, filter, page);
                }

                var result = await _getCatalogItemsAsync(currentCatalog, filter,
                    _settingsManager.Settings.FrontCatalogOnPageCount, page);
                return View(result);
            } catch(EntityNotFoundException ex) {
                if (UserSession is UnauthorizedSession)
                    return RedirectToAction("Index", "Home", new { id = "showloginform" });
                SetNotification(OperationResult.Failure("Вам не доступен данный каталог"));
                return RedirectToAction("Index", "Home");
            }
            catch(CatalogAccessException ex) {
                SetNotification(OperationResult.Failure("Вам не доступен данный каталог"));
                return RedirectToAction("Index", "Home");
            } catch (Exception ex) {
                _logger.Fatal(ex, "Failed to display catalog models. {message}. Filter: {@filter}; Page: {page}; User: {@user}",
                    ex.Message, filter, page, UserSession);
                return RedirectToAction("Common500", "Error");
            }
        }

        private async Task<NomFilterViewModel> _getCatalogItemsAsync(CatalogDTO catalog, NomFilterDTO filter, int modelsPerPage, int page = 1) {
            filter.SetDataFromCatalog(catalog);
            var productsFilter = _mapper.Map<ProductsFilterExtended>(filter);
            productsFilter.HideEmptySlots = !catalog.IsOpen;

            var currentPage = Page.Create(page - 1, modelsPerPage);
            var models = await _catalogItemsService.GetFullModelsAllListAsync(productsFilter, UserSession, currentPage);
            var totalItemsCount = await _catalogItemsService.GetModelsCountAsync(productsFilter, UserSession);

            var userCatalogs = await _catalogAccessService.GetAvailableUserCatalogsFullAsync(UserSession);
            
            if (!await _featureManager.IsEnabledAsync(GlobalConstant.FeatureFlags.FeatureAdvancedFunctionality)) {
                userCatalogs.RemoveAll(x => x.Id is CatalogType.RfInStockLines or CatalogType.RfInStockParts);
            }

            return new NomFilterViewModel {
                Filter = filter,
                Catalog = catalog,
                Catalogs = userCatalogs,
                Models = models.ToPagerList(currentPage, totalItemsCount),
                Page = page
            };
        }

        private async Task<IActionResult> _groupedCatalogItemsIndex(CatalogDTO currentCatalog, NomFilterDTO filter, int page = 1) {
            var result = await _getCatalogItemsAsync(currentCatalog, filter, 10000, page);
            ViewData["PreorderParts"] = _settingsManager.Settings.PreorderParts.Parts;
            return View("IndexSections", result);
        }

        public async Task<IActionResult> Search(NomFilterDTO filter) {
            try {
                var searchLookups = new SearchLookupsViewModel();
                var (colors, sizes, sizeLines) = await _catalogItemsService.GetFilterLookupListsAsync(CatalogType.All, true);
                searchLookups.Colors = colors;
                searchLookups.Sizes = sizes;
                searchLookups.SizeLines = sizeLines;
                searchLookups.Categories = await _categoryService.GetCategoriesAsync();
                var catalogs1C = (await _catalogAccessService.GetAvailableUserCatalogsAsync(UserSession ?? new UnauthorizedSession())).Select(x => x.Id.ToProductCatalog1CId()).ToList();

                var catalogs = await _catalogService.GetCatalogsAsync(catalogs1C);
                if (!await _featureManager.IsEnabledAsync(GlobalConstant.FeatureFlags.FeatureAdvancedFunctionality)) {
                    catalogs.RemoveAll(x => x.Id is CatalogType.RfInStockLines or CatalogType.RfInStockParts);
                }

                var catalogsSelectList = new SelectList(catalogs, nameof(CatalogDTO.Id), nameof(CatalogDTO.Name));
               
                return View(new SearchPanelViewModel { SearchLookups = searchLookups, NomFilter = filter, Catalogs = catalogsSelectList });
            } catch (Exception ex) {
                _logger.Fatal(ex, "Failed to get search form. {message}. User: {@user}", ex.Message, UserSession);
                return RedirectToAction("Common500", "Error");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Products(NomFilterDTO filter, [FromServices] ISettingsManager settingsManager) {
            try { 
                if (filter.CatalogId == CatalogType.All) {
                    filter.CatalogId = User.Identity is {IsAuthenticated: false} ? CatalogType.Open : CatalogType.All;
                }
                
                var productsFilter = _mapper.Map<ProductsFilterExtended>(filter);
                productsFilter.HideEmptySlots = filter.CatalogId != (int) CatalogType.Open;

                var models = await _catalogItemsService.GetFullModelsGroupedByCatalogsAsync(productsFilter, UserSession, 0, 0);
                var catalogs = (await _catalogService.GetCatalogsAsync(models.Keys.ToList())).ToDictionary(
                        k => k.Id, v => v);

                var result = new SearchAllViewModel {
                    Filter = filter,
                    SearchProducts = models,
                    Catalogs = catalogs,
                    SearchTitle = "Поиск по всем каталогам",
                    DescriptionSettingKey = settingsManager.Settings.OpenCatalogSearchDescription
                };

                if (filter.Categories is {Count: 1}) {
                    var category = await _categoryService.GetCategoryAsync(filter.Categories.First());
                    if (category != null) {
                        result.SearchTitle = category.Name;
                        result.HasDescription = true;
                    }
                }

                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Full search by filter failed, filter {@filter}. For user {@user}. {message}", filter, UserSession, ex.Message);
                SetNotification(OperationResult.Failure("Произошла ошибка во время поиска"));
                return RedirectToAction(nameof(Search), new {filter});
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Product(string articul, CatalogType catalog, int page = 1) {
            try {
                if (catalog == CatalogType.All) {
                    SetNotification(OperationResult.Failure("Каталог не выбран"));
                    return RedirectToAction(nameof(Index), new {catalogId = catalog});
                }

                var currentCatalog = await _catalogService.GetCatalogDetailsAsync(catalog);
                if (currentCatalog == null) {
                    return RedirectToAction("Model404", "Error");
                }

                if (!currentCatalog.IsOpen && UserSession is UnauthorizedSession) {
                    SetNotification(OperationResult.Failure("У вас закрыт доступ к каталогу."));
                    return RedirectToAction(nameof(Index), "Home");
                }
                
                var model = await _catalogItemsService.FindModelAsync(articul, catalog, UserSession ?? new UnauthorizedSession(), currentCatalog.IsOpen);
                if (model == null) {
                    SetNotification(OperationResult.Failure("Модель не найдена."));
                    return RedirectToAction(nameof(Index), new {catalogId = catalog, page});
                }

                ViewData["CatalogPage"] = page;
                var result = new ProductViewModel(model, currentCatalog, !User.Identity.IsAuthenticated || currentCatalog.IsOpen);
                result.ItemFull.Sizes = model.Sizes;

                SetMetaInfo(new MetaViewModel(model.Descriptor.MetaTitle, model.Descriptor.MetaDescription, model.Descriptor.MetaKeywords));

                if (UserSession is {UserId: > 0}) {
                    result.IsFavorite = await _favorite1CService.IsFavoriteAsync(articul, (int)catalog, UserSession.UserId);
                } else {
                    result.IsFavorite = false;
                }

                SetBreadcrumbs(ControllerName, "Index", result.Catalog.Name, new() {{"CatalogUri", result.Catalog.Uri}, {"Page", page}},
                    CreateBreadcrumb(ControllerName, ActionName, result.ItemFull.Descriptor.Articul));
                return View(result);
            } catch (Exception ex) {
                _logger.Fatal(ex, "Failed to load product page articul: {articul}, catalogId: {catalogId}. {message}", articul, catalog, ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        public async Task<IActionResult> ProductJson(string articul, CatalogType catalogId) {
            try {
                if (catalogId == CatalogType.All) {
                    return BadRequest(OperationResult.Failure("Каталог не выбран"));
                }

                var catalog = await _catalogService.GetCatalogDetailsAsync(catalogId);
                if (catalog == null) {
                    return BadRequest(OperationResult.Failure("Каталог не найден"));
                }

                if (!catalog.IsOpen && UserSession is UnauthorizedSession) {
                    return BadRequest(OperationResult.Failure("У вас закрыт доступ к каталогу."));
                }

                var model = await _catalogItemsService.FindModelAsync(articul, catalogId, UserSession ?? new UnauthorizedSession(), catalog.IsOpen);
                if (model == null) {
                    return BadRequest(OperationResult.Failure("Модель не найдена."));
                }

                var result = new ProductViewModel(_mapper.Map<CatalogItemGroupNew>(model), catalog, !User.Identity.IsAuthenticated || User.IsGuest() || catalog.IsOpen);
                result.ItemFull.Sizes = model.Sizes;
                return Ok(result);
            } catch (Exception ex) {
                _logger.Fatal(ex, "Failed to load product page articul: {articul}, catalogId: {catalogId}. {message}", articul, catalogId, ex.Message);
                return BadRequest(OperationResult.Failure("Не удалось получить данные о модели"));
            }
        }
    }
}
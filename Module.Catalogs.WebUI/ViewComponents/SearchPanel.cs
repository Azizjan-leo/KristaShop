using System;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Catalogs.WebUI.Models;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.Catalogs.WebUI.ViewComponents {
    public class SearchPanel : ViewComponentBase {
        private readonly ICatalogItemsService _catalogItemsService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;

        public SearchPanel(ICatalogItemsService catalogItemsService, ICategoryService categoryService, ILogger logger) {
            _catalogItemsService = catalogItemsService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(CatalogType catalogId, SearchingType searchingType, NomFilterDTO filter) {
            try {

                var searchLookups = new SearchLookupsViewModel();
                if (searchingType == SearchingType.Catalog) {
                    var (colors, sizes, sizeLines) = await _catalogItemsService.GetFilterLookupListsAsync(catalogId, catalogId.HasEmptySlots());
                    searchLookups.Colors = colors;
                    searchLookups.Sizes = sizes;
                    searchLookups.SizeLines = sizeLines;
                    searchLookups.Categories = await _categoryService.GetCategoriesAsync();
                } else if (searchingType == SearchingType.User) {
                    throw new NotImplementedException("Filter for SearchType.User not implemented");
                }

                return View(new SearchPanelViewModel {SearchLookups = searchLookups, NomFilter = filter});
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load search panel ViewComponent. {message}", ex.Message);
                return Content(string.Empty);
            }
        }
    }
}
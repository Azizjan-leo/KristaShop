using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Catalogs.Business.Interfaces;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.Catalogs.WebUI.ViewComponents {
    public class Category : ViewComponentBase {
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;

        public Category(ICategoryService categoryService, ILogger logger) {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = null) {
            try {
                var categories = await _categoryService.GetCategoriesAsync();
                if (!string.IsNullOrEmpty(viewName)) {
                    return View(viewName, categories);
                }

                return View(categories);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load categories ViewComponent. {message}", ex.Message);
            }

            return Content(string.Empty);
        }
    }
}
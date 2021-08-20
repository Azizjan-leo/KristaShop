using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Catalogs.Business.Interfaces;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Infrastructure;

namespace Module.Catalogs.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class CategoryController : Controller {
        private readonly ICategoryService _categoryService;
        private readonly ISettingsManager _settingsManager;

        public CategoryController(ICategoryService categoryService, ISettingsManager settingsManager) {
            _categoryService = categoryService;
            _settingsManager = settingsManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            ViewData["DescriptionSettingKey"] = _settingsManager.Settings.CategoriesDescription;

            var categories = await _categoryService.GetCategoriesAsync();
            return View(categories);
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class Instagram : ViewComponentBase {
        private readonly IBlogService _blogService;
        private readonly ISettingsManager _settingsManager;
        private readonly IDynamicPagesManager _dynamicPagesManager;
        private readonly ILogger _logger;

        public Instagram(IBlogService blogService, ISettingsManager settingsManager,
            IDynamicPagesManager dynamicPagesManager, ILogger logger) {
            _blogService = blogService;
            _settingsManager = settingsManager;
            _dynamicPagesManager = dynamicPagesManager;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            try {
                var blogs = await _blogService.GetTopBlogsAsync(4);
                ViewData["InstagramLink"] = _settingsManager.Settings.KristaInstagram;
                _dynamicPagesManager.TryGetValue("/Instagram/Index", out var page);
                ViewBag.InstagramDescription = page?.Body ?? "";
                return View(blogs);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load instagram ViewComponent. {message}", ex.Message);
            }

            return Content(string.Empty);
        }
    }
}
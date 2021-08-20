using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class Slider : ViewComponentBase {
        private readonly IBannerService _bannerService;
        private readonly ILogger _logger;

        public Slider(IBannerService bannerService, ILogger logger) {
            _bannerService = bannerService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            ViewBag.CatalogUri = string.Empty;

            try {
                return View(await _bannerService.GetBannersAsync(true));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load slider ViewComponent. {message}", ex.Message);
                return Content(string.Empty);
            }
        }
    }
}
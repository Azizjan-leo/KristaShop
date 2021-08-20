using System;
using Microsoft.AspNetCore.Mvc;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class About : ViewComponent {
        private readonly IDynamicPagesManager _dynamicPagesManager;
        private readonly ILogger _logger;

        public About(IDynamicPagesManager dynamicPagesManager, ILogger logger) {
            _dynamicPagesManager = dynamicPagesManager;
            _logger = logger;
        }
        
        public IViewComponentResult Invoke() {
            try {
                _dynamicPagesManager.TryGetValue("/Abus/Index", out var page);
                return View("Default", page?.Body ?? "");
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load about us section");
            }

            return Content(string.Empty);
        }
    }
}
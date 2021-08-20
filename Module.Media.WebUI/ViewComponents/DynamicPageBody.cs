using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class DynamicPageBody : ViewComponentBase {
        private readonly IDynamicPagesService _service;
        private readonly ISettingsManager _settingsManager;
        private readonly ILogger _logger;

        public DynamicPageBody(IDynamicPagesService service, ISettingsManager settingsManager, ILogger logger) {
            _service = service;
            _settingsManager = settingsManager;
            _logger = logger;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(string url) {
            try {
                var description = await _service.GetPageByUrlAsync(_settingsManager.Settings.OpenCatalogSearchDescription);
                return View("Default", description?.Body);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load dynamic pages menu");
            }
            return Content(string.Empty);
        }
    }
}
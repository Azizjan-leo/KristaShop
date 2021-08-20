using System;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class Youtube : ViewComponent {
        private readonly ISettingsManager _settingsManager;
        private readonly ILogger _logger;

        public Youtube(ISettingsManager settingsManager, ILogger logger) {
            _settingsManager = settingsManager;
            _logger = logger;
        }

        public IViewComponentResult Invoke() {
            try {
                ViewData["Youtube"] = _settingsManager.Settings.KristaYoutube;
                ViewData["YoutubeSubscribe"] = _settingsManager.Settings.KristaYoutubeSubscribe;
                return View();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load youtube ViewComponent. {message}", ex.Message);
            }

            return Content(string.Empty);
        }
    }
}
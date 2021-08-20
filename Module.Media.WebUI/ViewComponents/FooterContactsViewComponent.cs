using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class FooterContactsViewComponent : ViewComponentBase {
        private readonly IDynamicPagesManager _dynamicPagesManager;
        private readonly ISettingsManager _settingsManager;
        private readonly ILogger _logger;

        public FooterContactsViewComponent(IDynamicPagesManager dynamicPagesManager, ISettingsManager settingsManager,
            ILogger logger) {
            _dynamicPagesManager = dynamicPagesManager;
            _settingsManager = settingsManager;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            try {
                if (_dynamicPagesManager.TryGetValue(_settingsManager.Settings.FooterContacts, out var contactsContent)) {
                    return View("Default", contactsContent.Body);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load contact data for footer");
            }
            return Content(string.Empty);
        }
    }
}
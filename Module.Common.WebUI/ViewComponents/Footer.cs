using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Models;
using Serilog;

namespace Module.Common.WebUI.ViewComponents {
    public class Footer : ViewComponentBase {
        private readonly ISettingsManager _settingsManager;
        private readonly ILogger _logger;

        public Footer(ISettingsManager settingsManager, ILogger logger) {
            _settingsManager = settingsManager;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            try {
                var result = new FooterViewModel {
                    TermsOfUseUri = _settingsManager.Settings.TermsOfUse,
                    FacebookLink = _settingsManager.Settings.KristaFacebook,
                    VkLink = _settingsManager.Settings.KristaVk,
                    InstagramLink = _settingsManager.Settings.KristaInstagram,
                    YoutubeLink = _settingsManager.Settings.KristaYoutubeSubscribe,
                    PaymentDetails = _settingsManager.Settings.PaymentDetails,
                    DeliveryDetails = _settingsManager.Settings.DeliveryDetails
                };

                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load data for footer");
                return Content(string.Empty);
            }
        }
    }
}

using System.Threading.Tasks;
using KristaShop.Common.Models.Session;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Order.WebUI.Models;

namespace Module.Order.WebUI.ViewComponents {
    public class OrderForm : ViewComponentBase {
        private readonly ISettingsManager _settingsManager;

        public OrderForm(ISettingsManager settingsManager) {
            _settingsManager = settingsManager;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            if (UserSession is UnauthorizedSession) return Content(string.Empty);

            if (!UserSession.IsGuest) {
                ViewData["DeliveryDetailsUri"] = _settingsManager.Settings.DeliveryDetails;
                ViewData["PaymentDetailsUri"] = _settingsManager.Settings.PaymentDetails;
                ViewData["TermsOfUse"] = _settingsManager.Settings.TermsOfUse;

                return View(new OrderViewModel());
            } else {
                ViewData["DeliveryDetailsUri"] = _settingsManager.Settings.DeliveryDetails;
                ViewData["PaymentDetailsUri"] = _settingsManager.Settings.PaymentDetails;
                ViewData["TermsOfUse"] = _settingsManager.Settings.TermsOfUse;

                ViewData["DeliveryCatalogUri"] = null;
                return View("GuestOrder", new GuestOrderViewModel());
            }
        }
    }
}
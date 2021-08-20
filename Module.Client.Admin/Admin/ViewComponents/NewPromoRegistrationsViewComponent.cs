using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Client.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Models;

namespace Module.Client.Admin.Admin.ViewComponents {
    public class NewPromoRegistrationsViewComponent : ViewComponentBase {
        private readonly IRegistrationService _registrationService;

        public NewPromoRegistrationsViewComponent(IRegistrationService registrationService) {
            _registrationService = registrationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentOutValue<int> itemsCount, bool showTitle) {
            var registrations = await _registrationService.GetGuestsRequestsAsync();
            itemsCount.Value = registrations.Count;
            return View((registrations, showTitle));
        }
    }
}
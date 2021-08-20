using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Client.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Models;

namespace Module.Client.Admin.Admin.ViewComponents {
    public class NewRegistrationsViewComponent : ViewComponentBase {
        private readonly IRegistrationService _registrationService;

        public NewRegistrationsViewComponent(IRegistrationService registrationService) {
            _registrationService = registrationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentOutValue<int> itemsCount) {
            var registrations = await _registrationService.GetRequestsAsync();
            itemsCount.Value = registrations.Count;
            return View(registrations);
        }
    }
}
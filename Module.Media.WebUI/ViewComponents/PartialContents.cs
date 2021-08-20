using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;

namespace Module.Media.WebUI.ViewComponents {
    public class PartialContents : ViewComponentBase {
        private readonly IDynamicPagesService _dynamicPagesService;

        public PartialContents(IDynamicPagesService dynamicPagesService) {
            _dynamicPagesService = dynamicPagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string controllerName) {
            var contents =
                await _dynamicPagesService.GetPageByControllerAsync(controllerName, User.Identity.IsAuthenticated && !User.IsGuest());
            return View(contents);
        }
    }
}
using System;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class DynamicPagesMenu : ViewComponentBase {
        private readonly IDynamicPagesManager _manager;
        private readonly ILogger _logger;

        public DynamicPagesMenu(IDynamicPagesManager manager, ILogger logger) {
            _manager = manager;
            _logger = logger;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(string controllerName) {
            try {
                if(_manager.TryGetValuesByControllerForMenu(controllerName, !User.Identity.IsAuthenticated || User.IsGuest(), out var menuItems)) {
                    return View(menuItems);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load dynamic pages menu");
            }

            return Content(string.Empty);
        }
    }
}
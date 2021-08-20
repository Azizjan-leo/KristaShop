using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.App.Business.Interfaces;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.App.Admin.Admin.ViewComponents {
    public class NavbarBasicViewComponent : ViewComponentBase {
        private readonly IMenuService _menuService;
        private readonly IRoleAccessService _roleAccessService;
        private readonly ILogger _logger;

        public NavbarBasicViewComponent(IMenuService menuService, IRoleAccessService roleAccessService, ILogger logger) {
            _menuService = menuService;
            _roleAccessService = roleAccessService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(MenuType menuType = MenuType.Top, string controller = "") {
            try {
                if (string.IsNullOrEmpty(controller)) {
                    controller = HttpContext.GetRouteData().GetController();
                }

                var menuItems = await _menuService.GetMenuItemsWithSameParent(menuType, controller);
                if (UserSession.IsManager && !UserSession.IsRoot) {
                    var hasAccessTo = await _roleAccessService.HasAccessToRoutesAsync(UserSession.ManagerDetails.RoleId, menuItems.Select(x => new RouteValue(x.AreaName, x.ControllerName, x.ActionName)).ToList());
                    menuItems = menuItems.Where(x => hasAccessTo.Any(u => u.Area == x.AreaName && u.Controller == x.ControllerName && u.Action == x.ActionName)).ToList();
                }

                return View(menuItems.OrderBy(x => x.Order).ToList());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load admin navbar. {message}", ex.Message);
                throw;
            }
        }
    }
}

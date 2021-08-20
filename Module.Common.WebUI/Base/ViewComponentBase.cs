using System;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Models.Session;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Extensions;

namespace Module.Common.WebUI.Base {
    public abstract class ViewComponentBase : ViewComponent {
        private UserSession _userSession;
        protected UserSession UserSession => _userSession ??= HttpContext.GetUserSession();

        public async Task<bool> HasAccess(string controller, string action, string area = "Admin") {
            if (UserSession.IsRoot) {
                return true;
            }
            
            if (!UserSession.IsManager && HttpContext.GetRouteData().GetArea().Equals("Admin")) {
                throw new Exception("Access forbidden");
            }

            return await HttpContext.RequestServices.GetService<IPermissionManager>()!
                .HasAccessAsync(HttpContext, UserSession.ManagerDetails.RoleId, new RouteValue(area, controller, action));
        }
    }
}


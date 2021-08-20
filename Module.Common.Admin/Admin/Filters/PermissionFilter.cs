using System;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;

namespace Module.Common.Admin.Admin.Filters {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionFilter : Attribute, IAsyncAuthorizationFilter {
        public bool ForRootOnly { get; set; }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context) {
            var loginRoute = new RouteValueDictionary(new {area = context.RouteData.GetArea(), controller = "Identity", action = "Login"});
            var user = context.HttpContext.GetUserSession();
            if (user == null) {
                context.Result = new RedirectToRouteResult(loginRoute);
                return;
            }

            if (user.IsRoot) {
                return;
            }

            var notFoundRoute = new RouteValueDictionary(new {area = context.RouteData.GetArea(), controller = "Error", action = "Error404"});

            if (user.IsManager && !ForRootOnly) {
                var hasAccess = await context.HttpContext.RequestServices.GetService<IPermissionManager>()!
                    .HasAccessAsync(context.HttpContext, user.ManagerDetails.RoleId, context.RouteData.GetRouteValue());
                
                if (!hasAccess) {
                    context.Result = new ForbidResult();
                }
                return;
            }

            context.Result = new RedirectToRouteResult(notFoundRoute);
        }
    }
}
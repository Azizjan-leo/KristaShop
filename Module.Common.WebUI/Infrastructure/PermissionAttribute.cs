using System;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;

namespace Module.Common.WebUI.Infrastructure {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAsyncAuthorizationFilter {
        public bool ForManagerOnly { get; set; } = false;
        public bool ForPartnersOnly { get; set; } = false;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context) {
            var userSession = context.HttpContext.GetUserSession();

            if (userSession != null && 
                ((ForPartnersOnly && !userSession.IsPartner) || (ForManagerOnly && !userSession.IsManager))) {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Error", action = "Common404"}));
            }
        }
    }
}
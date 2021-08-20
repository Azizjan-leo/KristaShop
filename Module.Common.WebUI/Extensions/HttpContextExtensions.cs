using KristaShop.Common.Interfaces;
using KristaShop.Common.Models.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Module.Common.WebUI.Extensions {
    public static class HttpContextExtensions {
        public static UserSession GetUserSession(this HttpContext context) {
            var claimsManager = context.RequestServices.GetService<IClaimsManager>();
            return claimsManager!.Session;
        }
        
        public static string GetUserSessionClaim(this HttpContext context) {
            var claimsManager = context.RequestServices.GetService<IClaimsManager>();
            return claimsManager!.GetUserSessionClaim();
        }
    }
}
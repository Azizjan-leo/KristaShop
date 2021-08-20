using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Routing;

namespace KristaShop.Common.Extensions {
    public static class RouteValueDictionaryExtension {
        public static RouteValue GetRouteValue(this RouteData routeData) {
            return new(routeData.GetArea(), routeData.GetController(), routeData.GetAction());
        }
        
        public static string GetArea(this RouteData routeData) {
            return routeData.Values.ContainsKey("area") ? routeData.Values["area"].ToString() : string.Empty;
        }

        public static string GetController(this RouteData routeData) {
            return routeData.Values.ContainsKey("controller") ? routeData.Values["controller"].ToString() : string.Empty;
        }

        public static string GetAction(this RouteData routeData) {
            return routeData.Values.ContainsKey("action") ? routeData.Values["action"].ToString() : string.Empty;
        }
    }
}

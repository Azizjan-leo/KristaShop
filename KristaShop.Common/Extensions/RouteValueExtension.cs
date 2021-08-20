using KristaShop.Common.Models.Structs;

namespace KristaShop.Common.Extensions {
    public static class RouteValueExtension {
        public static string ToKeyString(this RouteValue value, string separator = ".") {
            return $"{value.Area}{separator}{value.Controller}{separator}{value.Action}";
        }
    }
}

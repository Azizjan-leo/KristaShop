using System.Text.RegularExpressions;
using KristaShop.Common.Interfaces.Models;

namespace KristaShop.Common.Extensions {
    public static class CatalogItemExtension {
        public static string GetModelKey(this ICatalogItemBase item) {
            return Regex.Replace($"{item.Articul}_{item.Size.Line}", @"[!@#$%+\/^&*\s]", "_");
        }
        
        public static string GetItemKey(this ICatalogItemBase item) {
            return Regex.Replace($"{item.Articul}_{item.Size.Value}_{item.ColorId}", @"[!@#$%+\/^&*\s]", "_");
        }
    }
}

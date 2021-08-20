using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using KristaShop.Common.Extensions;

namespace KristaShop.Common.Enums {
    public enum CatalogType {
        [Display(Name = "Все каталоги")]
        All = -1,
        [Display(Name = "Открытый каталог")]
        Open = 0,
        [Display(Name = "Наличие сериями")]
        InStockLines = 1,
        [Display(Name = "Наличие не сериями")]
        InStockParts = 2,
        [Display(Name = "Предзаказ")]
        Preorder = 3,
        [Display(Name = "На складе РФ сериями")]
        RfInStockLines = 4,
        [Display(Name = "На складе РФ не сериями")]
        RfInStockParts = 5,
        [Display(Name = "Распродажа сериями")]
        SaleLines = 6,
        [Display(Name = "Распродажа не сериями")]
        SaleParts = 7,
    }  

    public static class CatalogTypeExtensions {
        public static IEnumerable<CatalogType> GetAllCatalogs() {
            return Enum.GetValues<CatalogType>();
        }
        
        public static IEnumerable<CatalogType> GetAllCatalogsExceptOpen() {
            return Enum.GetValues<CatalogType>().Where(x => x > CatalogType.Open);
        }

        public static CatalogType ToProductCatalog1CId(this int value) {
            return value switch {
                < 0 => CatalogType.All,
                > (int) CatalogType.SaleParts => CatalogType.Open,
                _ => (CatalogType) value
            };
        }

        public static bool IsValidCatalog(this CatalogType value) {
            return value is > CatalogType.Open and <= CatalogType.SaleParts;
        }

        public static bool IsCatalogValueCorrect(this int value) {
            switch (value) {
                case < 0:
                case > (int)CatalogType.SaleParts:
                    return false;
                default:
                    return true;
            }
        }

        public static string AsString(this CatalogType value) {
            return value.GetDisplayName();
        }

        public static bool HasEmptySlots(this CatalogType value) {
            return value is CatalogType.Preorder or CatalogType.Open or CatalogType.All;
        }
    }
}

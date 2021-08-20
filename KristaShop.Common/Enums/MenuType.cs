using System.ComponentModel.DataAnnotations;

namespace KristaShop.Common.Enums {
    public enum MenuType {
        [Display(Name = "Верхнее")]
        Top = 1,

        [Display(Name = "Нижнее")]
        Bottom = 100,

        [Display(Name = "Левое")]
        Left = 999
    }
}
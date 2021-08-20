using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes.IsTrueAttribute;

namespace Module.Order.WebUI.Models {
    public class OrderViewModel {
        [Display(Name = "Упаковка")] 
        public bool HasExtraPackage { get; set; }

        [Display(Name = "Комментарий к заказу")]
        [MaxLength(2048, ErrorMessage = "Максимальное количество символов {0}")]
        public string Description { get; set; }

        [Display(Name = "Я ознакомился (лась) *")]
        [IsTrue(ErrorMessage = "Необходимо ознакомиться с условиями")]
        public bool Accepted { get; set; }
    }
}
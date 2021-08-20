using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes.IsTrueAttribute;

namespace Module.Order.WebUI.Models {
    public class GuestOrderViewModel {
        [Display(Name = "Ваше полное имя")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите Ваше полное имя")]
        public string GuestFullName { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите Ваш номер телефона")]
        [RegularExpression(@"^[0-9\-+\s]{6,40}$", ErrorMessage = "Укажите Ваш номер телефона")]
        public string GuestPhone { get; set; }

        [Display(Name = "Я ознакомился (лась) *")]
        [IsTrue(ErrorMessage = "Необходимо ознакомиться с условиями")]
        public bool Accepted { get; set; }
    }
}
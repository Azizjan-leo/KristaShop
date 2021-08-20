using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes;

namespace Module.Client.WebUI.Models {
    public class ChangePasswordViewModel {
        public int UserId { get; set; }

        [Display(Name = "Текущий пароль")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "Новый пароль")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "Длина пароля должна быть больше 3-х символов", MinimumLength = 4)]
#if !DEBUG
        [PasswordComplexity(ErrorMessage = "Введенный пароль слишком простой, введите другой пароль")]
#endif
        public string Password { get; set; }

        [Display(Name = "Подтвержение пароля")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароли должны совпадать")]
        public string ConfirmPassword { get; set; }

        public bool NeedCheckCurrentPassword { get; set; }
    }
}

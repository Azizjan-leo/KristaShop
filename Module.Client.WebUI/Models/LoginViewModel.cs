using System.ComponentModel.DataAnnotations;

namespace Module.Client.WebUI.Models {
    public class LoginViewModel {
        [Required(ErrorMessage = "Заполните поле {0}")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Заполните поле {0}")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
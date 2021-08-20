using System.ComponentModel.DataAnnotations;

namespace Module.App.WebUI.Models {
    public class FeedbackViewModel {
        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Person { get; set; }

        [Display(Name = "Сообщение")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [MaxLength(2000, ErrorMessage = "Превышено максимальное количество символов")]
        public string Message { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Phone { get; set; }

        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [EmailAddress(ErrorMessage = "Неверный формат поля {0}")]
        public string Email { get; set; }

        public string Captcha { get; set; }
    }
}
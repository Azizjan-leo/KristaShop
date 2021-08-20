using System.ComponentModel.DataAnnotations;

namespace Module.App.WebUI.Models {
    public class ManagementContactsFeedbackViewModel {
        [Display(Name = "Сообщение")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [MaxLength(2000, ErrorMessage = "Превышено максимальное количество символов")]
        public string Message { get; set; }
    }
}
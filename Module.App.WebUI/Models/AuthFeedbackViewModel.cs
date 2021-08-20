using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes;
using KristaShop.Common.Attributes.AllowedFileExtensionAttribute;
using Microsoft.AspNetCore.Http;

namespace Module.App.WebUI.Models {
    public class AuthFeedbackViewModel {
        [Display(Name = "Сообщение")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [MaxLength(2000, ErrorMessage = "Превышено максимальное количество символов")]
        public string Message { get; set; }

        [Display(Name = "Файл")]
        [AllowedFileExtensions(".xlsx,.xls,.doc,.docx,.ppt,.pptx,.txt,.pdf,.zip,.rar,image/*", ErrorMessage = "Данное расширение файла не поддерживается")]
        [MaxFileSize(10 * 1024 * 1024, ErrorMessage = "Превышен максимальный размер файла")]
        public IFormFile File { get; set; }
    }
}
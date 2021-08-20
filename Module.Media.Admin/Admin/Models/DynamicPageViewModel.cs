using System;
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes;
using KristaShop.Common.Attributes.AllowedFileExtensionAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.Media.Admin.Admin.Models {
    public class DynamicPageViewModel {
        public Guid Id { get; set; }

        [Display(Name = "Алиас")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string URL { get; set; }

        [Display(Name = "Заглавие")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Title { get; set; }

        [Display(Name = "Иконка заглавия")]
        [AllowedFileExtensions("image/*", ErrorMessage = "Неверный формат файла")]
        [MaxFileSize(20 * 1024, ErrorMessage = "Максимальный размер файла 20 КБ")]
        public IFormFile TitleIcon { get; set; }

        [Display(Name = "Иконка заглавия")] public string TitleIconPath { get; set; }

        [Display(Name = "Изображение")]
        [AllowedFileExtensions("image/*", ErrorMessage = "Неверный формат файла")]
        public IFormFile Image { get; set; }

        [Display(Name = "Изображение")] public string ImagePath { get; set; }

        [Display(Name = "Тело")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Body { get; set; }

        [Display(Name = "Макет")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Layout { get; set; }

        public SelectList Layouts { get; set; }

        public string LayoutName { get; set; }

        [Display(Name = "Для неавторизованных")]
        public bool IsOpen { get; set; } = true;

        [Display(Name = "Самостоятельная страница")]
        public bool IsSinglePage { get; set; }

        [Display(Name = "Показывать в меню")] public bool IsVisibleInMenu { get; set; } = true;

        [Display(Name = "Мета заглавие")] public string MetaTitle { get; set; }

        [Display(Name = "Мета описание")] public string MetaDescription { get; set; }

        [Display(Name = "Мета слова")] public string MetaKeywords { get; set; }

        public int Order { get; set; }
    }
}
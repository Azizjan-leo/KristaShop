using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Module.Media.Admin.Admin.Models {
    public class GalleryItemViewModel {
        public Guid Id { get; set; }

        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Title { get; set; }

        [Display(Name = "Картинка")] public IFormFile Image { get; set; }

        [Display(Name = "Картинка")] public string ImagePath { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Description { get; set; }

        [Display(Name = "Текст ссылки")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string LinkText { get; set; }

        [Display(Name = "Ссылка")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Link { get; set; }

        [Display(Name = "Видимость")] public bool IsVisible { get; set; }

        [Display(Name = "Порядок")] public int Order { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes.AllowedFileExtensionAttribute;
using Microsoft.AspNetCore.Http;

namespace Module.Media.Admin.Admin.Models {
    public class VideoGalleryViewModel {
        public Guid Id { get; set; }

        [Display(Name = "Заголовок")]
        [MinLength(1, ErrorMessage = "Заполните поле {0}")]
        [MaxLength(64, ErrorMessage = "Максимальная длина поля {0} 64 символа")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [MaxLength(2048, ErrorMessage = "Максимальная длина поля {0} 2048 символов")]
        public string Description { get; set; }

        [Display(Name = "Превью")]
        [MaxLength(256, ErrorMessage = "Максимальная длина поля {0} 256 символов")]
        public string PreviewPath { get; set; }

        [Display(Name = "Превью")]
        [AllowedFileExtensions("image/*", ErrorMessage = "Данный формат изображения запрещен")]
        public IFormFile Preview { get; set; }

        [Display(Name = "Ссылка на видео")]
        [MaxLength(256, ErrorMessage = "Максимальная длина поля {0} 256 символов")]
        public string VideoPath { get; set; }

        [Display(Name = "Видимость")] public bool IsVisible { get; set; }

        [Display(Name = "Для неавторизованых")]
        public bool IsOpen { get; set; }

        [Display(Name = "ЧПУ")]
        [MinLength(1, ErrorMessage = "Заполните поле {0}")]
        [MaxLength(64, ErrorMessage = "Максимальная длина поля {0} 64 символа")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Slug { get; set; }

        [Display(Name = "Порядок")] public int Order { get; set; }

        public int VideosCount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes.AllowedFileExtensionAttribute;
using KristaShop.Common.Attributes.RequiredThisOrOtherAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.Media.Admin.Admin.Models {
    public class VideoViewModel {
        public Guid Id { get; set; }

        [Display(Name = "Превью")]
        public string PreviewPath { get; set; }
        
        [Display(Name = "Превью")]
        [RequiredThisOrOther(nameof(PreviewPath), ErrorMessage = "Заполните поле {0}")]
        [AllowedFileExtensions("image/*", ErrorMessage = "Данный формат изображения запрещен")]
        public IFormFile Preview { get; set; }
        
        [Display(Name = "Ссылка на видео")]
        [MaxLength(256, ErrorMessage = "Максимальная длина поля {0} 256 символов")]
        [MinLength(1, ErrorMessage = "Заполните поле {0}")]
        public string VideoPath { get; set; }

        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [MaxLength(64, ErrorMessage = "Максимальная длина поля {0} 64 символа")]
        [MinLength(1, ErrorMessage = "Заполните поле {0}")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [MaxLength(2000, ErrorMessage = "Максимальная длина поля {0} 2000 символов")]
        public string Description { get; set; }

        [Display(Name = "Видимость")]
        public bool IsVisible { get; set; }

        [Display(Name = "Порядок")]
        public int Order { get; set; }

        [Display(Name="Галереи")]
        public List<Guid> GalleryIds { get; set; }

        [Display(Name="Галереи")]
        public SelectList Galleries { get; set; }

        public Guid FromGalleryId { get; set; }

        public VideoViewModel() {
            GalleryIds = new List<Guid>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.Media.Admin.Admin.Models {
    public class BannerItemViewModel {
        public Guid Id { get; set; }

        [Display(Name = "Картинка")] public IFormFile Image { get; set; }

        [Display(Name = "Картинка")] public string ImagePath { get; set; }

        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Title { get; set; }

        [Display(Name = "Подзаголовок")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Caption { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Description { get; set; }

        [Display(Name = "Ссылка")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Link { get; set; }

        [Display(Name = "Видимость")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public bool IsVisible { get; set; }

        [Display(Name = "Порядок")] public int Order { get; set; }

        [Display(Name = "Цвет заголовка")] public string Color { get; set; }

        [Display(Name = "Цвета заголовка")] public List<SelectListItem> Colors { get; set; }
    }
}
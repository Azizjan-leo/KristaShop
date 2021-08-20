using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Module.Catalogs.Admin.Admin.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        
        [Display(Name = "Категория в 1С")]
        public long CategoryId1C { get; set; }

        [Display(Name = "Картинка")]
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Наименование")]
        [RegularExpression("^[А-Яа-я_\\w\\s]*$", ErrorMessage = "Неверный формат: {0}")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Name { get; set; }

        [Display(Name = "Видимость")]
        public bool IsVisible { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Порядок")]
        public int Order { get; set; }
    }
}
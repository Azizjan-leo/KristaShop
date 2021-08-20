using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.Admin.Admin.Models
{
    public class Model1CViewModel {
        public CatalogItemGroupNew FullModelData { get; set; }

        public string Articul { get; set; }
        public string ItemName { get; set; }

        [Display(Name = "Ссылка на видео")]
        public string VideoUrl { get; set; }
        public int CurrentCatalogId { get; set; }

        [Display(Name = "Видимость")]
        public bool IsVisible { get; set; }

        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Цена по умолчанию")]
        public double DefaultPrice { get; set; }

        [Display(Name = "Картинка")]
        public IFormFile Image { get; set; }

        public IFormFileCollection Photos { get; set; }
        public string ImagePath { get; set; }
        
        [Display(Name = "Лимитированная модель")]
        public bool IsLimited { get; set; }

        private string _altText = string.Empty;

        [Display(Name = "Альтернативный текст картинки (alt)")]
        public string ImageAlternativeText { 
            get {
                if (string.IsNullOrEmpty(_altText)) {
                    return $"{Articul} {ItemName}".Trim();
                } else {
                    return _altText;
                }
            }
            set {
                _altText = value;
            }
        }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Description { get; set; }

        [Display(Name = "Материал")]
        public string Matherial { get; set; }

        [Display(Name = "Каталоги")]
        public string[] Catalogs { get; set; }
        
        [Display(Name = "Невидимость в каталогах")]
        public List<int> CatalogsInvisibility { get; set; }

        [Display(Name = "Категории")]
        public string[] Categories { get; set; }

        [Display(Name = "Мета заголовок")]
        public string MetaTitle { get; set; }

        [Display(Name = "Мета слова")]
        public string MetaKeywords { get; set; }

        [Display(Name = "Мета описание")]
        public string MetaDescription { get; set; }

        public Model1CViewModel() {
            CatalogsInvisibility = new List<int>();
        }
    }
}
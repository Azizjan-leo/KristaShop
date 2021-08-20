using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes.AllowedFileExtensionAttribute;
using KristaShop.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Module.Catalogs.Admin.Admin.Models
{
    public class CatalogViewModel
    {
        [Display(Name = "№ каталога")]
        public CatalogType Id { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Name { get; set; }
        
        [Display(Name = "Форма заказа")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public int OrderForm { get; set; }

        [Display(Name = "ЧПУ")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Uri { get; set; }

        [Display(Name = "Описание на главной")]
        public string Description { get; set; }

        [Display(Name = "Мета-заглавие")]
        public string MetaTitle { get; set; }

        [Display(Name = "Мета-слова")]
        public string MetaKeywords { get; set; }

        [Display(Name = "Мета-описание")]
        public string MetaDescription { get; set; }

        [Display(Name = "Описание в каталоге")]
        public string AdditionalDescription { get; set; }

        [Display(Name = "Порядок")]
        public int Order { get; set; }

        [Display(Name = "Запрет на скидки")]
        public bool IsDisableDiscount { get; set; }

        [Display(Name = "Видимость")]
        public bool IsVisible { get; set; }

        [Display(Name = "Открытый каталог")]
        public bool IsOpen { get; set; }

        [Display(Name = "Сериями")]
        public bool IsSet { get; set; }
        [Display(Name = "Превью")]
        [MaxLength(256, ErrorMessage = "Максимальная длина поля {0} 256 символов")]
        public string PreviewPath { get; set; }

        [Display(Name = "Превью")]
        [AllowedFileExtensions("image/*", ErrorMessage = "Данный формат изображения запрещен")]
        public IFormFile Preview { get; set; }

        [Display(Name = "Ссылка на видео")]
        [MaxLength(256, ErrorMessage = "Максимальная длина поля {0} 256 символов")]
        public string VideoPath { get; set; }

        [Display(Name = "Время закрытия каталога")]
        public DateTimeOffset? CloseTime { get; set; }

        [Display(Name = "Наценки каталога")]
        public List<CatalogExtraChargeViewModel> CatalogExtraCharges { get; set; }
        public void FixParametersByCatalogType() {
            switch (Id) {
                case CatalogType.Open:
                    IsOpen = true;
                    IsSet = true;
                    OrderForm = 2;
                    break;
                case CatalogType.InStockLines:
                case CatalogType.RfInStockLines:
                    IsOpen = false;
                    IsSet = true;
                    OrderForm = 1;
                    break;
                case CatalogType.InStockParts:
                case CatalogType.RfInStockParts:
                    IsOpen = false;
                    IsSet = false;
                    OrderForm = 1;
                    break;
                case CatalogType.Preorder:
                    IsOpen = false;
                    IsSet = true;
                    OrderForm = 2;
                    break;
            }
        }
    }
}
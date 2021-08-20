using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes.RequiredThisOrOtherAttribute;
using KristaShop.Common.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.Client.Admin.Admin.Models {
    public class UserUpdateViewModel {
        [Required(ErrorMessage = "Заполните поле {0}")]
        public Guid NewUserId { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Password { get; set; }

        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [RegularExpression("^[а-яА-Я ]+$", ErrorMessage = "{0} должно быть на кириллице.")]
        public string FullName { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Phone { get; set; }

        [Display(Name = "Электронная почта")]
        [EmailAddress(ErrorMessage = "{0} имеет некорректный формат")]
        public string Email { get; set; }

        [Display(Name = "Название торгового центра")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string MallAddress { get; set; }

        [Display(Name = "Адрес")]
        public string CompanyAddress { get; set; }

        [Display(Name = "Город")]
        [RequiredThisOrOther(nameof(NewCity), ErrorMessage = "Заполните поле {0} или {1}")]
        public int? CityId { get; set; }

        [Display(Name = "Название города")]
        public string NewCity { get; set; }

        public SelectList Cities { get; set; }

        public bool Activate { get; set; }

        public int UserId { get; set; }
        [Display(Name = "Менеджер")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите менеджера контрагента")]
        public int? ManagerId { get; set; }
        [Display(Name = "Менеджеры")]
        public SelectList Managers { get; set; }
        [Display(Name = "Видимые каталоги")]
        public List<CatalogType> VisibleCatalogs { get; set; }
        [Display(Name = "Список каталогов")]
        public SelectList CatalogsList { get; set; }
        [Display(Name = "Отправить на Email")]
        public bool SendToEmail { get; set; }

        public UserUpdateViewModel() {
            VisibleCatalogs = new List<CatalogType>();
        }

    }
}
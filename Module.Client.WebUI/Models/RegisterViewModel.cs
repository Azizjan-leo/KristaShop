using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes.IsTrueAttribute;
using KristaShop.Common.Attributes.RequiredThisOrOtherAttribute;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.Client.WebUI.Models {
    public class RegisterViewModel {
        [Required(ErrorMessage = "Заполните поле {0}")]
        [Display(Name = "ФИО")]
        [RegularExpression("^[а-яА-Я ]+$", ErrorMessage = "{0} должно быть на кириллице.")]
        public string FullName { get; set; }

        [Display(Name = "Город")]
        [RequiredThisOrOther(nameof(NewCity), ErrorMessage = "Заполните поле {0} или {1}")]
        public int? CityId { get; set; }

        [Display(Name = "Название города")]
        public string NewCity { get; set; }

        [Display(Name = "Название торгового центра")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string MallAddress { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Phone { get; set; }

        [Display(Name = "Электронная почта")]
        [EmailAddress(ErrorMessage = "Введена некорректная электронная почта")]
        public string Email { get; set; }

        [Display(Name = "Адрес")]
        public string CompanyAddress { get; set; }

        [IsTrue(ErrorMessage = "Для регистрации необходимо согласиться с условиями")]
        public bool IsAgree { get; set; }

        public string TermsOfUse { get; set; }

        public SelectList Cities { get; set; }
    }
}
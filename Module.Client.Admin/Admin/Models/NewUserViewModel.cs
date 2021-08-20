using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.Client.Admin.Admin.Models {
    public class NewUserViewModel {
        public Guid Id { get; set; }
        public int? UserId { get; set; }
        [Display(Name = "Наименование контрагента")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите наименование контрагента")]
        public string FullName { get; set; }
        [Display(Name = "Логин")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите логин")]
        [RegularExpression(@"^[a-z0-9_/-]{3,24}$", ErrorMessage = "Логин должен содержать только латиницу или цифры (не менее 3-х символов)")]
        public string Login { get; set; }
        [Display(Name = "Пароль")]
        [RegularExpression(@"^[0-9]{6,16}$", ErrorMessage = "Пароль должен содержать только цифры (не менее 6-ти)")]
        public string Password { get; set; }
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }
        [Display(Name = "EMail")]
        public string Email { get; set; }
        [Display(Name = "Менеджер")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите менеджера контрагента")]
        public int? ManagerId { get; set; }
        [Display(Name = "Менеджеры")]
        public SelectList Managers { get; set; }
        [Display(Name = "Город")]
        public int? CityId { get; set; }
        [Display(Name = "Города")]
        public SelectList Cities { get; set; }
        [Display(Name = "Город (другой)")]
        [MaxLength(32, ErrorMessage = "Максимальное количество символов {0}")]
        public string OtherCity { get; set; }
        [Display(Name = "Торговый центр")]
        public string MallAddress { get; set; }
        [Display(Name = "Адрес ТЦ")]
        public string CompanyAddress { get; set; }
        [Display(Name = "Видимые каталоги")]
        public List<CatalogType> VisibleCatalogs { get; set; }
        [Display(Name = "Список каталогов")]
        public SelectList CatalogsList { get; set; }
        [Display(Name = "Отправить на Email")]
        public bool SendToEmail { get; set; }
        public string ReturnTo { get; set; }

        public NewUserViewModel() {
            VisibleCatalogs = new List<CatalogType>();
        }
    }
}
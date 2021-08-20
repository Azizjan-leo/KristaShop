using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Client.Admin.Admin.Models.Filters {
    public class UsersFilter {
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Display(Name = "ФИО")]
        public string Person { get; set; }

        [Display(Name = "Город")]
        public string CityName { get; set; }

        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        [Display(Name = "Менеджер")]
        public string Manager { get; set; }
        
        [Display(Name = "Статус корзины")]
        public string CartStatus { get; set; }

        [Display(Name = "Последний вход от")]
        public DateTime? LastSignInFrom { get; set; }

        [Display(Name = "Последний вход до")]
        public DateTime? LastSignInTo { get; set; }

        [Display(Name = "Только новые пользователи")]
        public bool NewUsersOnly { get; set; }
    }
}
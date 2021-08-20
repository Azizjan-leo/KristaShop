using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.App.Admin.Admin.Models {
    public class ManagerDetailsViewModel {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Очередь получения регистраций")]
        public int RegistrationsQueueNumber { get; set; }
        [Display(Name = "Email для уведомлений")]
        public string NotificationsEmail { get; set; }
        [Display(Name = "Уведомления о новых регистрациях")]
        public bool SendNewRegistrationsNotification { get; set; }
        [Display(Name = "Уведомления о новых заказах")]
        public bool SendNewOrderNotification { get; set; }
        [Display(Name = "Тип пользователя")]
        public Guid RoleId { get; set; }
        public SelectList Roles { get; set; }
        public SelectList Managers { get; set; }
        [Display(Name = "Видимость регистраций")]
        public List<int> ManagerIdsForRegistrationsAccess { get; set; }
        [Display(Name = "Видимость заказов")]
        public List<int> ManagerIdsForOrdersAccess { get; set; }
    }
}

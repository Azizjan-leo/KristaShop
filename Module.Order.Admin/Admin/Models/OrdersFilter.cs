using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Order.Admin.Admin.Models {
    public class OrdersFilter {
        [Display(Name = "Менеджер")]
        public string Manager { get; set; }
        [Display(Name = "ФИО клиента")]
        public string ClientFullName { get; set; }
        [Display(Name = "Город")]
        public string CityName { get; set; }
        [Display(Name = "Предзаказ")]
        public bool PreorderOnly { get; set; }
        [Display(Name = "Наличие")]
        public bool InStockOnly { get; set; }
        [Display(Name = "Необработаные")]
        public bool ProcessedOnly { get; set; }
        [Display(Name = "Дата от")]
        public DateTime? OrderDateFrom { get; set; }
        [Display(Name = "Дата до")]
        public DateTime? OrderDateTo { get; set; }
    }
}
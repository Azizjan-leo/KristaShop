using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Order.Admin.Admin.Models {
    public class OrdersTotalFilter {
        [Display(Name = "Артикул")]
        public string Articul { get; set; }
        [Display(Name = "Цвет")]
        public int? ColorId { get; set; }
        [Display(Name = "Размерный ряд")]
        public string SizeValue { get; set; }
        [Display(Name = "Предзаказ")]
        public bool PreorderOnly { get; set; }
        [Display(Name = "Наличие")]
        public bool InStockOnly { get; set; }
        [Display(Name = "Необработаные")]
        public bool UnProcessedOnly { get; set; }
        [Display(Name = "Обработаные")]
        public bool ProcessedOnly { get; set; }
        [Display(Name = "Дата от")]
        public DateTime? OrderDateFrom { get; set; }
        [Display(Name = "Дата до")]
        public DateTime? OrderDateTo { get; set; }

    }
}
using System.ComponentModel.DataAnnotations;

namespace Module.Order.Admin.Admin.Models {
    public class PreOrderModelFilter {
        [Display(Name = "Артикул")]
        public string Articul { get; set; }
        [Display(Name = "Цвет")]
        public int ColorId { get; set; }
        [Display(Name = "Размерный ряд")]
        public string SizeValue { get; set; }
    }
}
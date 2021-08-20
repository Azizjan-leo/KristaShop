using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Client.Admin.Admin.Models {
    public class GuestAccessLinkModel {
        [Display(Name = "Предзаказ")]
        public bool IsPreoderVisible { get; set; }
        [Display(Name = "Наличие сериями")]
        public bool IsInstockByLinesVisible { get; set; }
        [Display(Name = "Наличие не сериями")]
        public bool IsInstockByPartsVisible { get; set; }
        [Display(Name = "Ссылка действительна до")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать дату окончания действия ссылки")]
        public DateTime ExpiredDate { get; set; }
    }
}
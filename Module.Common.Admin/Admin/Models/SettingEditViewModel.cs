using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Common.Admin.Admin.Models {
    public class SettingEditViewModel {
        public Guid Id { get; set; }

        [Display(Name = "Ключ")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Key { get; set; }

        [Display(Name = "Значение")]
        /*[Required(AllowEmptyStrings = true, ErrorMessage = "Заполните поле {0}")]*/
        public string Value { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
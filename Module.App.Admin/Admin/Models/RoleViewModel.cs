using System;
using System.ComponentModel.DataAnnotations;

namespace Module.App.Admin.Admin.Models {
    public class RoleViewModel {
        public Guid Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
    }
}

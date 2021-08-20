using System.Collections.Generic;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KristaShop.Common.Models.Filters {
    public class PartnersFilter {
        public SelectList CitiesSelect { get; set; }
        public List<int> Cities { get; set; }
        public SelectList PartnersSelect { get; set; }
        public List<int> Partners { get; set; }
        public SelectList ManagersSelect { get; set; }
        public List<int> Managers { get; set; }
        public DateRange PaymentDate { get; set; }
        public DateRange RevisionDate { get; set; }

        public PartnersFilter() {
            Cities = new List<int>();
            Partners = new List<int>();
            Managers = new List<int>();
        }
    }
}
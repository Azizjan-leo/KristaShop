using System.Collections.Generic;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KristaShop.Common.Models.Filters {
    public class PaymentsReportFilter {
        public SelectList CitiesSelect { get; set; }
        public List<int> Cities { get; set; }
        public SelectList PartnersSelect { get; set; }
        public List<int> Partners { get; set; }
        public SelectList ManagersSelect { get; set; }
        public List<int> Managers { get; set; }
        public DateRange Date { get; set; }
        public SelectList DocumentTypesSelect { get; set; }
        public List<string> DocumentTypes { get; set; }

        public PaymentsReportFilter() {
            Cities = new List<int>();
            Partners = new List<int>();
            Managers = new List<int>();
            DocumentTypes = new List<string>();
        }
    }
}
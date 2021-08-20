using System.Collections.Generic;
using System.ComponentModel;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KristaShop.Common.Models.Filters {
    public class ReportsFilter {
        [DisplayName("Артикулы")]
        public SelectList Articuls { get; set; }
        public List<string> SelectedArticuls { get; set; }

        [DisplayName("Цвета")]
        public SelectList Colors { get; set; }
        public List<int> SelectedColorIds { get; set; }

        [DisplayName("Города")]
        public SelectList Cities { get; set; }
        public List<int> SelectedCityIds { get; set; }

        [DisplayName("Клиенты")]
        public SelectList Users { get; set; }
        public List<int> SelectedUserIds { get; set; }

        [DisplayName("Менеджеры")]
        public SelectList Managers { get; set; }
        public List<int> SelectedManagerIds { get; set; }

        [DisplayName("Каталоги")]
        public SelectList Catalogs { get; set; }
        public List<int> SelectedCatalogIds { get; set; }
        
        [DisplayName("Каталоги")]
        public SelectList Collections { get; set; }
        public List<int> SelectedCollectionIds { get; set; }

        [DisplayName("Период")]
        public DateRange Date { get; set; }

        public ReportsFilter() {
            SelectedArticuls = new List<string>();
            SelectedColorIds = new List<int>();
            SelectedCityIds = new List<int>();
            SelectedUserIds = new List<int>();
            SelectedManagerIds = new List<int>();
            SelectedCatalogIds = new List<int>();
        }
    }
}

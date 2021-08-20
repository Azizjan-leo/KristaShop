using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KristaShop.Common.Models.Filters {
    public class ModelsFilter {
        private DateTimeOffset _dateTo;
        public string Articul { get; set; }
        public string ColorName { get; set; }
        public SelectList SizesSelect { get; set; }
        public List<string> Sizes { get; set; }
        public DateTimeOffset DateFrom { get; set; }

        public DateTimeOffset DateTo {
            get => _dateTo;
            set => _dateTo = value.AddDays(1).AddSeconds(-1);
        }

        public ModelsFilter() {
            Articul = string.Empty;
            ColorName = string.Empty;
            Sizes = new List<string>();
            DateFrom = DateTimeOffset.Now.Date.AddMonths(-1);
            DateTo = DateTimeOffset.Now.Date;
        }
    }
}
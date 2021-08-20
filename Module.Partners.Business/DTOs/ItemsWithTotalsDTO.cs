using System;
using System.Collections.Generic;
using KristaShop.Common.Models.Structs;

namespace Module.Partners.Business.DTOs {
    public class ItemsWithTotalsDTO<TItems> where TItems : class {
        public List<TItems> Items { get; set; }
        public ReportTotalInfo Totals { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

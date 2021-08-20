using System;

namespace Module.Order.Business.Models {
    public class OrdersTotalFilterDTO {
        public string Articul { get; set; }
        public int? ColorId { get; set; }
        public string SizeValue { get; set; }
        public bool PreorderOnly { get; set; }
        public bool InStockOnly { get; set; }
        public bool UnProcessedOnly { get; set; }
        public bool ProcessedOnly { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
    }
}
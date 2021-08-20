using System;
using System.Collections.Generic;

namespace Module.Common.Business.Models {
    public class SimpleGroupedModelDTO<TItems> {
        public string ModelKey { get; set; }
        public DateTimeOffset Date { get; set; }
        public ModelInfoDTO ModelInfo { get; set; }
        public int TotalAmount { get; set; }
        public double TotalSum { get; set; }
        public List<TItems> Items { get; set; }
    }
}
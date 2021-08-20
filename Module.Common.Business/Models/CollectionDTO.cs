using System;

namespace Module.Common.Business.Models {
    public class CollectionDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Date { get; set; }
        public int PercentValue { get; set; }
        public double Percent => (double)PercentValue / 100;
    }
}

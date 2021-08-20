using System;
using KristaShop.Common.Enums;

namespace Module.Catalogs.Business.Models {
    public class CatalogExtraChargeDTO {
        public Guid Id { get; set; }
        public CatalogType CatalogId { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ExtraChargeType Type { get; set; }
    }
}

using System;
using KristaShop.Common.Enums;

namespace Module.Catalogs.Admin.Admin.Models {
    public class CatalogExtraChargeViewModel {
        public Guid Id { get; set; }
        public CatalogType CatalogId { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ExtraChargeType Type { get; set; }
    }
}

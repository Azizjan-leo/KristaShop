using System;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.CatalogExtraChargeConfiguration"/>
    /// </summary>
    public class CatalogExtraCharge: IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public CatalogType CatalogId { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Catalog Catalog { get; set; }
        public ExtraChargeType Type { get; set; }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}
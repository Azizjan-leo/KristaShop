using KristaShop.Common.Enums;
using KristaShop.DataAccess.Domain.Seeds.Common;
using Microsoft.EntityFrameworkCore;
using System;

namespace KristaShop.DataAccess.Domain.Seeds {
    internal class CatalogsSeed : ISeeds {
        public ModelBuilder Seed(ModelBuilder builder) {
            builder.Entity<Entities.Catalog>().HasData(
                new Entities.Catalog(new Guid("777723b2-cf1c-4a81-ad89-b7eaaedd3109")) {
                    Name = "На складе РФ сериями",
                    Uri = "rf-instock-series",
                    OrderForm = OrderFormType.InStock,
                    IsDiscountDisabled = false,
                    IsVisible = true,
                    IsSet = true,
                    Id = CatalogType.RfInStockLines,
                    Order = 5
                },
                new Entities.Catalog(new Guid("08ea0ff5-2d5f-4b32-b224-1c1616dd11d8")) {
                    Name = "На складе РФ не сериями",
                    Uri = "rf-instock-noseries",
                    OrderForm = OrderFormType.InStock,
                    IsDiscountDisabled = false,
                    IsVisible = true,
                    IsSet = false,
                    Id = CatalogType.RfInStockParts,
                    Order = 6
                }
            );

            return builder;
        }
    }
}
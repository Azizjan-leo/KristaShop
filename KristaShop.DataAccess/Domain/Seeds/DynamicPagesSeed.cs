using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using KristaShop.DataAccess.Domain.Seeds.Common;
using KristaShop.DataAccess.Entities.Media;

namespace KristaShop.DataAccess.Domain.Seeds
{
    internal class DynamicPagesSeed : ISeeds {
        public ModelBuilder Seed(ModelBuilder builder) {
            builder.Entity<DynamicPage>()
                .HasData(
                    new DynamicPage {
                        Id = new Guid("90f3b709-6a99-440c-811c-d32f2753d070"),
                        Title = "Главная страница",
                        Order = 1
                    }
                );

            return builder;
        }
    }
}
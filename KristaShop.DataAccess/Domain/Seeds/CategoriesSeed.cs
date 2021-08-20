using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace KristaShop.DataAccess.Domain.Seeds
{
    internal static class CategoriesSeed
    {
        internal static ModelBuilder CategoryBuilder(this ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category[]
                {
                    new Category { Id = Guid.NewGuid(), Name = "Комбиенезоны", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Универсальные", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Для пышных", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Аксессуары", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Выпускные", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Вечерние", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Для мам", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Свадебные", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Подругам невесты", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Дуэт", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Накидки", IsVisible = true },
                    new Category { Id = Guid.NewGuid(), Name = "Коктейльные", IsVisible = true }
                });

            return builder;
        }
    }
}
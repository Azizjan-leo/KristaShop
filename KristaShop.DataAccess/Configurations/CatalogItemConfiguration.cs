using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KristaShop.DataAccess.Configurations {
    public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem> {
        public void Configure(EntityTypeBuilder<CatalogItem> builder) {
            builder.ToTable("1c_catalog", x => x.ExcludeFromMigrations());

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Model)
                .WithMany(x => x.CatalogItems)
                .HasForeignKey(x => x.ModelId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Color)
                .WithMany()
                .HasForeignKey(x => x.ColorId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.ModelCatalogOrder)
                .WithMany()
                .HasForeignKey(x => new {x.Articul, Catalog = x.CatalogId})
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Invisibility)
                .WithMany()
                .HasForeignKey(x => new {x.Articul, Catalog = x.CatalogId})
                .HasPrincipalKey(x => new {x.Articul, CatalogId = x.CatalogId})
                .OnDelete(DeleteBehavior.Restrict)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.ItemVisibility)
                .WithMany()
                .HasForeignKey(x => new {x.ModelId, x.ColorId, x.SizeValue, Catalog = x.CatalogId})
                .HasPrincipalKey(x => new {x.ModelId, x.ColorId, x.SizeValue, CatalogId = x.CatalogId})
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Catalog)
                .WithMany(x => x.CatalogItems)
                .HasForeignKey(x => x.CatalogId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict)
                .ExcludeForeignKeyFromMigration();

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.ModelId)
                .HasColumnName("model");

            builder.Property(x => x.Articul)
                .HasColumnName("artikul");
            
            builder.Property(x => x.ColorId)
                .HasColumnName("color");

            builder.Property(x => x.SizeValue)
                .HasColumnName("razmer");

            builder.Property(x => x.NomenclatureId)
                .HasColumnName("nomenklatura");

            builder.Property(x => x.Amount)
                .HasColumnName("kolichestvo");

            builder.Property(x => x.CatalogId)
                .HasConversion(new EnumToNumberConverter<CatalogType, int>())
                .HasColumnName("razdel");

            builder.Property(x => x.Price)
                .HasColumnName("price");

            builder.Property(x => x.PriceRub)
                .HasColumnName("price_rub");

            builder.Property(x => x.ExecutionDate)
                .HasColumnName("datav");
        }
    }
}
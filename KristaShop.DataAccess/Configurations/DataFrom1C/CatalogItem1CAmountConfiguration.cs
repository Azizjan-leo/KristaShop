using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    class CatalogItem1CAmountConfiguration : IEntityTypeConfiguration<CatalogItem1CAmount> {
        public void Configure(EntityTypeBuilder<CatalogItem1CAmount> builder) {
            builder.ToView("1c_catalog");
            builder.ToTable("CatalogItem1CAmount", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.ModelId)
                .HasColumnName("model");

            builder.Property(x => x.ColorId)
                .HasColumnName("color");

            builder.Property(x => x.SizeValue)
                .HasColumnName("razmer");

            builder.Property(x => x.NomenclatureId)
                .HasColumnName("nomenklatura");

            /*
            builder.Property(x => x.StorehouseId)
                .HasColumnName("sklad");
            */

            builder.Property(x => x.Amount)
                .HasColumnName("kolichestvo");

            builder.Property(x => x.CatalogId)
                .HasColumnName("razdel");

            builder.Property(x => x.Price)
                .HasColumnName("price");

            builder.Property(x => x.PriceInRub)
                .HasColumnName("price_rub");
        }
    }
}
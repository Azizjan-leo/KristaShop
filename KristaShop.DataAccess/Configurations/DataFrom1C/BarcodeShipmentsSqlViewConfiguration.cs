using System;
using System.Linq;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class BarcodeShipmentsSqlViewConfiguration : IEntityTypeConfiguration<BarcodeShipmentsSqlView> {
        public void Configure(EntityTypeBuilder<BarcodeShipmentsSqlView> builder) {
            builder.ToView(null);
            builder.ToTable("BarcodeShipmentsSqlView", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.Property(v => v.UserId)
                .HasColumnName("user_id");

            builder.Property(v => v.Articul)
                .HasColumnName("articul");

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.Property(v => v.ModelId)
                .HasColumnName("model_id");

            builder.Property(v => v.ColorId)
                .HasColumnName("color_id");

            builder.Property(v => v.ColorName)
                .HasColumnName("color_name");

            builder.Property(v => v.ColorPhoto)
                .HasColumnName("color_photo");

            builder.Property(v => v.ColorValue)
                .HasColumnName("color_group_rgb_value");

            builder.Property(v => v.Size)
                .HasColumnName("size_value")
                .HasConversion(ValueConverters.SizeConverter);

            builder.Property(v => v.Amount)
                .HasColumnName("amount");

            builder.Property(v => v.PartsCount)
                .HasColumnName("parts_count");

            builder.Property(v => v.MainPhoto)
                .HasColumnName("main_photo");

            builder.Property(v => v.PhotoByColor)
                .HasColumnName("photo_by_color");

            builder.Property(v => v.SaleDate)
                .HasColumnName("sale_date");

            builder.Property(v => v.Price)
                .HasColumnName("cena");

            builder.Property(v => v.PriceInRub)
                .HasColumnName("cena_rub");

            builder.Property(v => v.Barcodes)
                .HasColumnName("barcodes")
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
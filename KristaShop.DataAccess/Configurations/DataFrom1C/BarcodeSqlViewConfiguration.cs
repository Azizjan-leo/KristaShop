using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class BarcodeSqlViewConfiguration : IEntityTypeConfiguration<BarcodeSqlView> {
        public void Configure(EntityTypeBuilder<BarcodeSqlView> builder) {
            builder.ToView(null);
            builder.ToTable("BarcodeSqlView", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.Property(v => v.Barcode)
                .HasColumnName("barcode");

            builder.Property(v => v.Articul)
                .HasColumnName("articul");

            builder.Property(v => v.ModelId)
                .HasColumnName("model");

            builder.Property(v => v.ColorId)
                .HasColumnName("color");

            builder.Property(v => v.Size)
                .HasColumnName("razmer")
                .HasConversion(ValueConverters.SizeConverter);

            builder.Property(x => x.Price)
                .HasColumnName("price");

            builder.Ignore(x => x.PriceInRub);
        }
    }
}
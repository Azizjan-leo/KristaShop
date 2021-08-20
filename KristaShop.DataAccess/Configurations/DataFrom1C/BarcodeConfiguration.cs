using KristaShop.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class BarcodeConfiguration : IEntityTypeConfiguration<Barcode> {
        public void Configure(EntityTypeBuilder<Barcode> builder) {
            builder.ToTable("1c_barcodes", x => x.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Model)
                .WithMany(x => x.Barcodes)
                .HasForeignKey(x => x.ModelId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Color)
                .WithMany()
                .HasForeignKey(x => x.ColorId)
                .ExcludeForeignKeyFromMigration();
            
            builder.Property(x => x.Id)
                .HasColumnName("id");
            
            builder.Property(x => x.Value)
                .HasColumnName("barcode");

            builder.Property(x => x.ModelId)
                .HasColumnName("model");

            builder.Property(x => x.ColorId)
                .HasColumnName("color");

            builder.Property(x => x.SizeValue)
                .HasColumnName("razmer");
        }
    }
}
using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment> {
        public void Configure(EntityTypeBuilder<Shipment> builder) {
            builder.ToTable("1c_prodagi_klientov", x => x.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Model)
                .WithMany()
                .HasForeignKey(x => x.ModelId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Color)
                .WithMany()
                .HasForeignKey(x => x.ColorId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Collection)
                .WithMany()
                .HasForeignKey(x => x.CollectionId)
                .ExcludeForeignKeyFromMigration();

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.UserId)
                .HasColumnName("klient");

            builder.Property(x => x.ModelId)
                .HasColumnName("model");

            builder.Property(x => x.ColorId)
                .HasColumnName("color");

            builder.Property(x => x.SizeValue)
                .HasColumnName("razmer");

            builder.Property(x => x.Amount)
                .HasColumnName("kolichestvo");

            builder.Property(x => x.Price)
                .HasColumnName("cena");

            builder.Property(x => x.PriceInRub)
                .HasColumnName("cena_rub");

            builder.Property(x => x.ShipmentDate)
                .HasColumnName("datav");

            builder.Property(x => x.AttachmentsNumber)
                .HasColumnName("schet");

            builder.Property(x => x.CollectionId)
                .HasColumnName("collection");
        }
    }
}
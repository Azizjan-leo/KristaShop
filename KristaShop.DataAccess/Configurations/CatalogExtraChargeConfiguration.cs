using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KristaShop.DataAccess.Configurations {
    public class CatalogExtraChargeConfiguration : IEntityTypeConfiguration<CatalogExtraCharge> {
        public void Configure(EntityTypeBuilder<CatalogExtraCharge> builder) {
            builder.ToTable("catalog_extra_charges");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Catalog)
                .WithMany(x => x.CatalogExtraCharges)
                .HasForeignKey(x => x.CatalogId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.CatalogId)
               .HasConversion(new EnumToNumberConverter<CatalogType, int>())
               .HasColumnName("catalog_id")
               .HasColumnType("int")
               .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            builder.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .IsRequired(false);

            builder.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .IsRequired(false);

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired();
        }
    }
}
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KristaShop.DataAccess.Configurations {
    public class ModelCatalogOrderConfiguration : IEntityTypeConfiguration<ModelCatalogOrder> {
        public void Configure(EntityTypeBuilder<ModelCatalogOrder> builder) {
            builder.ToTable("nom_catalog_1c");

            builder.HasKey(x => new { x.Articul, CatalogId = x.CatalogId });
            
            builder.Property(x => x.Articul)
                .HasColumnName("articul")
                .HasColumnType("varchar(64)")
                .IsRequired();

            builder.Property(x => x.CatalogId)
                .HasColumnName("catalog_id")
                .HasConversion(new EnumToNumberConverter<CatalogType, int>());

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .IsRequired();
        }
    }
}
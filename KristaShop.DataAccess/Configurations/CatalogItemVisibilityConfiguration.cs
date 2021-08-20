using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class CatalogItemVisibilityConfiguration : IEntityTypeConfiguration<CatalogItemVisibility> {
        public void Configure(EntityTypeBuilder<CatalogItemVisibility> builder) {
            builder.ToTable("catalog_item_visibility");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Articul)
                .HasColumnName("articul")
                .HasMaxLength(255)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(x => x.ModelId)
                .HasColumnName("model_id")
                .IsRequired();

            builder.Property(x => x.SizeValue)
                .HasColumnName("size_value")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)")
                .IsRequired(true);

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id");

            builder.Property(x => x.CatalogId)
                .HasColumnName("catalog_id");

            builder.Property(x => x.IsVisible)
                .HasColumnName("is_visible");

            builder.Ignore(x => x.Size);
        }
    }
}
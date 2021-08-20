using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class CatalogItemForAddConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.CatalogItemForAdd> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.CatalogItemForAdd> builder) {
            builder.ToView("1c_catalog_item_for_add");
            builder.ToTable("CatalogItemForAdd", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.CatalogId)
                .HasColumnName("razdel");
            builder
                .Property(v => v.CatalogName)
                .HasColumnName("catalog_name");
            builder
                .Property(v => v.Articul)
                .HasColumnName("articul");
            builder
                .Property(v => v.ModelId)
                .HasColumnName("model");
            builder
                .Property(v => v.ColorId)
                .HasColumnName("color");
            builder
                .Property(v => v.MainPhoto)
                .HasColumnName("main_photo");
            builder
                .Property(v => v.ColorName)
                .HasColumnName("color_name");
            builder
                .Property(v => v.ColorValue)
                .HasColumnName("color_group_rgb_value");
            builder
                .Property(v => v.ColorPhoto)
                .HasColumnName("color_photo");
            builder
                .Property(v => v.NomenclatureId)
                .HasColumnName("nomenklatura");
            builder
                .Property(v => v.PartsCount)
                .HasColumnName("parts_count");
            builder
                .Property(v => v.Amount)
                .HasColumnName("amount");
            builder
                .Property(v => v.Size)
                .HasColumnName("razmer");
            builder
                .Property(v => v.SizeLine)
                .HasColumnName("line");
            builder
                .Property(v => v.MainPhoto)
                .HasColumnName("main_photo");
            builder
                .Property(v => v.PhotoByColor)
                .HasColumnName("photo_by_color");
        }
    }
}

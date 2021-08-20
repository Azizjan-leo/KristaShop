using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class CatalogItemFullConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.CatalogItemFull> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.CatalogItemFull> builder) {
            builder.ToView("1c_catalog_item_full");
            builder.ToTable("CatalogItemFull", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.Id)
                .HasColumnName("id");
            builder
                .Property(v => v.ModelId)
                .HasColumnName("model_id");
            builder
                .Property(v => v.ItemsCount)
                .HasColumnName("kolichestvo");
            builder
                .Property(v => v.CatalogId)
                .HasColumnName("razdel_id");
            builder
                .Property(v => v.Price)
                .HasColumnName("price");
            builder
                .Property(v => v.PriceInRub)
                .HasColumnName("price_rub");
            builder
                .Property(v => v.Discount)
                .HasColumnName("discount");
            builder
                .Property(v => v.ModelName)
                .HasColumnName("model_name");
            builder
                .Property(v => v.Articul)
                .HasColumnName("articul");
            builder
                .Property(v => v.Status)
                .HasColumnName("status");
            builder
                .Property(v => v.PartsCount)
                .HasColumnName("razmerov");
            builder
                .Property(v => v.Weight)
                .HasColumnName("weight");
            builder
                .Property(v => v.SizeLine)
                .HasColumnName("size_line");
            builder
                .Property(v => v.Size)
                .HasColumnName("size");
            builder
                .Property(v => v.NomenclatureId)
                .HasColumnName("nomenclature_id");
            builder
                .Property(v => v.MaterialId)
                .HasColumnName("material_id");
            builder
                .Property(v => v.MaterialName)
                .HasColumnName("material_name");
            builder
                .Property(v => v.ColorId)
                .HasColumnName("color_id");
            builder
                .Property(v => v.ColorGroupId)
                .HasColumnName("color_group_id");
            builder
                .Property(v => v.ColorGroupName)
                .HasColumnName("color_group_name");
            builder
                .Property(v => v.ColorName)
                .HasColumnName("color_name");
            builder
                .Property(v => v.ColorValue)
                .HasColumnName("color_group_rgb_value");
            builder
                .Property(v => v.ColorPhoto)
                .HasColumnName("color_photo");
            builder.Property(x => x.IsVisibleColor)
                .HasColumnName("is_visible_color");
        }
    }
}

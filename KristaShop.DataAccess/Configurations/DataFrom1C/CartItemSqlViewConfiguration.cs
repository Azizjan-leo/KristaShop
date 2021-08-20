using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class CartItemSqlViewConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.CartItemSqlView> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.CartItemSqlView> builder) {
            builder.ToTable("CartItemSqlView", t => t.ExcludeFromMigrations());
            builder.ToView("1c_cart_item");
            builder.HasNoKey();

            builder
                .Property(v => v.Id)
                .HasColumnName("id");
            builder
                .Property(v => v.UserId)
                .HasColumnName("user_id");
            builder
                .Property(v => v.UserFullName)
                .HasColumnName("user_fullname");
            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id");

            builder.Property(x => x.ManagerName)
                .HasColumnName("manager_name");

            builder.Property(x => x.CityId)
                .HasColumnName("city_id");

            builder.Property(x => x.CityName)
                .HasColumnName("city_name");
            builder
                .Property(v => v.CatalogId)
                .HasColumnName("catalog_id");
            builder
                .Property(v => v.Articul)
                .HasColumnName("articul");
            builder
                .Property(v => v.ModelId)
                .HasColumnName("model_id");
            builder
                .Property(v => v.NomenclatureId)
                .HasColumnName("nomenclature_id");
            builder
                .Property(v => v.ColorId)
                .HasColumnName("color_id");
            builder
                .Property(v => v.ColorName)
                .HasColumnName("color_name");
            builder
                .Property(v => v.ColorPhoto)
                .HasColumnName("color_photo");
            builder
                .Property(v => v.ColorValue)
                .HasColumnName("color_group_rgb_value");
            builder
                .Property(v => v.SizeValue)
                .HasColumnName("size_value");
            builder
                .Property(v => v.Price)
                .HasColumnName("price");
            builder
                .Property(v => v.PriceInRub)
                .HasColumnName("price_rub");
            builder
                .Property(v => v.Amount)
                .HasColumnName("amount");
            builder
                .Property(v => v.PartsCount)
                .HasColumnName("parts_count");
            builder
                .Property(v => v.CreatedDate)
                .HasColumnName("created_date");
            builder
                .Property(v => v.CatalogName)
                .HasColumnName("catalog_name");
            builder
                .Property(v => v.MainPhoto)
                .HasColumnName("main_photo");
            builder
                .Property(v => v.PhotoByColor)
                .HasColumnName("photo_by_color");
        }
    }
}

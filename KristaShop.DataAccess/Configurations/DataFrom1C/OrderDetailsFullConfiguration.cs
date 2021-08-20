using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class OrderDetailsFullConfiguration : IEntityTypeConfiguration<OrderDetailsFull> {
        public void Configure(EntityTypeBuilder<OrderDetailsFull> builder) {
            builder.ToView(null);
            builder.ToTable("OrderDetailsFull", t => t.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id");

            builder.Property(x => x.CatalogId)
                .HasColumnName("catalog_id");

            builder.Property(x => x.CatalogName)
                .HasColumnName("catalog_name");

            builder.Property(x => x.Articul)
                .HasColumnName("articul");

            builder.Property(x => x.MainPhoto)
                .HasColumnName("main_photo");

            builder.Property(x => x.PhotoByColor)
                .HasColumnName("photo_by_color");

            builder.Property(x => x.ModelId)
                .HasColumnName("model_id");

            builder.Property(x => x.NomenclatureId)
                .HasColumnName("nomenclature_id");

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id");

            builder.Property(x => x.ColorName)
                .HasColumnName("color_name");

            builder.Property(x => x.ColorValue)
                .HasColumnName("color_group_rgb_value");

            builder.Property(x => x.ColorPhoto)
                .HasColumnName("color_photo");

            builder.Property(x => x.Size)
                .HasColumnName("size_value")
                .HasConversion(ValueConverters.SizeConverter);

            builder.Property(x => x.Amount)
                .HasColumnName("amount");

            builder.Property(x => x.Price)
                .HasColumnName("price");

            builder.Property(x => x.PriceInRub)
                .HasColumnName("price_in_rub");
            
            builder.Property(x => x.CollectionId)
                .HasColumnName("collection_id");

            builder.Property(x => x.CollectionName)
                .HasColumnName("collection_name");
            
            builder.Property(x => x.CollectionPrepayPercent)
                .HasColumnName("collection_percent");
            
            builder.Ignore(x => x.PrepayPercent);
        }
    }
}
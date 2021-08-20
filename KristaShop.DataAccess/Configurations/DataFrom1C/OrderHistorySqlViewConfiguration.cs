using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class OrderHistorySqlViewConfiguration : IEntityTypeConfiguration<OrderHistorySqlView> {
        public void Configure(EntityTypeBuilder<OrderHistorySqlView> builder) {
            builder.ToView(null);
            builder.ToTable("OrderHistorySqlView", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(v => v.UserId)
                .HasColumnName("user_id");

            builder.Property(v => v.Articul)
                .HasColumnName("articul");

            builder.Property(v => v.ModelId)
                .HasColumnName("model_id");
            
            builder.Property(v => v.ModelName)
                .HasColumnName("model_name");

            builder.Property(v => v.ColorId)
                .HasColumnName("color_id");

            builder.Property(v => v.ColorName)
                .HasColumnName("color_name");

            builder.Property(v => v.ColorPhoto)
                .HasColumnName("color_photo");
 
            builder.Property(v => v.ColorValue)
                .HasColumnName("color_group_rgb_value");

            builder.Property(v => v.Size)
                .HasColumnName("size_value")
                .HasConversion(ValueConverters.SizeConverter);

            builder.Property(v => v.MainPhoto)
                .HasColumnName("main_photo");

            builder.Property(v => v.PhotoByColor)
                .HasColumnName("photo_by_color");

            builder.Property(v => v.Price)
                .HasColumnName("price");

            builder.Property(v => v.Amount)
                .HasColumnName("amount");

            builder.Property(v => v.OrderDate)
                .HasColumnName("order_date");

            builder.Property(v => v.CollectionId)
                .HasColumnName("collection_id");

            builder.Property(v => v.CollectionName)
                .HasColumnName("collection_name");

            builder.Property(x => x.CollectionPrepayPercent)
                .HasColumnName("collection_percent");

            builder.Ignore(x => x.PrepayPercent);
        }
    }
}
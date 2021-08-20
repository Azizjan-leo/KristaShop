using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Views.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class PartnerStorehouseHistoryItemSqlViewConfiguration : IEntityTypeConfiguration<PartnerStorehouseHistoryItemSqlView>{
        public void Configure(EntityTypeBuilder<PartnerStorehouseHistoryItemSqlView> builder) {
            builder.ToTable("PartnerStorehouseHistoryItemSqlView", t => t.ExcludeFromMigrations());
            builder.ToView(null);
            builder.HasNoKey();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.Articul)
                .HasColumnName("articul");

            builder.Property(x => x.Name)
             .HasColumnName("model_name");

            builder.Property(x => x.ModelId)
                .HasColumnName("model_id");

            builder.Property(x => x.MainPhoto)
                .HasColumnName("main_photo");

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id");

            builder.Property(z => z.ColorName)
                .HasColumnName("color_name");

            builder.Property(z => z.ColorCode)
                .HasColumnName("color_code");

            builder.Property(z => z.ColorImage)
                .HasColumnName("color_photo");

            builder.Property(x => x.Size)
                .HasColumnName("size_value")
                .HasConversion(ValueConverters.SizeConverter);

            builder.Property(x => x.Amount)
                .HasColumnName("amount");

            builder.Property(x => x.Price)
                .HasColumnName("price");

            builder.Property(x => x.PriceInRub)
                .HasColumnName("price_rub");
            
            builder.Property(x => x.MovementType)
                .HasColumnName("movement_type")
                .IsRequired();

            builder.Property(x => x.Direction)
                .HasColumnName("movement_direction")
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .HasColumnName("item_create_date");
        }
    }
}

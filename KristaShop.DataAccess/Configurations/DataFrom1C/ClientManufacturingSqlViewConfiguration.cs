using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ClientManufacturingSqlViewConfiguration : IEntityTypeConfiguration<ClientManufacturingItemSqlView> {
        public void Configure(EntityTypeBuilder<ClientManufacturingItemSqlView> builder) {
            builder.ToView("1c_client_manufactoring_item");
            builder.ToTable("ClientManufactoringSqlView",
                t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(v => v.Id)
                .HasColumnName("id");
            
            builder.Property(v => v.UserId)
                .HasColumnName("user_id");
            
            builder.Property(v => v.Articul)
                .HasColumnName("articul");
            
            builder.Property(v => v.ModelId)
                .HasColumnName("model_id");
            
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
                .HasColumnName("cena");
            
            builder.Property(v => v.PriceInRub)
                .HasColumnName("cena_rub");
            
            builder.Property(v => v.InKroyAmount)
                .HasColumnName("kroitsya");
            
            builder.Property(v => v.InKroyGotovAmount)
                .HasColumnName("gotovkroy");
            
            builder.Property(v => v.InZapuskAmount)
                .HasColumnName("zapusk");
            
            builder.Property(v => v.InVPoshiveAmount)
                .HasColumnName("vposhive");
            
            builder.Property(v => v.InSkladGPAmount)
                .HasColumnName("skladgp");

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
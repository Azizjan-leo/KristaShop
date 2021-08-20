using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class MoneyDocumentItemSqlViewConfiguration : IEntityTypeConfiguration<MoneyDocumentItemSqlView> {
        public void Configure(EntityTypeBuilder<MoneyDocumentItemSqlView> builder) {
            builder.ToView(null);
            builder.ToTable("MoneyDocumentItemSqlView",  t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.UserId)
                .HasColumnName("klient");

            builder.Property(x => x.DocumentId)
                .HasColumnName("id_doc");

            builder.Property(x => x.Articul)
                .HasColumnName("articul");

            builder.Property(x => x.ModelId)
                .HasColumnName("model");
            
            builder.Property(v => v.MainPhoto)
                .HasColumnName("main_photo");
            
            builder.Property(v => v.PhotoByColor)
                .HasColumnName("photo_by_color");

            builder.Property(v => v.Size)
                .HasColumnName("size_value")
                .HasConversion(ValueConverters.SizeConverter);
            
            builder.Property(x => x.ColorId)
                .HasColumnName("color");
            
            builder.Property(v => v.ColorName)
                .HasColumnName("color_name");
            
            builder.Property(v => v.ColorPhoto)
                .HasColumnName("color_photo");
            
            builder.Property(v => v.ColorValue)
                .HasColumnName("color_group_rgb_value");
            
            builder.Property(v => v.Price)
                .HasColumnName("price");
            
            builder.Property(v => v.PriceInRub)
                .HasColumnName("price_rub");
            
            builder.Property(v => v.Amount)
                .HasColumnName("amount");
        }
    }
}
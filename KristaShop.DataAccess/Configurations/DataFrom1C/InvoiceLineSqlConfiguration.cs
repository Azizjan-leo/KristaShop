using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class InvoiceLineSqlConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.InvoiceLineSql> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.InvoiceLineSql> builder) {
            builder.ToView("1c_invoice_line");
            builder.ToTable("InvoiceLineSql", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.Id)
                .HasColumnName("id");
            builder
                .Property(v => v.InvoiceId)
                .HasColumnName("id_dok");
            builder
                .Property(v => v.UserId)
                .HasColumnName("klient");
            builder
                .Property(v => v.ModelId)
                .HasColumnName("model");
            builder
                .Property(v => v.ColorId)
                .HasColumnName("color");
            builder
                .Property(v => v.Size)
                .HasColumnName("razmer");
            builder
                .Property(v => v.SizeLine)
                .HasColumnName("size_line");
            builder
                .Property(v => v.Description)
                .HasColumnName("osnovanie");
            builder
                .Property(v => v.IsProductLine)
                .HasColumnName("flstr");
            builder
                .Property(v => v.Amount)
                .HasColumnName("kolichestvo");
            builder
                .Property(v => v.Price)
                .HasColumnName("cena");
            builder
                .Property(v => v.Articul)
                .HasColumnName("articul");
            builder
                .Property(v => v.PartsCount)
                .HasColumnName("parts_count");
            builder
                .Property(v => v.ColorName)
                .HasColumnName("color_name");
            builder
                .Property(v => v.ColorPhoto)
                .HasColumnName("color_photo");
            builder
                .Property(v => v.ColorGroup)
                .HasColumnName("color_group_name");
            builder
                .Property(v => v.ColorValue)
                .HasColumnName("color_group_rgb_value");
            builder
                .Property(v => v.PhotoByColor)
                .HasColumnName("photo_by_color");
            builder
                .Property(v => v.MainPhoto)
                .HasColumnName("main_photo");
        }
    }
}

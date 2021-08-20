using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class BannerItemConfiguration : IEntityTypeConfiguration<BannerItem> {
        public void Configure(EntityTypeBuilder<BannerItem> builder) {
            builder.ToTable("banner_items");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Caption)
                .HasColumnName("caption")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ImagePath)
                .HasColumnName("image_path")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.Link)
                .HasColumnName("link")
                .HasMaxLength(128)
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder.Property(x => x.IsVisible)
                .HasColumnName("is_visible")
                .IsRequired();

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .IsRequired();

            builder.Property(x => x.TitleColor)
                .HasColumnName("title_color")
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class GalleryItemConfiguration : IEntityTypeConfiguration<GalleryItem> {
        public void Configure(EntityTypeBuilder<GalleryItem> builder) {
            builder.ToTable("gallery_items");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.ImagePath)
                .HasColumnName("image_path")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.LinkText)
                .HasColumnName("link_text")
                .HasMaxLength(128)
                .HasColumnType("varchar(128)")
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
        }
    }
}

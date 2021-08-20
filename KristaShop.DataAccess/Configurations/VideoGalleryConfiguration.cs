using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class VideoGalleryConfiguration : IEntityTypeConfiguration<VideoGallery> {
        public void Configure(EntityTypeBuilder<VideoGallery> builder) {
            builder.ToTable("video_gallery");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2048)
                .HasColumnType("varchar(2048)")
                .IsRequired(false);

            builder.Property(x => x.PreviewPath)
                .HasColumnName("preview_path")
                .HasMaxLength(256)
                .HasDefaultValueSql("''")
                .IsRequired(false);

            builder.Property(x => x.VideoPath)
                .HasColumnName("video_path")
                .HasMaxLength(256)
                .HasDefaultValueSql("''")
                .IsRequired(false);

            builder.Property(x => x.IsVisible)
                .HasColumnName("is_visible")
                .IsRequired();

            builder.Property(x => x.IsOpen)
                .HasColumnName("is_open")
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}

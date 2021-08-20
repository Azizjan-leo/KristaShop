using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class VideoConfiguration : IEntityTypeConfiguration<Video> {
        public void Configure(EntityTypeBuilder<Video> builder) {
            builder.ToTable("gallery_video");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.PreviewPath)
                .HasColumnName("preview_path")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.VideoPath)
                .HasColumnName("video_path")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2000)
                .IsRequired(false);

            builder.Property(x => x.IsVisible)
                .HasColumnName("is_visible")
                .IsRequired();
        }
    }
}
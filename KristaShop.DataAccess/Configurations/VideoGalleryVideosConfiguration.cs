using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class VideoGalleryVideosConfiguration : IEntityTypeConfiguration<VideoGalleryVideos> {
        public void Configure(EntityTypeBuilder<VideoGalleryVideos> builder) {
            builder.ToTable("video_gallery_videos");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Gallery)
                .WithMany(x => x.VideoGalleryVideos)
                .HasForeignKey(x => x.GalleryId);

            builder.HasOne(x => x.Video)
                .WithMany(x => x.VideoGalleryVideos)
                .HasForeignKey(x => x.VideoId);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.GalleryId)
                .HasColumnName("gallery_id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.VideoId)
                .HasColumnName("video_id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}

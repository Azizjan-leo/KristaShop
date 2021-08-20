using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class PromoLinkConfiguration  : IEntityTypeConfiguration<PromoLink> {
        public void Configure(EntityTypeBuilder<PromoLink> builder) {
            builder.ToTable("promo_link");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Link)
                .HasColumnName("link")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id")
                .IsRequired();

            builder.Property(x => x.DeactivateTime)
                .HasColumnName("link_deactivate_time")
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2048)
                .HasColumnType("varchar(2048)")
                .IsRequired(false);

            builder.Property(x => x.VideoPath)
                .HasColumnName("video_link")
                .HasMaxLength(256)
                .IsRequired(false);

            builder.Property(x => x.VideoPreviewPath)
                .HasColumnName("video_preview_link")
                .HasMaxLength(256)
                .IsRequired(false);
        }
    }
}
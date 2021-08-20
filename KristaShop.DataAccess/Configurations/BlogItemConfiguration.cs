using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class BlogItemConfiguration : IEntityTypeConfiguration<BlogItem> {
        public void Configure(EntityTypeBuilder<BlogItem> builder) {
            builder.ToTable("blog_items");

            builder.HasKey(x => x.Id);

            builder.Property(x=>x.Id)
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
                .HasMaxLength(64)
                .HasColumnType("varchar(64)")
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

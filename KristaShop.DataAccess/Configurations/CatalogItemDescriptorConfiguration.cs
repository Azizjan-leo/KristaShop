using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class CatalogItemDescriptorConfiguration : IEntityTypeConfiguration<CatalogItemDescriptor> {
        public void Configure(EntityTypeBuilder<CatalogItemDescriptor> builder) {
            builder.ToTable("catalog_item_descriptor");

            builder.HasKey(x => x.Articul);

            builder.Property(x => x.Articul)
                .HasColumnName("articul")
                .HasColumnType("varchar(64)")
                .IsRequired();

            builder.Property(x => x.IsVisible)
                .HasColumnName("is_visible")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x=>x.MainPhoto)
                .HasColumnName("main_photo")
                .HasColumnType("varchar(64)");

            builder.Property(x => x.AddDate)
                .HasColumnName("add_date")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.Matherial)
                .HasColumnName("matherial")
                .HasColumnType("text");

            builder.Property(x => x.AltText)
                .HasColumnName("alt_text")
                .HasColumnType("text");

            builder.Property(x => x.VideoLink)
                .HasColumnName("video_link")
                .HasColumnType("text");

            builder.Property(x => x.MetaTitle)
                .HasColumnName("meta_title")
                .HasColumnType("text");

            builder.Property(x => x.MetaKeywords)
                .HasColumnName("meta_keywords")
                .HasColumnType("text");

            builder.Property(x => x.MetaDescription)
                .HasColumnName("meta_description")
                .HasColumnType("text");

            builder.Property(x => x.IsLimited)
                .HasColumnName("is_limited");
        }
    }
}
using System;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class DynamicPageConfiguration : IEntityTypeConfiguration<DynamicPage> {
        public void Configure(EntityTypeBuilder<DynamicPage> builder) {
            builder.ToTable("menu_contents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Url)
                .HasColumnName("url")
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.TitleIconPath)
                .HasColumnName("title_icon_path")
                .IsRequired(false)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(256);

            builder.Property(x => x.ImagePath)
                .HasColumnName("image_path")
                .IsRequired(false)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(256);

            builder.Property(x => x.Body)
                .HasColumnName("body")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.Layout)
                .HasColumnName("layout")
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.IsOpen)
                .HasColumnName("is_authorize");

            builder.Property(x => x.IsSinglePage)
                .HasColumnName("is_single_page")
                .IsRequired();

            builder.Property(x => x.IsVisibleInMenu)
                .HasColumnName("is_visible_in_menu")
                .IsRequired();

            builder.Property(x => x.MetaTitle)
                .HasColumnName("meta_title")
                .HasMaxLength(500);

            builder.Property(x => x.MetaDescription)
                .HasColumnName("meta_description")
                .HasMaxLength(500);

            builder.Property(x => x.MetaKeywords)
                .HasColumnName("meta_keywords")
                .HasMaxLength(500);

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}

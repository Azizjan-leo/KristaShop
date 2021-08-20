using System;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KristaShop.DataAccess.Configurations {
    public class CatalogConfiguration : IEntityTypeConfiguration<Catalog> {
        public void Configure(EntityTypeBuilder<Catalog> builder) {
            builder.ToTable("dict_catalogs");

            builder.HasKey("_id");

            builder.Property<Guid>("_id")
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Id)
                .HasConversion(new EnumToNumberConverter<CatalogType, int>())
                .HasColumnName("catalog_id_1c")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(64)
                .IsRequired();   
            
            builder.Property(x => x.Uri)
                .HasColumnName("uri")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.OrderForm)
                .HasColumnName("order_form")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(512)
                .HasColumnType("varchar(512)")
                .IsRequired(false); 
            
            builder.Property(x => x.MetaTitle)
               .HasColumnName("meta_title")
               .HasMaxLength(512)
               .HasColumnType("varchar(512)")
               .IsRequired(false);

            builder.Property(x => x.MetaKeywords)
               .HasColumnName("meta_keywords")
               .HasMaxLength(512)
               .HasColumnType("varchar(512)")
               .IsRequired(false);

            builder.Property(x => x.MetaDescription)
                .HasColumnName("meta_description")
                .HasMaxLength(512)
                .HasColumnType("varchar(512)")
                .IsRequired(false);

            builder.Property(x => x.AdditionalDescription)
                .HasColumnName("additional_description")
                .HasMaxLength(4096)
                .HasColumnType("varchar(4096)")
                .IsRequired(false);

            builder.Property(x => x.Order)
                 .HasColumnName("order")
                 .HasColumnType("int")
                 .IsRequired();

            builder.Property(x => x.IsDiscountDisabled)
              .HasColumnName("is_disable_discount")
              .IsRequired();

            builder.Property(x => x.IsVisible)
                .HasColumnName("is_visible")
                .IsRequired();

            builder.Property(x => x.IsOpen)
                .HasColumnName("is_open")
                .IsRequired();

            builder.Property(x => x.IsSet)
                .HasColumnName("is_set")
                .IsRequired();

            builder.Property(x => x.PreviewPath)
                .HasColumnName("preview_path")
                .HasMaxLength(256);

            builder.Property(x => x.VideoPath)
               .HasColumnName("video_path")
               .HasMaxLength(256);

            builder.Property(x => x.CloseTime)
               .HasColumnName("close_time");
        }
    }
}
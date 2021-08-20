using System;
using System.Collections.Generic;
using System.Text;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations
{
    public class FaqSectionContentConfiguration : IEntityTypeConfiguration<FaqSectionContent>
    {
        public void Configure(EntityTypeBuilder<FaqSectionContent> builder)
        {
            builder.ToTable("faq_section_content");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Content)
                .HasColumnName("content")
                .HasMaxLength(5120)
                .HasColumnType("varchar(5120)")
                .IsRequired();

            builder.Property(x => x.FaqSectionId)
                .HasColumnName("faq_section_id")
                .HasColumnType("binary(16)")
                .IsRequired();
        }
    } 
}

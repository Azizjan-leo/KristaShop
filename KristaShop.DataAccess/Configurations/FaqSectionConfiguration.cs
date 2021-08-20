using System;
using System.Collections.Generic;
using System.Text;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations
{
    public class FaqSectionConfiguration : IEntityTypeConfiguration<FaqSection>
    {
        public void Configure(EntityTypeBuilder<FaqSection> builder)
        {
            builder.ToTable("faq_section");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.IconUrl)
                .HasColumnName("IconUrl")
                .HasMaxLength(64)
                .HasColumnType("varchar(64)")
                .IsRequired(false);

            builder.Property(x => x.FaqId)
                .HasColumnName("faq_id")
                .HasColumnType("binary(16)")
                .IsRequired();
        }
    }
}

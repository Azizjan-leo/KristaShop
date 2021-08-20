using System;
using System.Collections.Generic;
using System.Text;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations
{
    public class FaqConfiguration : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.ToTable("faq");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();
            
            builder.Property(x => x.ColorCode)
                .HasColumnName("ColorCode")
                .HasMaxLength(16)
                .HasColumnType("varchar(16)")
                .IsRequired(false);

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}

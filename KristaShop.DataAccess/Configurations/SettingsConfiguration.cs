using System;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings> {
        public void Configure(EntityTypeBuilder<Settings> builder) {
            builder.ToTable("dict_settings");
            
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Key).IsUnique();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Key)
                .HasColumnName("key")
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.Value)
                .HasColumnName("value")
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.OnlyRootAccess)
                .HasColumnName("only_root_access")
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}

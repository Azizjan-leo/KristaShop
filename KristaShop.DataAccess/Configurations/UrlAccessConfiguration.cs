using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class UrlAccessConfiguration : IEntityTypeConfiguration<UrlAccess> {
        public void Configure(EntityTypeBuilder<UrlAccess> builder) {
            builder.ToTable("url_access");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Url)
                .HasColumnName("url")
                .HasMaxLength(200)
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(1024)
                .HasColumnType("varchar(1024)");

            builder.Property(x => x.ForManager)
                .HasColumnName("for_manager")
                .IsRequired();

            builder.Property(x => x.ForRootOnly)
                .HasColumnName("for_root")
                .IsRequired();
        }
    }
}
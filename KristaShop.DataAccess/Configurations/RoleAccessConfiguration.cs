using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class RoleAccessConfiguration : IEntityTypeConfiguration<RoleAccess> {
        public void Configure(EntityTypeBuilder<RoleAccess> builder) {
            builder.ToTable("role_accesses");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.Accesses)
                .HasForeignKey(x => x.RoleId);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Area)
                .HasColumnName("area")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Controller)
                .HasColumnName("controller")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Action)
                .HasColumnName("action")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(521)
                .IsRequired();

            builder.Property(x => x.IsAccessGranted)
                .HasColumnName("is_access_granted")
                .IsRequired();
        }
    }
}

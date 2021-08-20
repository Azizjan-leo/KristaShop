using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class ManagerAccessConfiguration : IEntityTypeConfiguration<ManagerAccess> {
        public void Configure(EntityTypeBuilder<ManagerAccess> builder) {
            builder.ToTable("ext1c_managers_accesses");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ManagerDetails)
                .WithMany(x => x.Accesses)
                .HasForeignKey(x => x.ManagerId);

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id")
                .IsRequired();

            builder.Property(x => x.AccessToManagerId)
                .HasColumnName("access_to_manager_id")
                .IsRequired();

            builder.Property(x => x.AccessTo)
                .HasColumnName("access_to")
                .IsRequired();
        }
    }
}

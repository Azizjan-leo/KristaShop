using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager> {
        public void Configure(EntityTypeBuilder<Manager> builder) {
            builder.ToTable("1c_manager", t => t.ExcludeFromMigrations());

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Details)
                .WithOne(x => x.Manager)
                .HasForeignKey<Manager>(x => x.Id)
                .ExcludeForeignKeyFromMigration();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.DepartmentId)
                .HasColumnName("otdel");

            builder.Property(x => x.Name)
                .HasColumnName("name");
        }
    }
}
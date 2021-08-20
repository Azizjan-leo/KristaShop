using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class StorehouseConfiguration : IEntityTypeConfiguration<Storehouse> {
        public void Configure(EntityTypeBuilder<Storehouse> builder) {
            builder.ToView("1c_sklady");
            builder.ToTable("Storehouse", t => t.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.Property(x => x.IsCollective)
                .HasColumnName("obsch");

            builder.Property(x => x.Priority)
                .HasColumnName("mesto");
        }
    }
}
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class MaterialConfiguration : IEntityTypeConfiguration<Material> {
        public void Configure(EntityTypeBuilder<Material> builder) {
            builder.ToTable("1c_material", t => t.ExcludeFromMigrations());

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();
        }
    }
}
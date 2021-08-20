using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ColorGroupConfiguration : IEntityTypeConfiguration<ColorGroup> {
        public void Configure(EntityTypeBuilder<ColorGroup> builder) {
            builder.ToView(null);
            builder.ToTable("1c_colors_group", t => t.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.Property(v => v.Id)
                .HasColumnName("id");
            
            builder.Property(v => v.Name)
                .HasColumnName("name");
            
            builder.Property(v => v.Hex)
                .HasColumnName("rgb");
        }
    }
}
using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ColorsConfiguration : IEntityTypeConfiguration<Color> {
        public void Configure(EntityTypeBuilder<Color> builder) {
            builder.ToView(null);
            builder.ToTable("1c_colors", t => t.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Group)
                .WithMany()
                .HasForeignKey(x => x.GroupId)
                .ExcludeForeignKeyFromMigration();
            
            builder.Property(v => v.Id)
                .HasColumnName("id");
            
            builder.Property(v => v.Name)
                .HasColumnName("name");
            
            builder.Property(v => v.GroupId)
                .HasColumnName("group");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class Category1CConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.Category1C> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.Category1C> builder) {
            builder.ToTable("1c_razdelycataloga", t => t.ExcludeFromMigrations());
            builder.ToView(null);
            
            builder.HasNoKey();

            builder.Property(v => v.Id)
                .HasColumnName("id");
            
            builder.Property(v => v.Name)
                .HasColumnName("razdel");
        }
    }
}

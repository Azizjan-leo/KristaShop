using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class CollectionConfiguration : IEntityTypeConfiguration<Collection> {
        public void Configure(EntityTypeBuilder<Collection> builder) {
            builder.ToView(null);
            builder.ToTable("1c_collection", t => t.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.Property(x => x.CreateDate)
                .HasColumnName("dates");

            builder.Property(x => x.Date)
                .HasColumnName("datev");

            builder.Property(x => x.PercentValue)
                .HasColumnName("procent");

            builder.Ignore(x => x.Percent);
        }
    }
}
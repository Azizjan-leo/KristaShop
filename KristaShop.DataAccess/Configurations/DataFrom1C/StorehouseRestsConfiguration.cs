using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class StorehouseRestsConfiguration : IEntityTypeConfiguration<StorehouseRests> {
        public void Configure(EntityTypeBuilder<StorehouseRests> builder) {
            builder.ToTable("1c_sklad", t => t.ExcludeFromMigrations());

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.ModelId)
                .HasColumnName("model");

            builder.Property(x => x.NomenclatureId)
                .HasColumnName("nomenklatura");

            builder.Property(x => x.ColorId)
                .HasColumnName("color");

            builder.Property(x => x.Amount)
                .HasColumnName("kolichestvo");

            builder.Property(x => x.StorehouseId)
                .HasColumnName("sklad");

            builder.Property(x => x.StorehouseName)
                .HasColumnName("name");

            builder.Property(x => x.StorehouseIsCollective)
                .HasColumnName("obsch");

            builder.Property(x => x.StorehousePriority)
                .HasColumnName("mesto");

            builder.Property(x => x.IsLine)
                .HasColumnName("line");
        }
    }
}
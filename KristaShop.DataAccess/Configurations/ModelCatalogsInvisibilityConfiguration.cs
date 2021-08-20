using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class ModelCatalogsInvisibilityConfiguration : IEntityTypeConfiguration<ModelCatalogsInvisibility> {
        public void Configure(EntityTypeBuilder<ModelCatalogsInvisibility> builder) {
            builder.ToTable("model_catalogs_invisibility");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int(8)")
                .IsRequired();

            builder.Property(x => x.Articul)
                .HasColumnName("articul")
                .HasColumnType("varchar(64)")
                .IsRequired();

            builder.Property(x => x.CatalogId)
                .HasColumnName("catalog_id")
                .HasColumnType("int(8)")
                .IsRequired();
        }
    }
}
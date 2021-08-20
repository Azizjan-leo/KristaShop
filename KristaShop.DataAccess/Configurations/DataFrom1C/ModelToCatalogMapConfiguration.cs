using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ModelToCatalogMapConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.ModelToCatalog1CMap> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.ModelToCatalog1CMap> builder) {
            builder.ToView("1c_model_to_catalog");
            builder.ToTable("ModelToCatalogMap", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.Articul)
                .HasColumnName("articul");
            builder
                .Property(v => v.CatalogId)
                .HasColumnName("razdel_id");
        }
    }
}

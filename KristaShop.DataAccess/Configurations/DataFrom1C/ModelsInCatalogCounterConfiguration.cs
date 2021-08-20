using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ModelsInCatalogCounterConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.ModelsInCatalogCounter> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.ModelsInCatalogCounter> builder) {
            builder.ToView("1c_models_in_catalog_counter");
            builder.ToTable("ModelsInCatalogCounter", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.CatalogId)
                .HasColumnName("catalog_id");
            builder
                .Property(v => v.Count)
                .HasColumnName("models_count");
        }
    }
}

using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ModelToCategotyMapConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.ModelToCategory1CMap> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.ModelToCategory1CMap> builder) {
            builder.ToView("1c_model_to_category");
            builder.ToTable("ModelToCategotyMap", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.Articul)
                .HasColumnName("articul");
            builder
                .Property(v => v.CategoryId)
                .HasColumnName("category_id");
            builder
                .Property(v => v.CategoryName)
                .HasColumnName("category_name");

        }
    }
}

using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ModelParamsConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.ModelParamsSqlView> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.ModelParamsSqlView> builder) {
            builder.ToView("1c_models_params");
            builder.ToTable("ModelParams", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.ColorId)
                .HasColumnName("color_id");
            builder
                .Property(v => v.ColorName)
                .HasColumnName("color_name");
            builder
                .Property(v => v.NomenclatureId)
                .HasColumnName("nomenclature_id");
            builder
                .Property(v => v.Size)
                .HasColumnName("size");
            builder
                .Property(v => v.SizeLine)
                .HasColumnName("size_line");
            builder
                .Property(v => v.ModelName)
                .HasColumnName("model_name");
        }
    }
}

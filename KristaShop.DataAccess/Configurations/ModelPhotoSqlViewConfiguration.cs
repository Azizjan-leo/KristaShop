using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class ModelPhotoSqlViewConfiguration : IEntityTypeConfiguration<ModelPhotoSqlView> {
        public void Configure(EntityTypeBuilder<ModelPhotoSqlView> builder) {
            builder.ToView("model_photos_sql_view");
            builder.ToTable("ModelPhotoSqlView", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.Articul)
                .HasColumnName("articul");

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.PhotoPath)
                .HasColumnName("photo_path");

            builder.Property(x => x.OldPhotoPath)
                .HasColumnName("old_photo_path");

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id");

            builder.Property(x => x.ColorName)
                .HasColumnName("color_name");

            builder.Property(x => x.Order)
                .HasColumnName("order");
        }
    }
}

using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class ModelPhoto1CConfiguration : IEntityTypeConfiguration<ModelPhoto1C> {
        public void Configure(EntityTypeBuilder<ModelPhoto1C> builder) {
            builder.ToTable("model_photos_1c");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.Articul, x.Order });

            builder.HasOne(x => x.Color)
                .WithMany()
                .HasForeignKey(x => x.ColorId)
                .OnDelete(DeleteBehavior.Restrict)
                .ExcludeForeignKeyFromMigration();
            
            builder.Property(x => x.Articul)
                .HasColumnName("articul")
                .HasColumnType("varchar(64)")
                .IsRequired();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int(8)")
                .IsRequired();

            builder.Property(x => x.PhotoPath)
                .HasColumnName("photo_path")
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder.Property(x => x.OldPhotoPath)
                .HasColumnName("old_photo_path")
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id")
                .HasColumnType("int(8)");

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .HasColumnType("int(6)")
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}

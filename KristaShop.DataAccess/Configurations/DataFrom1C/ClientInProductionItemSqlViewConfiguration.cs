using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ClientInProductionItemSqlViewConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.ClientInProductionItemSqlView> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.ClientInProductionItemSqlView> builder) {
            builder.ToView("1c_client_in_production_item");
            builder.ToTable("ClientInProductionItemSqlView", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.Id)
                .HasColumnName("id");
            builder
                .Property(v => v.UserId)
                .HasColumnName("user_id");
            builder
                .Property(v => v.Articul)
                .HasColumnName("articul");
            builder
                .Property(v => v.ModelId)
                .HasColumnName("model_id");
            builder
                .Property(v => v.ColorId)
                .HasColumnName("color_id");
            builder
                .Property(v => v.ColorName)
                .HasColumnName("color_name");
            builder
                .Property(v => v.ColorPhoto)
                .HasColumnName("color_photo");
            builder
                .Property(v => v.ColorValue)
                .HasColumnName("color_group_rgb_value");
            builder
                .Property(v => v.SizeValue)
                .HasColumnName("size_value");
            builder
                .Property(v => v.PartsCount)
                .HasColumnName("parts_count");
            builder
                .Property(v => v.MainPhoto)
                .HasColumnName("main_photo");
            builder
                .Property(v => v.PhotoByColor)
                .HasColumnName("photo_by_color");
            builder
                .Property(v => v.InStageKroy)
                .HasColumnName("kroitsya");
            builder
                .Property(v => v.InStageKroyDone)
                .HasColumnName("gotovkroy");
            builder
                .Property(v => v.InStageZapusk)
                .HasColumnName("zapusk");
            builder
                .Property(v => v.InStageVposhive)
                .HasColumnName("vposhive");
            builder
                .Property(v => v.InStageSkladGP)
                .HasColumnName("skladgp");
        }
    }
}

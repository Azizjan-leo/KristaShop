using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class Favorite1CItemConfiguration : IEntityTypeConfiguration<Favorite1CItemItem> {
        public void Configure(EntityTypeBuilder<Favorite1CItemItem> builder) {
            builder.ToTable("favorite_items_1c");

            builder.HasKey(x => new { x.UserId, x.Articul, x.CatalogId });

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
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
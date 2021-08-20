using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem> {
        public void Configure(EntityTypeBuilder<CartItem> builder) {
            builder.ToTable("cart_items_1c");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Model)
                .WithMany()
                .HasForeignKey(x => x.ModelId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Color)
                .WithMany()
                .HasForeignKey(x => x.ColorId)
                .ExcludeForeignKeyFromMigration();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int(8)")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .HasColumnType("int(8)")
                .IsRequired();

            builder.Property(x => x.CatalogId)
                .HasColumnName("catalog_id")
                .HasColumnType("int(8)")
                .IsRequired();

            builder.Property(x => x.Articul)
                .HasColumnName("articul")
                .HasColumnType("varchar(64)")
                .IsRequired();

            builder.Property(x => x.ModelId)
                .HasColumnName("model_id")
                .HasColumnType("int(8)")
                .IsRequired();

            builder.Property(x => x.NomenclatureId)
                .HasColumnName("nomenclature_id")
                .HasColumnType("int(8)");

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id")
                .HasColumnType("int(8)")
                .IsRequired();

            builder.Property(x => x.SizeValue)
                .HasColumnName("size_value")
                .HasColumnType("varchar(32)")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .HasColumnType("double")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .HasColumnType("double")
                .IsRequired();

            builder.Property(x => x.PriceInRub)
                .HasColumnName("price_rub")
                .HasColumnType("double")
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .HasColumnType("int(6)")
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasColumnName("created_date")
                .HasColumnType("datetime")
                .IsRequired();

        }
    }
}
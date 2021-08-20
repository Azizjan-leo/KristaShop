using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails> {
        public void Configure(EntityTypeBuilder<OrderDetails> builder) {
            builder.ToTable("for1c_order_details");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Details)
                .HasForeignKey(x => x.OrderId);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(x => x.CatalogId)
                .HasColumnName("catalog_id")
                .IsRequired();

            builder.Property(x => x.NomenclatureId)
                .HasColumnName("nomenclature_id")
                .IsRequired();

            builder.Property(x => x.ModelId)
                .HasColumnName("model_id")
                .IsRequired();

            builder.Property(x => x.SizeValue)
                .HasColumnName("size_value")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)")
                .IsRequired();

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id")
                .IsRequired();

            builder.Property(x => x.StorehouseId)
                .HasColumnName("storehouse_id")
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .IsRequired();

            
            builder.Property(x => x.PriceInRub)
                .HasColumnName("price_in_rub")
                .IsRequired();

            builder.Property(x => x.CollectionId)
                .HasColumnName("collection_id");

            builder.Ignore(x => x.Articul);
            builder.Ignore(x => x.ColorName);
        }
    }
}
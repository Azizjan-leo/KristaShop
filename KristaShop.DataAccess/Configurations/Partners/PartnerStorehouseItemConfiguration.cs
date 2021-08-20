using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class PartnerStorehouseItemConfiguration : IEntityTypeConfiguration<PartnerStorehouseItem> {
        public void Configure(EntityTypeBuilder<PartnerStorehouseItem> builder) {
            builder.ToTable("part_storehouse_items");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.Articul)
                .HasColumnName("articul")
                .HasMaxLength(255)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(x => x.ModelId)
                .HasColumnName("model_id")
                .IsRequired();

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id")
                .IsRequired();

            builder.Property(x => x.SizeValue)
                .HasColumnName("size_value")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)")
                .IsRequired();

            builder.Ignore(x => x.Size);

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(x => x.PriceInRub)
                .HasColumnName("price_rub")
                .IsRequired();

            builder.Property(x => x.OrderType)
                .HasColumnName("order_type")
                .IsRequired();

            builder.Property(x => x.IncomeDate)
                .HasColumnName("income_date")
                .IsRequired();

            builder.Ignore(x => x.Barcodes);
        }
    }
}
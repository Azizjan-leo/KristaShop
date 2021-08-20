using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class PartnerStorehouseHistoryItemConfiguration : IEntityTypeConfiguration<PartnerStorehouseHistoryItem> {
        public void Configure(EntityTypeBuilder<PartnerStorehouseHistoryItem> builder) {
            builder.ToTable("part_storehouse_history_items");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.MovementType)
                .HasColumnName("movement_type")
                .IsRequired();

            builder.Property(x => x.Direction)
                .HasColumnName("movement_direction")
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

            builder.Property(x => x.Size)
                .HasColumnName("size_value")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)")
                .HasConversion(ValueConverters.SizeConverter)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(x => x.PriceInRub)
                .HasColumnName("price_rub")
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .IsRequired();
        }
    }
}

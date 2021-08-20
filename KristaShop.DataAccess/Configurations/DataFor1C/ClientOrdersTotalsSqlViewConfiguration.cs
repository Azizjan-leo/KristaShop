using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class ClientOrdersTotalsSqlViewConfiguration : IEntityTypeConfiguration<ClientOrdersTotalsSqlView> {
        public void Configure(EntityTypeBuilder<ClientOrdersTotalsSqlView> builder) {
            builder.HasNoKey();
            builder.ToView(null);
            builder.ToTable("ClientOrdersTotalsSqlView", t => t.ExcludeFromMigrations());

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.UserFullName)
                .HasColumnName("user_fullname");

            builder.Property(x => x.UserHasCartItems)
                .HasColumnName("has_cart_items");

            builder.Property(x => x.CityId)
                .HasColumnName("city_id");

            builder.Property(x => x.CityName)
                .HasColumnName("city_name");

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id");

            builder.Property(x => x.ManagerName)
                .HasColumnName("manager_name");

            builder.Property(x => x.PreorderTotalAmount)
                .HasColumnName("preorder_amount");

            builder.Property(x => x.InStockTotalAmount)
                .HasColumnName("instock_amount");

            builder.Property(x => x.PreorderTotalSum)
                .HasColumnName("preorder_total_sum");

            builder.Property(x => x.PreorderTotalSumInRub)
                .HasColumnName("preorder_total_sum_rub");

            builder.Property(x => x.InStockTotalSum)
                .HasColumnName("instock_total_sum");

            builder.Property(x => x.InStockTotalSumInRub)
                .HasColumnName("instock_total_sum_rub");

            builder.Ignore(x => x.TotalAmount);
            builder.Ignore(x => x.TotalSum);
            builder.Ignore(x => x.TotalSumInRub);
        }
    }
}
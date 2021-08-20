using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class OrderStatsConfiguration : IEntityTypeConfiguration<OrderStats> {
        public void Configure(EntityTypeBuilder<OrderStats> builder) {
            builder.HasNoKey();
            builder.ToView("for1c_orders_admin_stats");
            builder.ToTable("OrderStats", t => t.ExcludeFromMigrations());

            builder.Property(x => x.OrdersCount)
                .HasColumnName("orders_count");

            builder.Property(x => x.TotAmountPreorder)
                .HasColumnName("tot_amount_preorder");

            builder.Property(x => x.TotSumPreorder)
                .HasColumnName("tot_sum_preorder");

            builder.Property(x => x.TotAmountRetail)
                .HasColumnName("tot_amount_retail");

            builder.Property(x => x.TotSumRetail)
                .HasColumnName("tot_sum_retail");

        }
    }
}
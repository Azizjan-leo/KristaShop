using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Views {
    public class OrderTotalsSqlViewConfiguration : IEntityTypeConfiguration<OrderTotalsSqlView> {
        public void Configure(EntityTypeBuilder<OrderTotalsSqlView> builder) {
            builder.ToView(null);
            builder.ToTable("OrderTotalsSqlView", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.Sum)
                .HasColumnName("sum");

            builder.Property(x => x.SumInRub)
                .HasColumnName("sum_rub");

            builder.Property(x => x.Type)
                .HasColumnName("type");

            builder.Property(x => x.PrepayPercent)
                .HasColumnName("prepay_percent");
        }
    }
}

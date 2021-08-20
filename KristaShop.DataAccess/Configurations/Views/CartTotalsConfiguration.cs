using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Views {
    public class CartTotalsConfiguration : IEntityTypeConfiguration<ReportTotals> {
        public void Configure(EntityTypeBuilder<ReportTotals> builder) {
            builder.ToView(null);
            builder.ToTable("ReportTotals", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.TotalAmount)
                .HasColumnName("total_amount");

            builder.Property(x => x.TotalPrice)
                .HasColumnName("total_price");

            builder.Property(x => x.TotalPriceInRub)
                .HasColumnName("total_price_rub");

            builder.Property(x => x.CatalogId)
                .HasColumnName("catalog_id");
        }
    }
}
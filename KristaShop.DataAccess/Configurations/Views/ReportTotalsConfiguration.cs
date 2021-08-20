using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Views {
    public class ReportTotalsConfiguration : IEntityTypeConfiguration<UserCartTotals> {
        public void Configure(EntityTypeBuilder<UserCartTotals> builder) {
            builder.ToView(null);
            builder.ToTable("UserCartTotals", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.UserFullName)
                .HasColumnName("fullname");

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id");

            builder.Property(x => x.ManagerName)
                .HasColumnName("manager_name");

            builder.Property(x => x.CityId)
                .HasColumnName("city_id");

            builder.Property(x => x.CityName)
                .HasColumnName("city_name");

            builder.Property(x => x.TotalItemsCount)
                .HasColumnName("total_items_count");

            builder.Property(x => x.TotalPrice)
                .HasColumnName("total_price");

            builder.Property(x => x.TotalPriceRub)
                .HasColumnName("total_price_rub");
        }
    }
}
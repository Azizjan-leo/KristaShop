using KristaShop.DataAccess.Views.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Views {
    public class PartnersSqlViewConfiguration : IEntityTypeConfiguration<PartnerSqlView> {
        public void Configure(EntityTypeBuilder<PartnerSqlView> builder) {
            builder.ToView(null);
            builder.ToTable("PartnerSqlView", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.FullName)
                .HasColumnName("fullname");

            builder.Property(x => x.ManagerName)
                .HasColumnName("manager_name");

            builder.Property(x => x.CityName)
                .HasColumnName("city_name");

            builder.Property(x => x.Email)
                .HasColumnName("email");

            builder.Property(x => x.Phone)
                .HasColumnName("phone");

            builder.Property(x => x.MallAddress)
                .HasColumnName("addresstc");

            builder.Property(x => x.ShipmentsItemsCount)
                .HasColumnName("shipments_items_count");

            builder.Property(x => x.ShipmentsItemsSum)
                .HasColumnName("shipments_items_sum");

            builder.Property(x => x.StorehouseItemsCount)
                .HasColumnName("storehouse_items_count");

            builder.Property(x => x.DebtItemsCount)
                .HasColumnName("debt_items_count");

            builder.Property(x => x.DebtItemsSum)
                .HasColumnName("debt_items_sum");

            builder.Property(x => x.RevisionDate)
                .HasColumnName("revision_date");

            builder.Property(x => x.PaymentDate)
                .HasColumnName("payment_date");
        }
    }
}

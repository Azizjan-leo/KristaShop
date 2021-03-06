using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ShipingAdminConfiguration : IEntityTypeConfiguration<ShipingAdmin> {
        public void Configure(EntityTypeBuilder<ShipingAdmin> builder) {
            builder.ToView("shiping_admin");
            builder.ToTable("ShipingAdmin", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.UserFullName)
                .HasColumnName("user_name");

            builder.Property(x => x.CityName)
                .HasColumnName("city_name");

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id");

            builder.Property(x => x.ManagerFullName)
                .HasColumnName("manager_name");

            builder.Property(x => x.TotAmount)
                .HasColumnName("tot_amount");

            builder.Property(x => x.TotPrice)
                .HasColumnName("tot_price");

            builder.Property(x => x.TotPriceInRub)
                .HasColumnName("tot_price_rub");
        }
    }
}
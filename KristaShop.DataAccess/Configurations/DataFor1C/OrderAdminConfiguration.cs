using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class OrderAdminConfiguration : IEntityTypeConfiguration<OrderAdmin> {
        public void Configure(EntityTypeBuilder<OrderAdmin> builder) {
            builder.HasNoKey();
            builder.ToView("for1c_orders_admin");
            builder.ToTable("OrderAdmin", t => t.ExcludeFromMigrations());

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.CreateDate)
                .HasColumnName("create_date");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.UserFullName)
                .HasColumnName("client_name");

            builder.Property(x => x.UserHasCartItems)
                .HasColumnName("has_cart_items");

            builder.Property(x => x.CityId)
                .HasColumnName("city_id");

            builder.Property(x => x.CityName)
                .HasColumnName("city_name");

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id");

            builder.Property(x => x.ManagerFullName)
                .HasColumnName("manager_name");

            builder.Property(x => x.IsProcessedPreorder)
                .HasColumnName("is_processed_preorder");

            builder.Property(x => x.IsProcessedRetail)
                .HasColumnName("is_processed_retail");

            builder.Property(x => x.PreorderTotalSum)
                .HasColumnName("preorder_total_sum");

            builder.Property(x => x.PreorderTotalSumInRub)
                .HasColumnName("preorder_total_sum_rub");

            builder.Property(x => x.RetailTotalSum)
                .HasColumnName("retail_total_sum");

            builder.Property(x => x.RetailTotalSumInRub)
                .HasColumnName("retail_total_sum_rub");

            builder.Property(x => x.PreorderAmount)  
                .HasColumnName("preorder_amount");

            builder.Property(x => x.RetailAmount)
                .HasColumnName("retail_amount");

            builder.Property(x => x.HasExtraPack)
                .HasColumnName("has_extra_pack");

            builder.Property(x => x.UserComments)
                .HasColumnName("description");

            builder.Property(x => x.IsReviewed)
                .HasColumnName("is_reviewed");

            builder.Ignore(x => x.TotalSum);
            builder.Ignore(x => x.TotalSumInRub);
            builder.Ignore(x => x.IsUnprocessed);
        }
    }
}
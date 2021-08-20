using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class OrderConfiguration : IEntityTypeConfiguration<Order> {
        public void Configure(EntityTypeBuilder<Order> builder) {
            builder.ToTable("for1c_orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.UserLogin)
                .HasColumnName("user_login")
                .HasMaxLength(255)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(x => x.HasExtraPack)
                .HasColumnName("has_extra_pack")
                .IsRequired();

            builder.Property(x => x.IsProcessedPreorder)
                .HasColumnName("is_processed_preorder")
                .IsRequired();

            builder.Property(x => x.IsProcessedRetail)
                .HasColumnName("is_processed_retail")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2048)
                .HasColumnType("varchar(2048)")
                .HasDefaultValue("")
                .IsRequired();

            builder.Property(x => x.IsReviewed)
                .HasColumnName("is_reviewed")
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}
using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class ManagerDetailsSqlViewConfiguration : IEntityTypeConfiguration<ManagerDetailsSqlView> {
        public void Configure(EntityTypeBuilder<ManagerDetailsSqlView> builder) {
            builder.ToTable("ManagerDetailsSqlView", t => t.ExcludeFromMigrations());
            builder.ToView(null);
            builder.HasNoKey();

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.Property(x => x.RegistrationsQueueNumber)
                .HasColumnName("registration_queue_number");

            builder.Property(x => x.NotificationsEmail)
                .HasColumnName("notifications_email");

            builder.Property(x => x.SendNewOrderNotification)
                .HasColumnName("send_new_order_notification");

            builder.Property(x => x.SendNewRegistrationsNotification)
                .HasColumnName("send_new_registrations_notification");

            builder.Property(x => x.RoleId)
              .HasColumnName("role_id")
              .HasColumnType("binary(16)");
        }
    }
}
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KristaShop.Common.Extensions;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class ManagerDetailsConfiguration : IEntityTypeConfiguration<ManagerDetails> {
        public void Configure(EntityTypeBuilder<ManagerDetails> builder) {
            builder.ToTable("ext1c_managers_details");

            builder.HasKey(x => x.ManagerId);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.ManagerDetails)
                .HasForeignKey(x => x.RoleId);

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id")
                .IsRequired();

            builder.Property(x => x.RegistrationsQueueNumber)
                .HasColumnName("registration_queue_number")
                .IsRequired();

            builder.Property(x => x.NotificationsEmail)
                .HasColumnName("notifications_email")
                .HasMaxLength(256)
                .IsRequired(false);

            builder.Property(x => x.SendNewOrderNotification)
                .HasColumnName("send_new_order_notification")
                .IsRequired();

            builder.Property(x => x.SendNewRegistrationsNotification)
                .HasColumnName("send_new_registrations_notification")
                .IsRequired();

            builder.Property(x => x.RoleId)
               .HasColumnName("role_id")
               .HasColumnType("binary(16)")
               .IsRequired();
        }
    }
}

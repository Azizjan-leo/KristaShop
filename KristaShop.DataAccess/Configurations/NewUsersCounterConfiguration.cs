using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class NewUsersCounterConfiguration : IEntityTypeConfiguration<NewUsersCounter> {
        public void Configure(EntityTypeBuilder<NewUsersCounter> builder) {
            builder.ToTable("new_users_counter");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Counter)
                .HasColumnName("counter")
                .IsRequired();

            builder.Property(x => x.UpdateTimestamp)
                .HasColumnName("update_time_stamp")
                .IsRequired(false);
        }
    }
}

using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class UserNewPasswordConfiguration : IEntityTypeConfiguration<UserNewPassword> {
        public void Configure(EntityTypeBuilder<UserNewPassword> builder) {
            builder.ToTable("for1c_user_new_passwords");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.PasswordSrc)
                .HasColumnName("password_src")
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
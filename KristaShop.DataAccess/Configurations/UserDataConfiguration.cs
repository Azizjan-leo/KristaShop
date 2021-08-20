using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class UserDataConfiguration : IEntityTypeConfiguration<UserData> {
        public void Configure(EntityTypeBuilder<UserData> builder) {
            builder.ToTable("user_data");

            builder.HasKey(x => x.UserId);

            builder.HasOne(x => x.User)
                .WithOne(x => x.Data)
                .HasForeignKey<UserData>(x => x.UserId)
                .ExcludeForeignKeyFromMigration();
            
            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.LastSignIn)
                .HasColumnName("last_sign_in")
                .IsRequired();
        }
    }
}
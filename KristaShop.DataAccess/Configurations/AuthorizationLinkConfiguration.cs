using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class AuthorizationLinkConfiguration : IEntityTypeConfiguration<AuthorizationLink> {
        public void Configure(EntityTypeBuilder<AuthorizationLink> builder) {
            builder.ToTable("authorization_link");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.Code)
                .HasColumnName("random_code")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.RecordTimeStamp)
                .HasColumnName("record_time_stamp")
                .IsRequired();

            builder.Property(x => x.ValidTo)
                .HasColumnName("valid_to")
                .IsRequired(false);

            builder.Property(x => x.LoginDate)
                .HasColumnName("login_date")
                .IsRequired(false);

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .HasColumnType("INT(11)")
                .HasDefaultValueSql($"{(int)AuthorizationLinkType.MultipleAccess}")
                .IsRequired();

        }
    }
}

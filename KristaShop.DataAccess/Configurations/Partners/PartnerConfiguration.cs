using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class PartnerConfiguration : IEntityTypeConfiguration<Partner> {
        public void Configure(EntityTypeBuilder<Partner> builder) {
            builder.ToTable("partners");
            builder.HasKey(x => x.UserId);

            builder.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId)
             .ExcludeForeignKeyFromMigration();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.DateApproved)
                .HasColumnName("date_approved")
                .IsRequired();

            builder.Property(x => x.PaymentRate)
                .HasColumnName("payment_rate")
                .IsRequired();
        }
    }
}
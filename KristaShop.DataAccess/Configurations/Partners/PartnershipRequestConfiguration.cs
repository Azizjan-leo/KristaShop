using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class PartnershipRequestConfiguration : IEntityTypeConfiguration<PartnershipRequest> {
        public void Configure(EntityTypeBuilder<PartnershipRequest> builder) {
            builder.ToTable("part_partnership_requests");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserId).IsUnique();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.RequestedDate)
                .HasColumnName("requested_date")
                .IsRequired();

            builder.Property(x => x.IsAcceptedToProcess)
             .HasColumnName("is_accepted_to_process");

            builder.Property(x => x.IsConfirmed)
                .HasColumnName("is_confirmed");

            builder.Property(x => x.AnsweredDate)
                .HasColumnName("answered_date");
        }
    }
}

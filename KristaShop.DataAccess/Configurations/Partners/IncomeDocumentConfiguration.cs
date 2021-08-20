using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class IncomeDocumentConfiguration : IEntityTypeConfiguration<IncomeDocument> {
        public void Configure(EntityTypeBuilder<IncomeDocument> builder) {
            builder.Property(x => x.ShipmentDate)
                .HasColumnName("execution_date")
                .IsRequired();
        }
    }
}
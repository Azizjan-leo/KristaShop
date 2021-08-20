using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class PaymentDocumentConfiguration : IEntityTypeConfiguration<PaymentDocument> {
        public void Configure(EntityTypeBuilder<PaymentDocument> builder) {

        }
    }
}
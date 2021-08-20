using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class SellingDocumentConfiguration : IEntityTypeConfiguration<SellingDocument> {
        public void Configure(EntityTypeBuilder<SellingDocument> builder) {
            
        }
    }
}
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class SellingRequestDocumentConfiguration : IEntityTypeConfiguration<SellingRequestDocument> {
        public void Configure(EntityTypeBuilder<SellingRequestDocument> builder) {
            
        }
    }
}
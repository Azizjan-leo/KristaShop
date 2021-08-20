using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class RevisionDocumentConfiguration : IEntityTypeConfiguration<RevisionDocument> {
        public void Configure(EntityTypeBuilder<RevisionDocument> builder) {
            
        }
    }
}
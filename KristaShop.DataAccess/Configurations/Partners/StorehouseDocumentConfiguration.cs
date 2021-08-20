using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class StorehouseDocumentConfiguration : IEntityTypeConfiguration<StorehouseDocument> {
        public void Configure(EntityTypeBuilder<StorehouseDocument> builder) { }
    }
}
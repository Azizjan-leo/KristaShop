using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class MoneyDocumentConfiguration : IEntityTypeConfiguration<MoneyDocument> {
        public void Configure(EntityTypeBuilder<MoneyDocument> builder) {
            builder.Property(x => x.Sum)
                .HasColumnName("sum")
                .IsRequired();
        }
    }
}
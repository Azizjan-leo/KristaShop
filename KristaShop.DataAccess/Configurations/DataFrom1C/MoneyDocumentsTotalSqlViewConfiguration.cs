using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class MoneyDocumentsTotalSqlViewConfiguration : IEntityTypeConfiguration<MoneyDocumentsTotalSqlView> {
        public void Configure(EntityTypeBuilder<MoneyDocumentsTotalSqlView> builder) {
            builder.ToView(null);
            builder.ToTable("MoneyDocumentsTotalSqlView",  t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.UserId)
                .HasColumnName("klient");

            builder.Property(x => x.InitialBalance)
                .HasColumnName("nach_ost");

            builder.Property(x => x.FinalBalance)
                .HasColumnName("kon_ost");

            builder.Property(x => x.ToPay)
                .HasColumnName("dolg_plus");

            builder.Property(x => x.Income)
                .HasColumnName("dolg_minus");

            builder.Property(x => x.InitialDate)
                .HasColumnName("nach_data");
            
            builder.Property(x => x.FinalDate)
                .HasColumnName("kon_data");
        }
    }
}
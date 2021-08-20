using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class MoneyDocumentSqlViewConfiguration : IEntityTypeConfiguration<MoneyDocumentSqlView> {
        public void Configure(EntityTypeBuilder<MoneyDocumentSqlView> builder) {
            builder.ToView(null);
            builder.ToTable("MoneyDocumentSqlView",  t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Number)
                .HasColumnName("nomer_doc");

            builder.Property(x => x.Name)
                .HasColumnName("name_doc")
                .HasConversion(ValueConverters.MoneyDocumentTypeConverter);
            
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

            builder.Property(x => x.CreateDate)
                .HasColumnName("data_doc");
        }
    }
}
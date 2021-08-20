using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class InvoiceSqlConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.InvoiceSql> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.InvoiceSql> builder) {
            builder.ToView("1c_invoice");
            builder.ToTable("InvoiceSql", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.Id)
                .HasColumnName("id");
            builder
                .Property(v => v.UserId)
                .HasColumnName("klient");
            builder
                .Property(v => v.UserFullName)
                .HasColumnName("user_name");
            builder
                .Property(v => v.CityName)
                .HasColumnName("city_name");
            builder
                .Property(v => v.ManagerId)
                .HasColumnName("manager_id");
            builder
                .Property(v => v.ManagerFullName)
                .HasColumnName("manager_name");
            builder
                .Property(v => v.InvoiceClientTitle)
                .HasColumnName("pklient");
            builder
                .Property(v => v.CreateDate)
                .HasColumnName("datadok");
            builder
                .Property(v => v.InvoiceNum)
                .HasColumnName("nomerdok");
            builder
                .Property(v => v.PrePay)
                .HasColumnName("ispavans");
            builder
                .Property(v => v.TotPay)
                .HasColumnName("koplate");
            builder
                .Property(v => v.Currency)
                .HasColumnName("val");
            builder
                .Property(v => v.ExchangeRate)
                .HasColumnName("kurs_rub");
            builder
                .Property(v => v.WasPayed)
                .HasColumnName("oplachen");
            builder
                .Property(v => v.IsPrepay)
                .HasColumnName("is_prepay");
        }
    }
}

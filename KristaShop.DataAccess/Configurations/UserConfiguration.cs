using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class UserConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.ToTable("1c_clients", t => t.ExcludeFromMigrations());
            builder.ToView(null);

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Manager)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ManagerId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .ExcludeForeignKeyFromMigration();

            builder.Property(x => x.Id)
                .HasColumnName("id");
            
            builder.Property(x => x.Name)
                .HasColumnName("name");
            
            builder.Property(x => x.FullName)
                .HasColumnName("fullname");
            
            builder.Property(x => x.Login)
                .HasColumnName("login");
            
            builder.Property(x => x.Password)
                .HasColumnName("password_md5");

            builder.Property(x => x.Phone)
                .HasColumnName("phone");
            
            builder.Property(x => x.Email)
                .HasColumnName("email");
            
            builder.Property(x => x.Address)
                .HasColumnName("address");
            
            builder.Property(x => x.MallAddress)
                .HasColumnName("addresstc");
            
            builder.Property(x => x.CityId)
                .HasColumnName("city");
            
            builder.Property(x => x.Balance)
                .HasColumnName("avansdolg");
            
            builder.Property(x => x.BalanceInRub)
                .HasColumnName("avansdolgrub");
            
            builder.Property(x => x.IsManager)
                .HasColumnName("imanager");
            
            builder.Property(x => x.ManagerId)
                .HasColumnName("manager");
            
            builder.Property(x => x.CreateDate)
                .HasColumnName("cdate")
                .HasConversion(ValueConverters.StringToDateConverter);

            builder.Property<bool>("_accessToInStockLinesCatalog")
                .HasColumnName("access2")
                .HasColumnType("tinyint(1)");
            
            builder.Property<bool>("_accessToInStockPartsCatalog")
                .HasColumnName("access3")
                .HasColumnType("tinyint(1)");
            
            builder.Property<bool>("_accessToPreorder")
                .HasColumnName("access4")
                .HasColumnType("tinyint(1)");

            builder.Property<bool>("_accessToRfInStockLinesCatalog")
              .HasColumnName("access5")
              .HasColumnType("tinyint(1)");

            builder.Property<bool>("_accessToRfInStockPartsCatalog")
                .HasColumnName("access6")
                .HasColumnType("tinyint(1)");
            
            builder.Property<bool>("_accessToSaleLinesCatalog")
                .HasColumnName("access7")
                .HasColumnType("tinyint(1)");
            
            builder.Property<bool>("_accessToSalePartsCatalog")
                .HasColumnName("access8")
                .HasColumnType("tinyint(1)");

            builder.Property<string>("_decryptedPassword")
                .HasColumnName("password")
                .HasColumnType("varchar(255)");
        }
    }
}
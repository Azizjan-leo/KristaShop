using System;
using System.Linq;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class UserSqlViewConfiguration : IEntityTypeConfiguration<UserSqlView> {
        public void Configure(EntityTypeBuilder<UserSqlView> builder) {
            builder.ToView("1c_clients");
            builder.ToTable("User", t => t.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.Property(x => x.FullName)
                .HasColumnName("fullname");

            builder.Property(x => x.Date)
                .HasColumnName("date");

            builder.Property(x => x.Login)
                .HasColumnName("login");

            builder.Property(x => x.Password)
                .HasColumnName("password_md5");

            builder.Property(x => x.Phone)
                .HasColumnName("phone");

            builder.Property(x => x.Address)
                .HasColumnName("address");

            builder.Property(x => x.Email)
                .HasColumnName("email");

            builder.Property(x => x.CityId)
                .HasColumnName("city");

            builder.Property(x => x.CityName)
                .HasColumnName("city_name");

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager");

            builder.Property(x => x.ManagerName)
                .HasColumnName("manager_name");

            builder.Property(x => x.MallAddress)
                .HasColumnName("addresstc");
            
            builder.Property(x => x.IsManager)
                .HasColumnName("imanager");
            
            builder.Property(x => x.IsPartner)
                .HasColumnName("is_partner");

            builder.Property(x => x.LastSignIn)
                .HasColumnName("last_sign_in");

            builder.Property(x => x.CatalogsAccess)
                .HasColumnName("catalogs_access")
                .HasConversion(x => string.Join(',', x), x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Metadata
                    .SetValueComparer(new ValueComparer<string[]>((c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), c => c.ToArray()));

            builder.Property(x => x.Balance)
                .HasColumnName("avansdolg");

            builder.Property(x => x.BalanceInRub)
                .HasColumnName("avansdolgrub");

            builder.Property(x => x.CreateDate)
                .HasColumnName("cdate");

        }
    }
}
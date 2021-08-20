using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class NewUserSqlViewConfiguration : IEntityTypeConfiguration<NewUserSqlView> {
        public void Configure(EntityTypeBuilder<NewUserSqlView> builder) {
            builder.ToTable("NewUserSqlView", t => t.ExcludeFromMigrations());
            builder.ToView(null);

            builder.HasNoKey();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)");

            builder.Property(x => x.FullName)
                .HasColumnName("fullname");

            builder.Property(x => x.CityId)
                .HasColumnName("city_id");

            builder.Property(x => x.CityName)
                .HasColumnName("city_name");
            
            builder.Property(x => x.NewCity)
                .HasColumnName("new_city");

            builder.Property(x => x.MallAddress)
                .HasColumnName("mall_address");

            builder.Property(x => x.Phone)
                .HasColumnName("phone");

            builder.Property(x => x.Email)
                .HasColumnName("email");

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id");

            builder.Property(x => x.ManagerName)
                .HasColumnName("manager_name");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.CreateDate)
                .HasColumnName("create_date");
        }
    }
}
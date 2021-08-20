using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFor1C {
    public class NewUserConfiguration : IEntityTypeConfiguration<NewUser> {
        public void Configure(EntityTypeBuilder<NewUser> builder) {
            builder.ToTable("for1c_new_users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.FullName)
                .HasColumnName("fullname")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.CityId)
                .HasColumnName("city_id")
                .IsRequired(false);

            builder.Property(x => x.NewCity)
                .HasColumnName("new_city")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)")
                .IsRequired(false);
            
            builder.Property(x => x.Password)
                .HasColumnName("Password")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)")
                .IsRequired(false);
            
            builder.Property(x => x.Login)
                .HasColumnName("Login")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)")
                .IsRequired(false);
            
            builder.Property(x => x.CompanyAddress)
                .HasColumnName("CompanyAddress")
                .HasMaxLength(256)
                .HasColumnType("varchar(256)")
                .IsRequired(false);

            builder.Property(x => x.MallAddress)
                .HasColumnName("mall_address")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(255);

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(255);

            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id")
                .IsRequired(false);

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired(false);

            builder.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .IsRequired(false);

        }
    }
}
using KristaShop.DataAccess.Views.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Views {
    public class PartnershipRequestSqlViewConfiguration : IEntityTypeConfiguration<PartnershipRequestSqlView> {
        public void Configure(EntityTypeBuilder<PartnershipRequestSqlView> builder) {
            builder.ToView(null);
            builder.ToTable("PartnershipRequestSqlView", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.Id)
                .HasColumnName("id"); 

            builder.Property(x => x.UserId)
                .HasColumnName("user_id"); 

            builder.Property(x => x.FullName)
                .HasColumnName("fullname"); 

            builder.Property(x => x.ManagerName)
                .HasColumnName("manager_name"); 

            builder.Property(x => x.CityName)
                .HasColumnName("city_name"); 

            builder.Property(x => x.Email)
                .HasColumnName("email"); 

            builder.Property(x => x.Phone)
                .HasColumnName("phone"); 

            builder.Property(x => x.MallAddress)
                .HasColumnName("addresstc"); 

            builder.Property(x => x.RequestedDate)
                .HasColumnName("requested_date"); 

            builder.Property(x => x.IsAcceptedToProcess)
                .HasColumnName("is_accepted_to_process"); 
                        
            builder.Property(x => x.IsConfirmed)
                .HasColumnName("is_confirmed"); 

            builder.Property(x => x.AnsweredDate)
                .HasColumnName("answered_date"); 

            builder.Property(x => x.LastSignIn)
                .HasColumnName("last_sign_in"); 
        }
    }
}

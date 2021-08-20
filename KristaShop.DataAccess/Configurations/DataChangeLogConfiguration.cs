using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class DataChangeLogConfiguration : IEntityTypeConfiguration<DataChangeLog> {
        public void Configure(EntityTypeBuilder<DataChangeLog> builder) {
            builder.ToTable("data_change_log");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.TableName)
                .HasColumnName("table_name")
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder.Property(x => x.EntityName)
                .HasColumnName("entity_name")
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired(false);

            builder.Property(x => x.Operation)
                .HasColumnName("operation")
                .HasColumnType("varchar(32)")
                .IsRequired();

            builder.Property(x => x.Timestamp)
                .HasColumnName("timestamp")
                .IsRequired();

            builder.Property(x => x.KeyValues)
                .HasColumnName("key_values")
                .HasColumnType("json")
                .IsRequired();

            builder.Property(x => x.OldValues)
                .HasColumnName("old_values")
                .HasColumnType("json")
                .IsRequired(false);

            builder.Property(x => x.NewValues)
                .HasColumnName("new_values")
                .HasColumnType("json")
                .IsRequired(false);
        }
    }
}
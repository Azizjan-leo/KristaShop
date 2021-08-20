using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Views {
    public class AppliedImportItemConfiguration : IEntityTypeConfiguration<AppliedImportItem> {
        public void Configure(EntityTypeBuilder<AppliedImportItem> builder) {
            builder.ToView(null);
            builder.ToTable("AppliedImportItem", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)");

            builder.Property(x => x.Key)
                .HasColumnName("key");

            builder.Property(x => x.KeyValue)
                .HasColumnName("key_value");

            builder.Property(x => x.ApplyDate)
                .HasColumnName("apply_date");

            builder.Property(x => x.BackupFile)
                .HasColumnName("backup_file_path");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.Login)
                .HasColumnName("login");
        }
    }
}
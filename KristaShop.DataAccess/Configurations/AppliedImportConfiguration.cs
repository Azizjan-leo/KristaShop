using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class AppliedImportConfiguration : IEntityTypeConfiguration<AppliedImport> {
        public void Configure(EntityTypeBuilder<AppliedImport> builder) {
            builder.ToTable("applied_imports");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Key)
                .HasColumnName("key")
                .HasMaxLength(64)
                .HasColumnType("varchar(64)")
                .IsRequired();

            builder.Property(x => x.KeyValue)
                .HasColumnName("key_value")
                .HasMaxLength(128)
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder.Property(x => x.ApplyDate)
                .HasColumnName("apply_date")
                .IsRequired();

            builder.Property(x => x.BackupFile)
                .HasColumnName("backup_file_path")
                .HasMaxLength(64)
                .HasColumnType("varchar(64)")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();
        }
    }
}
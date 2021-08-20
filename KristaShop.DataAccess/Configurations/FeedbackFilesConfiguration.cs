using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class FeedbackFilesConfiguration : IEntityTypeConfiguration<FeedbackFile> {
        public void Configure(EntityTypeBuilder<FeedbackFile> builder) {
            builder.ToTable("feedback_files");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Feedback)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.ParentId);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.ParentId)
                .HasColumnName("parent_id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Filename)
                .HasColumnName("filename")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.VirtualPath)
                .HasColumnName("virtual_path")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("current_timestamp()")
                .IsRequired();
        }
    }
}
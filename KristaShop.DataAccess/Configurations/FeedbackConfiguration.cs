using System;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback> {
        public void Configure(EntityTypeBuilder<Feedback> builder) {
            builder.ToTable("feedback_items");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.Person)
                .HasColumnName("person")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.Message)
                .HasColumnName("message")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(64);

            builder.Property(x => x.Viewed)
                .HasColumnName("viewed")
                .HasDefaultValue(false)
                .HasColumnType("TINYINT(1)")
                .IsRequired();

            builder.Property(x => x.RecordTimeStamp)
                .HasColumnName("record_time_stamp")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("current_timestamp()")
                .IsRequired();

            builder.Property(x => x.ViewTimeStamp)
                .HasColumnName("view_time_stamp");

            builder.Property(x => x.ReviewerUserId)
                .HasColumnName("user_id")
                .HasColumnType("binary(16)")
                .HasDefaultValue(Guid.Empty)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasColumnType("INT(11)")
                .HasColumnName("type")
                .IsRequired();
        }
    }
}

using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class DocumentConfiguration : IEntityTypeConfiguration<Document> {
        public void Configure(EntityTypeBuilder<Document> builder) {
            builder.ToTable("part_documents");
            builder.HasKey(x => x.Id);
            builder.HasDiscriminator(x => x.DocumentType);
            builder.HasIndex(x => x.Number).IsUnique();

            builder.Property(x => x.DocumentType)
                .HasColumnName("document_type")
                .HasColumnType("varchar(128)")
                .HasMaxLength(128)
                .IsRequired();

            builder.HasOne(x => x.Partner)
             .WithMany(x => x.Documents)
             .HasForeignKey(x => x.UserId)
             .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.ParentId)
                .HasColumnName("parent_id")
                .IsRequired(false);
            
            builder.Property(x => x.Number)
                .HasColumnName("number")
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.Direction)
                .HasColumnName("direction")
                .IsRequired();
            
            builder.Property(x => x.State)
                .HasColumnName("state")
                .HasConversion(new EnumToStringConverter<State>())
                .HasColumnType("varchar(32)")
                .IsRequired();

            builder.Ignore(x => x.Name);
        }
    }
}
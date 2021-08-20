using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class DocumentItemsConfiguration : IEntityTypeConfiguration<DocumentItem> {
        public void Configure(EntityTypeBuilder<DocumentItem> builder) {
            builder.ToTable("part_documents_items");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Document)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.DocumentId);

            builder.HasOne(x => x.Model)
                .WithMany()
                .HasForeignKey(x => x.ModelId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Color)
                .WithMany()
                .HasForeignKey(x => x.ColorId)
                .ExcludeForeignKeyFromMigration();
            
            builder.HasOne(x => x.FromDocument)
                .WithMany()
                .HasForeignKey(x => x.FromDocumentId);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();
            
            builder.Property(x => x.DocumentId)
                .HasColumnName("document_id")
                .IsRequired();

            builder.Property(x => x.Articul)
                .HasColumnName("articul")
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");
            
            builder.Property(x => x.ModelId)
                .HasColumnName("model_id")
                .IsRequired();
            
            builder.Property(x => x.Size)
                .HasColumnName("size_value")
                .HasColumnType("nvarchar(255)")
                .HasConversion(ValueConverters.SizeConverter, ValueConverters.SizeComparer)
                .IsRequired();

            builder.Property(x => x.ColorId)
                .HasColumnName("color_id")
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(x => x.PriceInRub)
                .HasColumnName("price_rub")
                .IsRequired();

            builder.Property(x => x.Date)
                .HasColumnName("operation_date")
                .IsRequired();

            builder.Property(x => x.FromDocumentId)
                .HasColumnName("from_document_id");

        }
    }
}
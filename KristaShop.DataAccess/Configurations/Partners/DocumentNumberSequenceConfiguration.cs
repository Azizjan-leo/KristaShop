using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Partners {
    public class DocumentNumberSequenceConfiguration : IEntityTypeConfiguration<DocumentNumberSequence> {
        public void Configure(EntityTypeBuilder<DocumentNumberSequence> builder) {
            builder.ToTable("part_documents_sequence");
            
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();
        }
    }
}
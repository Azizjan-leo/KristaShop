using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class ModelConfiguration : IEntityTypeConfiguration<Model> {
        public void Configure(EntityTypeBuilder<Model> builder) {
            builder.ToTable("1c_models", t => t.ExcludeFromMigrations());
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Collection)
                .WithMany()
                .HasForeignKey(x => x.CollectionId)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Descriptor)
                .WithMany()
                .HasForeignKey(x => x.Articul)
                .HasPrincipalKey(x => x.Articul)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Material)
                .WithMany(x => x.Models)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict)
                .ExcludeForeignKeyFromMigration();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(x => x.Articul)
                .HasColumnName("articul");

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(x => x.Parts)
                .HasColumnName("razmerov")
                .IsRequired();

            builder.Property(x => x.Weight)
                .HasColumnName("weight")
                .IsRequired();

            builder.Property(x => x.SizeLine)
                .HasColumnName("line")
                .IsRequired();

            builder.Property(x => x.MaterialId)
                .HasColumnName("material");

            builder.Property(x => x.Discount)
                .HasColumnName("skidka")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(x => x.CollectionId)
                .HasColumnName("collection");
        }
    }
}
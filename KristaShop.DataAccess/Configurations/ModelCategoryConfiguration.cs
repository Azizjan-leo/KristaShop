using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class ModelCategoryConfiguration : IEntityTypeConfiguration<ModelCategory> {
        public void Configure(EntityTypeBuilder<ModelCategory> builder) {
            builder.ToTable("1c_modelrazdely", x => x.ExcludeFromMigrations());

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Model)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.ModelId)
                .OnDelete(DeleteBehavior.Restrict)
                .ExcludeForeignKeyFromMigration();

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .HasPrincipalKey(x => x.CategoryId1C)
                .OnDelete(DeleteBehavior.Restrict)
                .ExcludeForeignKeyFromMigration();

            builder.Property(x => x.ModelId)
                .HasColumnName("model");

            builder.Property(x => x.CategoryId)
                .HasColumnName("razdel");
        }
    }
}
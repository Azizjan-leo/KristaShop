using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class CatalogItemBriefConfiguration : IEntityTypeConfiguration<Entities.DataFrom1C.CatalogItemBrief> {
        public void Configure(EntityTypeBuilder<Entities.DataFrom1C.CatalogItemBrief> builder) {
            builder.ToView("1c_catalog_item_brief");
            builder.ToTable("CatalogItemBrief", t => t.ExcludeFromMigrations());
            builder.HasNoKey();

            builder
                .Property(v => v.Articul)
                .HasColumnName("articul");
            builder
                .Property(v => v.IsVisible)
                .HasColumnName("is_visible")
                .HasColumnType("tinyint(1)");
            builder
                .Property(v => v.CatalogId)
                .HasColumnName("razdel_id");
            builder
                .Property(v => v.Price)
                .HasColumnName("price");
            builder
                .Property(v => v.PriceInRub)
                .HasColumnName("price_rub");
            builder
                .Property(v => v.Order)
                .HasColumnName("order");
            builder
                .Property(v => v.MainPhoto)
                .HasColumnName("main_photo");
            builder
                .Property(v => v.AltText)
                .HasColumnName("alt_text");
            builder
                .Property(v => v.AddDate)
                .HasColumnName("add_date");
            builder
                .Property(v => v.ItemsCount)
                .HasColumnName("tot_items_count");
            builder
                .Property(v => v.Description)
                .HasColumnName("description");
            
            builder.Property(x => x.IsLimited)
                .HasColumnName("is_limited");
        }
    }
}

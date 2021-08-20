using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations {
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem> {
        public void Configure(EntityTypeBuilder<MenuItem> builder) {
            builder.ToTable("menu_items");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ParentItem)
                .WithMany(x => x.ChildItems)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.Property(x => x.MenuType)
                .HasColumnName("menu_type")
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.AreaName)
                .HasColumnName("area_name")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.ControllerName)
                .HasColumnName("controller_name")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.ActionName)
                .HasColumnName("action_name")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.Url)
                .HasColumnName("url")
                .HasMaxLength(256);

            builder.Property(x => x.Icon)
                .HasColumnName("icon")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)");

            builder.Property(x => x.Order)
                .HasColumnName("order");

            builder.Property(x => x.ParentId)
                .HasColumnName("parent_id")
                .HasColumnType("binary(16)")
                .IsRequired(false);

            builder.Property(x => x.BadgeTarget)
                .HasColumnName("badge_target")
                .HasMaxLength(32)
                .HasColumnType("varchar(32)")
                .IsRequired(false);

            builder.Ignore(x => x.Url);
        }
    }
}
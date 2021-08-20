using KristaShop.Common.Models;
using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.DataAccess.Configurations.Views {
    public class CreateTableStatementConfiguration : IEntityTypeConfiguration<CreateTableStatement> {
        public void Configure(EntityTypeBuilder<CreateTableStatement> builder) {
            builder.HasNoKey();
            builder.ToTable("CreateTableStatement", t => t.ExcludeFromMigrations());
            builder.ToView(null);
            builder.Property(x => x.CreateTable)
                .HasColumnName("Create Table");
        }
    }

    public class ScalarConfiguration : IEntityTypeConfiguration<Scalar> {
        public void Configure(EntityTypeBuilder<Scalar> builder) {
            builder.ToTable("Scalar", t => t.ExcludeFromMigrations());
            builder.HasNoKey();
            builder.ToView(null);
        }
    }

    public class ScalarIntConfiguration : IEntityTypeConfiguration<ScalarInt> {
        public void Configure(EntityTypeBuilder<ScalarInt> builder) {
            builder.ToTable("ScalarInt", t => t.ExcludeFromMigrations());
            builder.HasNoKey();
            builder.ToView(null);
        }
    }
    
    public class ScalarLongConfiguration : IEntityTypeConfiguration<ScalarULong> {
        public void Configure(EntityTypeBuilder<ScalarULong> builder) {
            builder.ToTable("ScalarLong", t => t.ExcludeFromMigrations());
            builder.HasNoKey();
            builder.ToView(null);
        }
    }

    public class LookupListItemConfiguration : IEntityTypeConfiguration<LookUpItem<int, string>> {
        public void Configure(EntityTypeBuilder<LookUpItem<int, string>> builder) {
            builder.ToTable("LookUpItem<int, string>", t => t.ExcludeFromMigrations());
            builder.HasNoKey();
            builder.ToView(null);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace KristaShop.Common.Extensions {
    public static class EfForeignKeyExtensions {
        public static ReferenceCollectionBuilder<TEntity, TRelatedEntity> ExcludeForeignKeyFromMigration<TEntity,
            TRelatedEntity>(
            [NotNull] this ReferenceCollectionBuilder<TEntity, TRelatedEntity> referenceCollectionBuilder)
            where TEntity : class
            where TRelatedEntity : class {
            return (ReferenceCollectionBuilder<TEntity, TRelatedEntity>)
                ((ReferenceCollectionBuilder) referenceCollectionBuilder).HasConstraintName(
                    GenerateExcludeFromMigrationKey());
        }

        public static ReferenceReferenceBuilder<TEntity, TRelatedEntity> ExcludeForeignKeyFromMigration<TEntity,
            TRelatedEntity>(
            [NotNull] this ReferenceReferenceBuilder<TEntity, TRelatedEntity> referenceCollectionBuilder)
            where TEntity : class
            where TRelatedEntity : class {
            return (ReferenceReferenceBuilder<TEntity, TRelatedEntity>)
                ((ReferenceReferenceBuilder)referenceCollectionBuilder).HasConstraintName(
                    GenerateExcludeFromMigrationKey());
        }

        private static string GenerateExcludeFromMigrationKey() {
            return $"{MigrationsModelDifferWithoutForeignKey.ExcludeFromMigrationKey}{Guid.NewGuid():N}";
        }
    }

    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public class MigrationsModelDifferWithoutForeignKey : MigrationsModelDiffer {
        public const string ExcludeFromMigrationKey = "ExcludeForeignKeyMigration";

        public MigrationsModelDifferWithoutForeignKey
        ([NotNull] IRelationalTypeMappingSource typeMappingSource,
            [NotNull] IMigrationsAnnotationProvider migrationsAnnotations,
            [NotNull] IChangeDetector changeDetector,
            [NotNull] IUpdateAdapterFactory updateAdapterFactory,
            [NotNull] CommandBatchPreparerDependencies commandBatchPreparerDependencies)
            : base(typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory,
                commandBatchPreparerDependencies) { }

        public override IReadOnlyList<MigrationOperation> GetDifferences(IRelationalModel source,
            IRelationalModel target) {
            var operations = base.GetDifferences(source, target)
                .Where(op => op is not AddForeignKeyOperation || op is AddForeignKeyOperation opObject && !opObject.Name.StartsWith(ExcludeFromMigrationKey))
                .Where(op => op is not DropForeignKeyOperation || op is DropForeignKeyOperation opObject && !opObject.Name.StartsWith(ExcludeFromMigrationKey))
                .ToList();
            
            foreach (var operation in operations.OfType<CreateTableOperation>()) {
                operation.ForeignKeys?.RemoveAll(x => x.Name.StartsWith(ExcludeFromMigrationKey));
            }

            return operations;
        }
    }
}
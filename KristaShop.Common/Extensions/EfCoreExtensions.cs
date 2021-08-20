using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Structs;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.Common.Extensions {
    public static class EfCoreExtensions {
        public static IQueryable<TEntity> IncludeWhere<TEntity, TProperty>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>> includeNavigationPropertyPath,
            Expression<Func<TEntity, bool>> wherePredicate)
            where TEntity : class {
            return source.Include(includeNavigationPropertyPath)
                .Where(wherePredicate);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector, CatalogOrderDir direction) {
            return direction == CatalogOrderDir.Asc ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector, CatalogOrderDir direction) {
            return direction == CatalogOrderDir.Asc ? source.ThenBy(keySelector) : source.ThenByDescending(keySelector);
        }

        public static IQueryable<TEntity> ForPage<TEntity>(this IQueryable<TEntity> source, Page page)
            where TEntity : class {
            if (!page.IsValid()) {
                return source;
            }

            return source.Skip(page.GetSizeToSkip()).Take(page.Size);
        }
    }
}
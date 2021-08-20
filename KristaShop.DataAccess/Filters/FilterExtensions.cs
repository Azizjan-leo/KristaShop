using System.Linq;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Interfaces;

namespace KristaShop.DataAccess.Filters {
    public static class FilterExtensions {
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source, ReportsFilter filter)
            where TSource : IUserCatalogNomenclature {
            return ReportsFilterMethods.BuildQuery(source, filter);
        }
        
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source, ProductsFilter filter)
            where TSource : CatalogItem {
            return ProductsFilterBuilder.BuildQuery(source, filter);
        }
    }
}
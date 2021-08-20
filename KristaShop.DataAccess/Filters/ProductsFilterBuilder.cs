using System.Linq;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Filters {
    internal static class ProductsFilterBuilder {
        public static IQueryable<TSource> BuildQuery<TSource>(IQueryable<TSource> source, ProductsFilter filter)
            where TSource : CatalogItem {
            
            if (!filter.ShowHiddenModels) {
                source = source.Where(x => x.Model.Descriptor.IsVisible == true)
                    .Where(x => x.Invisibility == null);
            }

            if (filter.HideEmptySlots) {
                source = source.Where(x => x.Amount > 0);
            }

            if (filter.Articuls != null && filter.Articuls.Any()) {
                source = source.Where(x => filter.Articuls.Contains(x.Model.Articul));
            }

            if (filter is ProductsFilterExtended filterExtended) {
                source = _applyExtendedFilter(source, filterExtended);
            } else if (filter.AllowFilterByCatalog) {
                source = source.Where(x => x.CatalogId == filter.CatalogId);
            }
            
            return source;
        }

        private static IQueryable<TSource> _applyExtendedFilter<TSource>(IQueryable<TSource> source, ProductsFilterExtended filter)
            where TSource : CatalogItem {
            if (!string.IsNullOrEmpty(filter.Articul)) {
                source = source.Where(x => EF.Functions.Like(x.Articul, $"%{filter.Articul}%"));
            }

            if (filter.MinPrice > 0.0d) {
                source = source.Where(x => x.Price >= filter.MinPrice);
            }

            if (filter.MaxPrice > 0.0d) {
                source = source.Where(x => x.Price <= filter.MaxPrice);
            }

            if (!filter.AllowFilterByCatalog && filter.CatalogIds != null && filter.CatalogIds.Any()) {
                source = source.Where(x => filter.CatalogIds.Contains((int) x.CatalogId));
            }
            
            if (filter.CategoriesIds != null && filter.CategoriesIds.Any()) {
                source = source.Where(x => x.Model.Categories.Any(c => filter.CategoriesIds.Contains(c.CategoryId)));
            }
            
            if (filter.ColorIds != null && filter.ColorIds.Any()) {
                source = source.Where(x => filter.ColorIds.Contains(x.ColorId));
            }
            
            if (filter.Sizes != null && filter.Sizes.Any()) {
                source = source.Where(x => filter.Sizes.Contains(x.SizeValue));
            }
            
            if (filter.SizeLines != null && filter.SizeLines.Any()) {
                source = source.Where(x => filter.Sizes.Contains(x.Model.SizeLine));
            }
            
            if (filter.IncludeWithoutCatalog) {
                // TODO
            }

            return source;
        }
    }
}
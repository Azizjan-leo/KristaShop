using System.Linq;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities.Interfaces;

namespace KristaShop.DataAccess.Filters {
    internal static class ReportsFilterMethods {
        public static IQueryable<TSource> BuildQuery<TSource>(IQueryable<TSource> source, ReportsFilter filter)
            where TSource : IUserCatalogNomenclature {
            source = _applyNomenclatureFilter(source, filter);
            source = _applyUserFilter(source, filter);
            source = _applyCatalogFilter(source, filter);
            return source;
        }
        
        private static IQueryable<TSource> _applyNomenclatureFilter<TSource>(IQueryable<TSource> source, ReportsFilter filter)
            where TSource : INomenclature {
            if (filter.SelectedArticuls.Any()) {
                source = source.Where(x => filter.SelectedArticuls.Contains(x.Model.Articul));
            }

            if (filter.SelectedColorIds.Any()) {
                source = source.Where(x => filter.SelectedColorIds.Contains(x.ColorId));
            }

            return source;
        }
        
        private static IQueryable<TSource> _applyUserFilter<TSource>(IQueryable<TSource> source, ReportsFilter filter)
            where TSource : IUserNomenclature {
            source = _applyNomenclatureFilter(source, filter);

            if (filter.SelectedCityIds.Any()) {
                source = source.Where(x => filter.SelectedCityIds.Contains(x.User.CityId.Value));
            }

            if (filter.SelectedUserIds.Any()) {
                source = source.Where(x => filter.SelectedUserIds.Contains(x.UserId));
            }

            if (filter.SelectedManagerIds.Any()) {
                source = source.Where(x => filter.SelectedManagerIds.Contains(x.User.ManagerId.Value));
            }

            return source;
        }
        
        private static IQueryable<TSource> _applyCatalogFilter<TSource>(IQueryable<TSource> source, ReportsFilter filter)
            where TSource : IUserCatalogNomenclature {
            if (filter.SelectedCatalogIds.Any()) {
                source = source.Where(x => filter.SelectedCatalogIds.Contains((int) x.CatalogId));
            }

            return source;
        }
    }
}
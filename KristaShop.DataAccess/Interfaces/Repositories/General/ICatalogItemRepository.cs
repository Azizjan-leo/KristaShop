using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface ICatalogItemRepository : IRepository<CatalogItem, int> {
        Task<List<CatalogItem>> GetCatalogItems(CatalogType catalogId, string articul, string sizeValue, int colorId);
        Task<IEnumerable<CatalogItemBrief>> GetBriefTopCatalogItemsAsync(CatalogType catalogId, int quantity);
        Task<IEnumerable<CatalogItemBrief>> GetBriefByFilterAsync(ProductsFilterExtended filter, Page page);
        Task<int> GetTotalItemsCountByFilterAsync(ProductsFilterExtended filter);
        Task<IEnumerable<CatalogItemFull>> GetFullsByArticulsAsync(IEnumerable<int> catalogIds, IEnumerable<string> articuls, bool excludeHiddenByColors = true);
        Task<int> GetCatalogItemMaxAmountAsync(CatalogType catalogId, int modelId, int colorId);
        Task<IEnumerable<CatalogItemForAdd>> GetCatalogItemsForAdd();
        Task<IEnumerable<CatalogItemFull>> GetHistoryCatalogItemsAsync();
        Task<IEnumerable<CatalogItemFull>> GetHistoryCatalogItemsByArticulAsync(string articul);
        Task<Dictionary<string, List<ModelToCatalog1CMap>>> GetCatalogsByArticulAsync(bool includeWithoutCatalog = false);
        Task<Dictionary<string, List<ModelToCategory1CMap>>> GetCategoriesByArticulAsync();
        Task<(List<string> sizes, List<string> sizeLines)> GetSizesInCatalogsAsync(bool hideEmpty = false);
        Task<List<LookUpItem<int, string>>> GetColorsInCatalogsAsync(bool hideEmpty = false);
    }
}
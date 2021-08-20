using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Filters;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class CatalogItemRepository : Repository<CatalogItem, int>, ICatalogItemRepository {

        public CatalogItemRepository(DbContext context) : base(context) { }

        public async Task<List<CatalogItem>> GetCatalogItems(CatalogType catalogId, string articul, string sizeValue, int colorId) {
            return await All.Where(x =>
                x.Articul == articul && x.ColorId == colorId &&
                (x.SizeValue == sizeValue || x.Model.SizeLine == sizeValue) && x.CatalogId == catalogId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CatalogItemBrief>> GetBriefTopCatalogItemsAsync(CatalogType catalogId, int quantity) {
            return await _createBriefItemsPagedListAsync(new ProductsFilter {
                CatalogId = catalogId,
                OrderDirection = CatalogOrderDir.Asc,
                OrderType = CatalogOrderType.OrderByPosition,
                ShowHiddenModels = false,
                Articuls = null,
                HideEmptySlots = catalogId != CatalogType.Open,
                IncludeDescription = true
            }, new Page(0, quantity));
        }

        public async Task<IEnumerable<CatalogItemBrief>> GetBriefByFilterAsync(ProductsFilterExtended filter, Page page) {
            return await _createBriefItemsPagedListAsync(filter, page);
        }

        public async Task<int> GetTotalItemsCountByFilterAsync(ProductsFilterExtended filter) {
            return await _getTotalItemsCountByFilterAsync(filter);
        }

        public async Task<IEnumerable<CatalogItemFull>> GetFullsByArticulsAsync(IEnumerable<int> catalogIds,
            IEnumerable<string> articuls, bool excludeHiddenByColors = true) {
            return await _getFullsByArticulsAsync(catalogIds, articuls, excludeHiddenByColors);
        }
        
        public async Task<int> GetCatalogItemMaxAmountAsync(CatalogType catalogId, int modelId, int colorId) {
            return await All
                .Where(x => x.CatalogId == catalogId && x.ModelId == modelId && x.ColorId == colorId)
                .SumAsync(x => x.Amount);
        }
        
        public async Task<IEnumerable<CatalogItemForAdd>> GetCatalogItemsForAdd() {
            var result = await All
                .Where(x => x.CatalogId != 0 && x.Amount > 0)
                .GroupBy(x => new {
                    Catalog = x.CatalogId,
                    x.ModelId,
                    x.NomenclatureId,
                    x.ColorId,
                    x.SizeValue,
                    x.Model.Articul,
                    x.Model.SizeLine,
                    x.Model.Parts,
                    ColorName = x.Color.Name,
                    x.Color.Group.Hex,
                    x.Model.Descriptor.MainPhoto
                })
                .Select(x => new CatalogItemForAdd {
                    CatalogId = (int) x.Key.Catalog,
                    CatalogName = x.Key.Catalog.GetDisplayName(),
                    ModelId = x.Key.ModelId,
                    NomenclatureId = x.Key.NomenclatureId,
                    ColorId = x.Key.ColorId,
                    Size = x.Key.SizeValue,
                    Articul = x.Key.Articul,
                    SizeLine = x.Key.SizeLine,
                    PartsCount = x.Key.Parts,
                    ColorName = x.Key.ColorName,
                    ColorValue = x.Key.Hex,
                    ColorPhoto = "",
                    PhotoByColor = "",
                    MainPhoto = x.Key.MainPhoto,
                    Amount = x.Sum(c => c.Amount)
                })
                .ToListAsync();

            return result;
        }
        
        public async Task<IEnumerable<CatalogItemFull>> GetHistoryCatalogItemsAsync() {
            return await _getHistoryItemsAsync();
        }
        
        public async Task<IEnumerable<CatalogItemFull>> GetHistoryCatalogItemsByArticulAsync(string articul) {
            return await _getHistoryItemByArticulAsync(articul);
        }

        public async Task<Dictionary<string, List<ModelToCatalog1CMap>>> GetCatalogsByArticulAsync(bool includeWithoutCatalog) {
            var result = !includeWithoutCatalog
                ? await _getCatalogIdsByArticuls()
                : await _getCatalogIdsByArticulsIncludeArticulsWithoutCatalog();
            return result.GroupBy(x => x.Articul).ToDictionary(k => k.Key, v => v.ToList());
        }

        public async Task<Dictionary<string, List<ModelToCategory1CMap>>> GetCategoriesByArticulAsync() {
            var result = await Context.Set<Model>().SelectMany(model => model.Categories, (model, modelCategory) =>
                    new ModelToCategory1CMap {
                        Articul = model.Articul,
                        CategoryId = modelCategory.CategoryId,
                        CategoryName = modelCategory.Category.Name
                    }).Distinct()
                .ToListAsync();
            
            return result.GroupBy(x => x.Articul).ToDictionary(k => k.Key, v => v.ToList());
        }

        public async Task<(List<string> sizes, List<string> sizeLines)> GetSizesInCatalogsAsync(bool hideEmpty = false) {
            var query = All;
            if (hideEmpty) {
                query = query.Where(x => x.Amount > 0);
            }

            var selectedSizes = await query
                .Select(x => new {x.SizeValue, x.Model.SizeLine})
                .Distinct()
                .ToListAsync();

            var sizeLines = selectedSizes
                .Where(x => x.SizeValue == "0")
                .Select(x => x.SizeLine)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var sizes = selectedSizes
                .Where(x => x.SizeValue != "0")
                .Select(x => x.SizeValue)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            
            return (sizes, sizeLines);
        }

        public async Task<List<LookUpItem<int, string>>> GetColorsInCatalogsAsync(bool hideEmpty = false) {
            var query = All;
            if (hideEmpty) {
                query = query.Where(x => x.Amount > 0);
            }

            var result = await query
                .Select(x => new LookUpItem<int, string>(x.Color.Id, x.Color.Name))
                .Distinct()
                .ToListAsync();

            return result.OrderBy(x => x.Value).ToList();
        }
        
        private async Task<List<CatalogItemBrief>> _createBriefItemsPagedListAsync(ProductsFilter filter, Page page) {
            var result =  await _createCatalogBriefItemsList(filter)
                .ForPage(page)
                .ToListAsync();

            return result;
        }

        private IQueryable<CatalogItemBrief> _createCatalogBriefItemsList(ProductsFilter filter) {
            if (filter.CatalogId == CatalogType.All) {
                return _createAllCatalogsBriefItemsList(filter);
            }
            
            var query = All
                .Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Filter(filter)
                .GroupBy(x => new {
                    x.Model.Articul,
                    x.Model.Descriptor.MainPhoto,
                    x.Model.Descriptor.AltText,
                    IsLimited = x.Model.Descriptor != null ? x.Model.Descriptor.IsLimited : (bool?) null,
                    IsVisible = x.Model.Descriptor != null ? x.Model.Descriptor.IsVisible : null,
                    x.Model.Descriptor.Description,
                    Order = x.ModelCatalogOrder != null ? x.ModelCatalogOrder.Order : 0 ,
                    CatalogId = x.CatalogId
                })
                .Select(x => new CatalogItemBrief {
                    Articul = x.Key.Articul,
                    CatalogId = (int) x.Key.CatalogId,
                    MainPhoto = x.Key.MainPhoto ?? "",
                    AddDate = x.Min(c => c.ExecutionDate),
                    AltText = x.Key.AltText ?? "",
                    IsLimited = x.Key.IsLimited ?? false,
                    IsVisible = x.Key.IsVisible ?? false,
                    Description = x.Key.Description ?? "",
                    Price = x.Min(c => c.Price),
                    PriceInRub = x.Min(c => c.PriceRub),
                    ItemsCount = x.Sum(c => c.Amount),
                    Order = x.Key.Order,
                });

            return _createOrderedQuery(query, filter.OrderDirection, filter.OrderType);
        }

        private IQueryable<CatalogItemBrief> _createAllCatalogsBriefItemsList(ProductsFilter filter) {
            var query = All
                .Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Filter(filter)
                .GroupBy(x => new {
                    x.Model.Articul,
                    x.Model.Descriptor.MainPhoto,
                    x.Model.Descriptor.AltText,
                    IsLimited = x.Model.Descriptor != null ? x.Model.Descriptor.IsLimited : (bool?) null,
                    IsVisible = x.Model.Descriptor != null ? x.Model.Descriptor.IsVisible : null,
                    x.Model.Descriptor.Description,
                    Order = 0,
                    CatalogId = CatalogType.All
                })
                .Select(x => new CatalogItemBrief {
                    Articul = x.Key.Articul,
                    CatalogId = (int) x.Key.CatalogId,
                    MainPhoto = x.Key.MainPhoto ?? "",
                    AddDate = x.Min(c => c.ExecutionDate),
                    AltText = x.Key.AltText ?? "",
                    IsLimited = x.Key.IsLimited ?? false,
                    IsVisible = x.Key.IsVisible ?? false,
                    Description = x.Key.Description ?? "",
                    Price = x.Min(c => c.Price),
                    PriceInRub = x.Min(c => c.PriceRub),
                    ItemsCount = x.Sum(c => c.Amount),
                    Order = x.Key.Order,
                });

            return _createOrderedQuery(query, filter.OrderDirection, filter.OrderType);
        }

        private static IQueryable<CatalogItemBrief> _createOrderedQuery(IQueryable<CatalogItemBrief> query,
            CatalogOrderDir orderDirection, CatalogOrderType orderType) {
            return orderType switch {
                CatalogOrderType.OrderByPosition => query.OrderBy(x => x.Order, orderDirection)
                    .ThenBy(x => x.Articul, orderDirection),
                CatalogOrderType.OrderByPrice => query.OrderBy(x => x.Price, orderDirection)
                    .ThenBy(x => x.Articul, orderDirection),
                CatalogOrderType.OrderByDate => query.OrderBy(x => x.AddDate, orderDirection)
                    .ThenBy(x => x.Articul, orderDirection),
                CatalogOrderType.OrderRandom => query.OrderBy(x => CustomMysqlFunctions.Random()).ThenBy(x => x.Articul),
                _ => query.OrderBy(x => x.Order, orderDirection).ThenBy(x => x.Articul, orderDirection)
            };
        }

        private async Task<int> _getTotalItemsCountByFilterAsync(ProductsFilter filter) {
            if (filter.CatalogId == CatalogType.All) {
                throw new ExceptionBase($"Catalog should not be {CatalogType.All}");
            }

            return await All
                .Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Filter(filter)
                .GroupBy(x => new {
                    x.Model.Articul,
                    x.Model.Descriptor.MainPhoto,
                    x.Model.Descriptor.AltText,
                    IsLimited = x.Model.Descriptor != null ? x.Model.Descriptor.IsLimited : (bool?) null,
                    IsVisible = x.Model.Descriptor != null ? x.Model.Descriptor.IsVisible : null,
                    x.Model.Descriptor.Description,
                    x.ModelCatalogOrder.Order,
                    CatalogId = x.CatalogId
                }).CountAsync();
        }
        
        private async Task<IEnumerable<CatalogItemFull>> _getFullsByArticulsAsync(IEnumerable<int> catalogIds,
            IEnumerable<string> articuls, bool excludeHiddenByColors) {
            var query = All
                .Where(x => articuls.Contains(x.Model.Articul))
                .Where(x => catalogIds.Contains((int) x.CatalogId));

            if (excludeHiddenByColors) {
                query = query.Where(x => x.ItemVisibility.IsVisible || x.ItemVisibility == null);
            }
            
            var result = await query.Select(x => new CatalogItemFull {
                Id = x.Id,
                ModelId = x.ModelId,
                ColorId = x.ColorId,
                ItemsCount = x.Amount,
                CatalogId = (int) x.CatalogId,
                Price = x.Price,
                PriceInRub = x.PriceRub,
                Size = x.SizeValue,
                SizeLine = x.Model.SizeLine,
                NomenclatureId = x.NomenclatureId,
                ModelName = x.Model.Name,
                Articul = x.Model.Articul,
                Status = x.Model.Status,
                PartsCount = x.Model.Parts,
                Weight = x.Model.Weight,
                MaterialId = x.Model.MaterialId ?? 0,
                Discount = x.Model.Discount,
                MaterialName = x.Model.Material.Name ?? "",
                ColorName = x.Color.Name,
                ColorGroupId = x.Color.GroupId ?? 0,
                ColorGroupName = x.Color.Group.Name,
                ColorPhoto = "",
                ColorValue = x.Color.Group.Hex ?? "",
                IsVisibleColor = x.ItemVisibility == null || x.ItemVisibility.IsVisible
            }).ToListAsync();

            return result;
        }
        
        private async Task<IEnumerable<CatalogItemFull>> _getHistoryItemsAsync() {
            return await _createHistoryFullCatalogItemsList(Context.Set<Barcode>()).ToListAsync();
        }

        private async Task<IEnumerable<CatalogItemFull>> _getHistoryItemByArticulAsync(string articul) {
            var query = Context.Set<Barcode>().AsQueryable();
            if (!string.IsNullOrEmpty(articul)) {
                query = query.Where(x => x.Model.Articul.Equals(articul));
            }

            return await _createHistoryFullCatalogItemsList(query).ToListAsync();
        }

        private IQueryable<CatalogItemFull> _createHistoryFullCatalogItemsList(IQueryable<Barcode> query) {
            return query.Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Model).ThenInclude(x => x.CatalogItems)
                .SelectMany(barcode => barcode.Model.CatalogItems
                        .Where(x => x.ColorId == barcode.ColorId && x.SizeValue == barcode.SizeValue || x.SizeValue == "0")
                        .DefaultIfEmpty(),
                    (barcode, catalogItem) => new CatalogItemFull {
                        ModelId = barcode.Model.Id,
                        ModelName = barcode.Model.Name,
                        Articul = barcode.Model.Articul,
                        Status = barcode.Model.Status,
                        Weight = barcode.Model.Weight,
                        PartsCount = barcode.Model.Parts,
                        SizeLine = barcode.Model.SizeLine,
                        Size = barcode.SizeValue,
                        ColorId = barcode.ColorId,
                        ColorName = barcode.Color.Name,
                        ColorGroupId = barcode.Color.GroupId ?? 0,
                        ColorGroupName = barcode.Color.Group.Name ?? "",
                        ColorValue = barcode.Color.Group.Hex ?? "",
                        ColorPhoto = "",
                        ItemsCount = (int?) catalogItem.Amount ?? 0,
                        Price = (int?) catalogItem.Price ?? barcode.Model.Price,
                        PriceInRub = (int?) catalogItem.PriceRub ?? 0,
                        CatalogId = (int) ((CatalogType?) catalogItem.CatalogId ?? CatalogType.All),
                    });
        }

        private async Task<IEnumerable<ModelToCatalog1CMap>> _getCatalogIdsByArticuls() {
            return await All.Select(x => new ModelToCatalog1CMap {
                Articul = x.Articul,
                CatalogId = (int) x.CatalogId
            }).Distinct().ToListAsync();
        }

        private async Task<IEnumerable<ModelToCatalog1CMap>> _getCatalogIdsByArticulsIncludeArticulsWithoutCatalog() {
            return await Context.Set<Barcode>()
                .SelectMany(barcode => barcode.Model.CatalogItems
                        .Where(x => x.ColorId == barcode.ColorId && x.SizeValue == barcode.SizeValue || x.SizeValue == "0"),
                    (barcode, item) => new ModelToCatalog1CMap {
                        Articul = barcode.Model.Articul,
                        CatalogId = (int) ((CatalogType?) item.CatalogId ?? CatalogType.All)
                    }).Distinct()
                .ToListAsync();
        }
    }
}
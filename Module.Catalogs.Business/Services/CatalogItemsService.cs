using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Session;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Module.Catalogs.Business.Common;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Catalogs.Business.UnitOfWork;
using Module.Common.Business.Models;

namespace Module.Catalogs.Business.Services {
    public class CatalogItemsService : ICatalogItemsService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CatalogItemsService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CatalogItemBriefDTO>> GetCatalogTopModelItemsAsync(CatalogType catalogId, int quantity = 6) {
            var result = await _uow.CatalogItems.GetBriefTopCatalogItemsAsync(catalogId, quantity);
            return _mapper.Map<IEnumerable<CatalogItemBriefDTO>>(result);
        }

        public async Task<IEnumerable<CatalogItemGroupNew>> GetFullModelsAllListAsync(ProductsFilterExtended filter,
            UserSession userSession, Page page) {
            filter.SetCatalogIds((await userSession.GetAvailableCatalogsAsync(_uow)).Select(x => x.Id).ToList());          
            var briefItemsList =
                _mapper.Map<List<CatalogItemBriefDTO>>(
                    await _uow.CatalogItems.GetBriefByFilterAsync(filter, page));
            return await _extendBriefModelsListAsync(briefItemsList, filter);
        }

        public async Task<Dictionary<CatalogType, List<CatalogItemGroupNew>>>
            GetFullModelsGroupedByCatalogsAsync(ProductsFilterExtended filter, UserSession userSession, int pageNum = 0,
                int pageSize = 25) {
            var tmpList = await GetFullModelsAllListAsync(filter, userSession, new Page(pageNum -1, pageSize));

            var resultList = new Dictionary<CatalogType, List<CatalogItemGroupNew>>();
            foreach (var item in tmpList) {
                if (item.CatalogId == (int) CatalogType.InStockLines || item.CatalogId == (int) CatalogType.InStockParts) {
                    if (item.CatalogItems.Sum(x => x.Items.Sum(c => c.Amount)) == 0) continue;
                }

                if (!resultList.ContainsKey(item.CatalogId.ToProductCatalog1CId())) {
                    resultList.Add(item.CatalogId.ToProductCatalog1CId(), new List<CatalogItemGroupNew>());
                }

                resultList[item.CatalogId.ToProductCatalog1CId()].Add(item);
            }

            return resultList;
        }

        public async Task<List<CatalogItemForAddDTO>> GetProductsListForAddAsync() {
            return _mapper.Map<List<CatalogItemForAddDTO>>(await _uow.CatalogItems.GetCatalogItemsForAdd());
        }

        public async Task<CatalogItemGroupNew> FindModelAsync(string articul, CatalogType catalogId, UserSession userSession,
            bool showEmptySlots = false) {
            var fullList = await GetFullModelsAllListAsync(new ProductsFilterExtended {
                CatalogId = catalogId, OrderDirection = CatalogOrderDir.Asc,
                OrderType = CatalogOrderType.OrderByPosition, Articuls = new[] {articul},
                HideEmptySlots = !showEmptySlots,
                IncludeDescription = true, IncludeCategoriesMap = true, IncludePhotosList = true,
                IncludeItemsDescriptions = true, ShowHiddenModels = false, ExcludeHiddenByColors = true
            }, userSession, new Page(0, 1));

            return fullList.FirstOrDefault();
        }

        public async Task<CatalogItemGroupNew> FindModelForAdminAsync(string articul, int catalogId,
            UserSession userSession) {
            var fullList = await GetFullModelsAllListAsync(new ProductsFilterExtended {
                CatalogId = catalogId.ToProductCatalog1CId(), OrderDirection = CatalogOrderDir.Asc,
                OrderType = CatalogOrderType.OrderByPosition, Articuls = new[] {articul}, HideEmptySlots = false,
                IncludeDescription = true, IncludeCategoriesMap = true, IncludePhotosList = true,
                IncludeItemsDescriptions = true, ShowHiddenModels = true, ExcludeHiddenByColors = false
            }, userSession, new Page(0, 1));

            return fullList.FirstOrDefault();
        }

        public async Task<int> GetModelsCountAsync(ProductsFilterExtended filter, UserSession userSession = null) {
            filter.SetCatalogIds((await userSession.GetAvailableCatalogsAsync(_uow)).Select(x => x.Id).ToList());
            return await _uow.CatalogItems.GetTotalItemsCountByFilterAsync(filter);
        }

        public async Task<IEnumerable<CatalogItemGroupNew>> GetFullModelsHistoryAsync() {
            var filter = new ProductsFilterExtended {
                IncludeDescription = true,
                IncludeItemsDescriptions = true,
                IncludeWithoutCatalog = true,
                IncludeCatalogsMap = true,
                IncludeCategoriesMap = true
            };
            var items = await _uow.CatalogItems.GetHistoryCatalogItemsAsync();
            return await _extendBriefCatalogItemsByArticulAsync(items.ToList(), filter);
        }

        public async Task<CatalogItemGroupNew> FindHistoryModelAsync(string articul) {
            var items = await _uow.CatalogItems.GetHistoryCatalogItemsByArticulAsync(articul);
            var filter = new ProductsFilterExtended {
                IncludeDescription = true,
                IncludeItemsDescriptions = true,
                IncludeWithoutCatalog = true,
                IncludeCatalogsMap = true,
                IncludeCategoriesMap = true
            };

            return (await _extendBriefCatalogItemsByArticulAsync(items.ToList(), filter)).First();
        }

        public async Task<List<ModelPhotoDTO>> GetModelPhotosAsync(string articul) {
            return _mapper.Map<List<ModelPhotoDTO>>(await _uow.ModelPhotos.GetAllByArticulAsync(articul));
        }

        public async Task<(IEnumerable<LookUpItem<int, string>>, IEnumerable<string>, IEnumerable<string>)>
            GetFilterLookupListsAsync(CatalogType catalogId, bool showEmptySlots = false) {
            var (sizes, sizeLines) = await _uow.CatalogItems.GetSizesInCatalogsAsync(!showEmptySlots);
            var colors = await _uow.CatalogItems.GetColorsInCatalogsAsync(!showEmptySlots);

            return (colors, sizes, sizeLines);
        }

        public IEnumerable<ColorDTO> GetAllColors(IEnumerable<CatalogItemGroupNew> itemsList) {
            return itemsList
                .SelectMany(x => x.Colors)
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .OrderBy(x => x.Name)
                .ToList();
        }

        public IEnumerable<string> GetAllSizes(IEnumerable<CatalogItemGroupNew> itemsList) {
            return itemsList.SelectMany(x => x.Sizes).Distinct()
                .OrderBy(x => x, new SizeStringComparer())
                .ToList();
        }

        public async Task<List<int>> GetModelCatalogsInvisibilityAsync(string articul) {
            return await _uow.ModelCatalogInvisibilities.GetInvisibilityInCatalogIdsByArticulAsync(articul);
        }

        public async Task UpdateCatalogItemVisibility(string articul, int modelId, SizeValue size, int colorId,
            CatalogType catalogId, bool isVisible) {
            await _uow.CatalogItemsVisibility.CreateOrUpdateAsync(articul, modelId, size, colorId, catalogId, isVisible);
            await _uow.SaveChangesAsync();
        }

        private async Task<List<CatalogItemGroupNew>> _extendBriefModelsListAsync(
            IList<CatalogItemBriefDTO> sourceList, ProductsFilterExtended filter) {
            if (!sourceList.Any()) return new List<CatalogItemGroupNew>();
            var articuls = sourceList.Select(s => s.Articul).Distinct().ToList();
            var fullItems =
                await _uow.CatalogItems.GetFullsByArticulsAsync(filter.CatalogIds, articuls, filter.ExcludeHiddenByColors);
            filter.IncludeItemsDescriptions = true;
            if (filter.CatalogId == CatalogType.All) {
                return await _extendBriefCatalogItemsByArticulAsync((fullItems).ToList(), filter);
            }

            return await _extendBriefCatalogItemsByArticulAndCatalogAsync(sourceList, fullItems, articuls, filter);
        }

        private async Task<List<CatalogItemGroupNew>> _extendBriefCatalogItemsByArticulAndCatalogAsync(
            IList<CatalogItemBriefDTO> sourceList,
            IEnumerable<CatalogItemFull> fullItems, IReadOnlyCollection<string> articuls,
            ProductsFilterExtended filter) {
            var sourceItems = sourceList
                .GroupBy(x => new {x.Articul, x.CatalogId})
                .ToDictionary(k => new {k.Key.Articul, k.Key.CatalogId}, v => v.First());

            var (descriptors, catalogs, categories, photos) = await CatalogItemDescriptors(filter, articuls);

            if (filter.HideEmptySlots) {
                fullItems = fullItems.Where(x => x.ItemsCount > 0);
            }

            var result = fullItems.GroupBy(x => new {x.Articul, x.CatalogId}, (key, groupedItems) => {
                    var items = groupedItems.ToList();

                    foreach (var catalogItem in items) {
                        if (string.IsNullOrEmpty(catalogItem.Size) || catalogItem.Size.Equals("0")) {
                            catalogItem.Size = catalogItem.SizeLine;
                        }
                    }

                    var item = new CatalogItemGroupNew {
                        Descriptor = new CatalogItemDescriptorDTO() {
                            Articul = key.Articul
                        },
                        CatalogId = key.CatalogId,
                        CommonPrice = items.Min(x => x.Price),
                        CommonPriceInRub = items.Min(x => x.PriceInRub),
                        Sizes = items.Select(x=>x.RealSize.Value).Distinct().OrderBy(x => x, new SizeStringComparer()).ToList(),
                        Colors = _mapper.Map<List<ColorDTO>>(items).Distinct().OrderBy(x => x.Name).ToList(),
                        CatalogItems = items.GroupBy(x => x.RealSize.Value).Select(x => new SizeGroup(x.Key, _mapper.Map<List<CatalogItemDTO>>(x))).ToList(),
                        Photos = photos.ContainsKey(key.Articul) ? photos[key.Articul] : new List<ModelPhotoDTO>(),

                        InCategories = categories.ContainsKey(key.Articul)
                            ? _mapper.Map<List<Category1CDTO>>(categories[key.Articul])
                            : new List<Category1CDTO>(),

                        InCatalogs = catalogs.ContainsKey(key.Articul)
                            ? _mapper.Map<List<Catalog1CDTO>>(catalogs[key.Articul])
                            : items.GroupBy(
                                    x => new {Id = x.CatalogId, Name = x.CatalogId.ToProductCatalog1CId().AsString()},
                                    (catalogKey, _) => new Catalog1CDTO {Id = catalogKey.Id, Name = catalogKey.Name})
                                .ToList()
                    };

                    if (sourceItems.ContainsKey(key)) {
                        _mapper.Map(sourceItems[key], item);
                    }
                    
                    if (descriptors.ContainsKey(key.Articul)) {
                        item.Descriptor = _mapper.Map(descriptors[key.Articul], item.Descriptor);
                    }

                    return item;
                })
                .SortBy(articuls, x => x.Descriptor.Articul);

            return result.ToList();
        }

        private async Task<List<CatalogItemGroupNew>> _extendBriefCatalogItemsByArticulAsync(
            IReadOnlyCollection<CatalogItemFull> fullItems, ProductsFilterExtended filter) {
            var articuls = fullItems.Select(x => x.Articul).Distinct().ToList();
            var (descriptors, catalogs, categories, photos) = await CatalogItemDescriptors(filter, articuls);

            var result = fullItems.GroupBy(x => x.Articul, (key, groupedItems) => {
                    var items = groupedItems.ToList();

                    foreach (var catalogItem in items) {
                        if (string.IsNullOrEmpty(catalogItem.Size) || catalogItem.Size.Equals("0")) {
                            catalogItem.Size = catalogItem.SizeLine;
                        }
                    }

                    var item = new CatalogItemGroupNew {
                        Descriptor = new CatalogItemDescriptorDTO() {
                            Articul = key
                        },
                        CatalogId = items.First().CatalogId,
                        CommonPrice = items.Min(x => x.Price),
                        CommonPriceInRub = items.Min(x => x.PriceInRub),
                        Sizes = items.Select(x => x.RealSize.Value).Distinct().OrderBy(x => x, new SizeStringComparer()).ToList(),
                        Colors = _mapper.Map<List<ColorDTO>>(items).OrderBy(x => x.Name).Distinct().ToList(),
                        CatalogItems = items.GroupBy(x => x.RealSize.Value).Select(x => new SizeGroup(x.Key, _mapper.Map<List<CatalogItemDTO>>(x))).ToList(),
                        Photos = photos.ContainsKey(key) ? photos[key] : new List<ModelPhotoDTO>(),

                        InCategories = categories.ContainsKey(key)
                            ? _mapper.Map<List<Category1CDTO>>(categories[key])
                            : new List<Category1CDTO>(),

                        InCatalogs = catalogs.ContainsKey(key)
                            ? _mapper.Map<List<Catalog1CDTO>>(catalogs[key])
                            : items.GroupBy(
                                    x => new {Id = x.CatalogId, Name = x.CatalogId.ToProductCatalog1CId().AsString()},
                                    (catalogKey, _) => new Catalog1CDTO {Id = catalogKey.Id, Name = catalogKey.Name})
                                .ToList()
                    };

                    if (descriptors.ContainsKey(key)) {
                        item.Descriptor = _mapper.Map(descriptors[key], item.Descriptor);
                    }


                    return item;
                })
                .SortBy(articuls, x => x.Descriptor.Articul);

            return result.ToList();
        }

        private async Task<(Dictionary<string, CatalogItemDescriptor> modelsDescriptorsDict,
                Dictionary<string, List<ModelToCatalog1CMap>> modelsToCatalogsMap,
                Dictionary<string, List<ModelToCategory1CMap>> modelsToCategoriesMap,
                Dictionary<string, List<ModelPhotoDTO>> allPhotosList)>
            CatalogItemDescriptors(ProductsFilterExtended filter, IReadOnlyCollection<string> articuls) {
            var modelsDescriptorsDict = filter.IncludeItemsDescriptions
                ? await _uow.CatalogItemDescriptors.GetByArticulsAsDictionaryAsync(articuls)
                : new Dictionary<string, CatalogItemDescriptor>();

            var modelsToCatalogsMap = filter.IncludeCatalogsMap
                ? await _uow.CatalogItems.GetCatalogsByArticulAsync(filter.IncludeWithoutCatalog)
                : new Dictionary<string, List<ModelToCatalog1CMap>>();

            var modelsToCategoriesMap = filter.IncludeCategoriesMap
                ? await _uow.CatalogItems.GetCategoriesByArticulAsync()
                : new Dictionary<string, List<ModelToCategory1CMap>>();

            var allPhotosList = filter.IncludePhotosList
                ? _mapper.Map<List<ModelPhotoDTO>>(await _uow.ModelPhotos.GetAllAsync(articuls))
                : new List<ModelPhotoDTO>();

            var photos = allPhotosList.GroupBy(x => x.Articul).ToDictionary(x => x.Key, v => v.ToList());

            return (modelsDescriptorsDict, modelsToCatalogsMap, modelsToCategoriesMap, photos);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Session;
using KristaShop.Common.Models.Structs;
using Module.Catalogs.Business.Models;
using Module.Common.Business.Models;

namespace Module.Catalogs.Business.Interfaces {
    public interface ICatalogItemsService {
        Task<IEnumerable<CatalogItemBriefDTO>> GetCatalogTopModelItemsAsync(CatalogType catalogId, int quantity = 6);
        Task<IEnumerable<CatalogItemGroupNew>> GetFullModelsAllListAsync(ProductsFilterExtended filter, UserSession userSession, Page page);
        Task<Dictionary<CatalogType, List<CatalogItemGroupNew>>> GetFullModelsGroupedByCatalogsAsync(ProductsFilterExtended filter, UserSession userSession, int pageNum = 0, int pageSize = 25);
        Task<List<CatalogItemForAddDTO>> GetProductsListForAddAsync();
        Task<CatalogItemGroupNew> FindModelAsync(string articul, CatalogType catalogId, UserSession userSession, bool showEmptySlots = false);
        Task<CatalogItemGroupNew> FindModelForAdminAsync(string articul, int catalogId, UserSession userSession);
        Task<int> GetModelsCountAsync(ProductsFilterExtended filter, UserSession userSession = null);
        Task<IEnumerable<CatalogItemGroupNew>> GetFullModelsHistoryAsync();
        Task<CatalogItemGroupNew> FindHistoryModelAsync(string articul);
        Task<List<ModelPhotoDTO>> GetModelPhotosAsync(string articul);
        Task<(IEnumerable<LookUpItem<int, string>>, IEnumerable<string>, IEnumerable<string>)> GetFilterLookupListsAsync(CatalogType catalogId, bool showEmptySlots = false);
        IEnumerable<ColorDTO> GetAllColors(IEnumerable<CatalogItemGroupNew> itemsList);
        IEnumerable<string> GetAllSizes(IEnumerable<CatalogItemGroupNew> itemsList);
        Task<List<int>> GetModelCatalogsInvisibilityAsync(string articul);
        Task UpdateCatalogItemVisibility(string articul, int modelId, SizeValue size, int colorId, CatalogType catalogId, bool isVisible);
    }
}

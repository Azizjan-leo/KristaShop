using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.Business.Interfaces
{
    public interface IFavorite1CService {
        Task<IEnumerable<CatalogItemBriefDTO>> getUserFavoritesAsync(int userId);
        Task<OperationResult> DeleteFavoriteAsync(int userId, string articul, int catalogId);

        Task<OperationResult> AddOrDeleteFavoriteAsync(int userId, string articul, int catalogId);
        Task<bool> IsFavoriteAsync(string articul, int catalogId, int userId);
    }
}
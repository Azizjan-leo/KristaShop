using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using Module.Common.Business.Models;
using Module.Order.Business.Models;

namespace Module.Order.Business.Interfaces
{
    public interface ICartService {
        Task<CartItem1CDTO> GetCartItemByIdAsync(int cartId, int userId);
        Task<List<CartItem1CDTO>> GetCartItemsAsync(int userId);
        Task<int> GetCartTotalAmountAsync(int userId);
        Task<List<CartItem1CDTO>> GetCartsItemsGroupedAsync(ReportsFilter filter);
        Task<List<UserCartTotalsDTO>> GetCartTotalsGroupedByUsersAsync(ReportsFilter filter);
        Task<ReportTotalsDTO> GetCartsTotalsAsync(ReportsFilter filter);
        Task<List<int>> GetUserIdsWithFilledCarts();
        Task<List<string>> GetArticulsAsync();
        Task<List<LookUpItem<int, string>>> GetUsersAsync();
        
        Task<int> InsertOrUpdateCartItemAsync(CartItem1CDTO dto);
        Task RemoveCartItemByIdAsync(int cartId, int userId);
        Task<int> UpdateCartItemAmountByIdAsync(int cartId, int userId, int quantity);
        Task ClearUserCartAsync(int userId);
        Task ClearOldItemsAsync();
     
    }
}
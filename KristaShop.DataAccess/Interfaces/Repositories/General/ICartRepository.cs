using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Views;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface ICartRepository : IRepository<CartItem, int> {
        Task<IEnumerable<CartItem>> GetAllByUserIdAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int userId, int modelId, int colorId, int catalogId, string sizeValue);
        Task<CartItemSqlView?> GetDetailedCartItemByIdAsync(int itemId);
        Task<IEnumerable<CartItemSqlView>> GetGroupedAllItemsAsync(ReportsFilter filter, bool forGuest = false);
        Task<IEnumerable<ReportTotals>> GetTotalsByAllCartsAsync(ReportsFilter filter);
        Task<IEnumerable<UserCartTotals>> GetCartTotalsForAllUsersAsync(ReportsFilter filter);
        Task<int> GetTotalAmountAsync(int userId);
        Task<List<int>> GetUserIdsWithNotEmptyCarts();
        Task<List<string>> GetArticulsListInCartsAsync();
        Task<List<LookUpItem<int, string>>> GetUsersListInCartsAsync();
        public Task MoveCartItemsToOtherUserAsync(int fromUserId, int toUserId);
        public Task ClearUserCartAsync(int userId);
        Task ClearAllOldCartsAsync(DateTime beforeDate);
    }
}
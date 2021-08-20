using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Helpers;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Filters;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class CartRepository : Repository<CartItem, int>, ICartRepository {
        public CartRepository(DbContext context) : base(context) { }

        public async Task<IEnumerable<CartItem>> GetAllByUserIdAsync(int userId) {
            var result = await All.Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Color).ThenInclude(x => x.Group)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result
                .OrderBy(x => x.Articul)
                .ThenBy(x => x.SizeValue, new SizeStringComparer())
                .ThenBy(x => x.Color);
        }

        public async Task<CartItem?> GetCartItemAsync(int userId, int modelId, int colorId, int catalogId, string sizeValue) {
            return await All.Where(x =>
                x.UserId == userId &&
                x.CatalogId == catalogId.ToProductCatalog1CId() &&
                x.ModelId == modelId &&
                x.ColorId == colorId &&
                x.SizeValue == sizeValue
            ).FirstOrDefaultAsync();
        }

        public async Task<CartItemSqlView?> GetDetailedCartItemByIdAsync(int itemId) {
            return await All.Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Color).ThenInclude(x => x.Group)
                .Where(x => x.Id == itemId)
                .Select(x => new CartItemSqlView {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserFullName = x.User.FullName,
                    ManagerId = x.User.ManagerId ?? 0,
                    ManagerName = x.User.Manager.Name ?? "",
                    CityId = x.User.CityId ?? 0,
                    CityName = x.User.City.Name ?? "",
                    CatalogId = (int) x.CatalogId,
                    CatalogName = x.CatalogId.AsString(),
                    Articul = x.Articul,
                    ModelId = x.ModelId,
                    NomenclatureId = x.NomenclatureId,
                    MainPhoto = x.Model.Descriptor.MainPhoto ?? "",
                    ColorId = x.ColorId,
                    ColorName = x.Color.Name,
                    ColorPhoto = "",
                    ColorValue = x.Color.Group.Hex,
                    Price = x.Price,
                    PriceInRub = x.PriceInRub,
                    Amount = x.Amount,
                    PartsCount = x.Model.Parts,
                    CreatedDate = x.CreatedDate
                }).FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<CartItemSqlView>>
            GetGroupedAllItemsAsync(ReportsFilter filter, bool forGuest = false) {
            var query = All.Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Color).ThenInclude(x => x.Group)
                .Filter(filter);

            if (!forGuest) {
                query = query.Where(x => x.UserId > 0);
            }

            return await query
                .OrderBy(x => x.CatalogId)
                .ThenBy(x => x.Articul)
                .ThenBy(x => x.Color.Name)
                .ThenBy(x => x.SizeValue)
                .GroupBy(x => new {
                    x.UserId,
                    x.User.FullName,
                    x.User.ManagerId,
                    ManagerName = x.User.Manager.Name,
                    x.User.CityId,
                    CityName = x.User.City.Name,
                    CatalogId = x.CatalogId,
                    x.Articul,
                    x.ModelId,
                    x.NomenclatureId,
                    x.Model.Descriptor.MainPhoto,
                    x.Model.SizeLine,
                    x.Model.Parts,
                    x.SizeValue,
                    x.ColorId,
                    ColorName = x.Color.Name,
                    x.Color.Group.Hex,
                })
                .Select(x => new CartItemSqlView {
                    UserId = x.Key.UserId,
                    UserFullName = x.Key.FullName,
                    ManagerId = x.Key.ManagerId ?? 0,
                    ManagerName = x.Key.ManagerName ?? "",
                    CityId = x.Key.CityId ?? 0,
                    CityName = x.Key.CityName ?? "",
                    CatalogId = (int) x.Key.CatalogId,
                    CatalogName = x.Key.CatalogId.AsString(),
                    Articul = x.Key.Articul,
                    ModelId = x.Key.ModelId,
                    PartsCount = x.Key.Parts,
                    NomenclatureId = x.Key.NomenclatureId,
                    MainPhoto = x.Key.MainPhoto ?? "",
                    SizeValue = x.Key.SizeValue != "" ? x.Key.SizeValue : x.Key.SizeLine,
                    ColorId = x.Key.ColorId,
                    ColorName = x.Key.ColorName,
                    ColorPhoto = "",
                    ColorValue = x.Key.Hex,
                    Price = x.Average(c => c.Price),
                    PriceInRub = x.Average(c => c.PriceInRub),
                    Amount = x.Sum(c => c.Amount),
                })
                .ToListAsync();
        }
        
        public async Task<IEnumerable<ReportTotals>> GetTotalsByAllCartsAsync(ReportsFilter filter) {
            return await All.Filter(filter).GroupBy(x => x.CatalogId)
                .Select(x => new ReportTotals {
                    CatalogId = x.Key,
                    TotalAmount = x.Sum(c => c.Amount),
                    TotalPrice = x.Sum(c => c.Price * c.Amount),
                    TotalPriceInRub = x.Sum(c => c.PriceInRub * c.Amount),
                }).ToListAsync();
        }

        public async Task<IEnumerable<UserCartTotals>> GetCartTotalsForAllUsersAsync(ReportsFilter filter) {
            return await All.Filter(filter).GroupBy(x => new {
                    x.UserId,
                    x.User.FullName,
                    x.User.ManagerId,
                    ManagerName = x.User.Manager.Name,
                    x.User.CityId,
                    CityName = x.User.City.Name
                })
                .Select(x => new UserCartTotals {
                    UserId = x.Key.UserId,
                    UserFullName = x.Key.FullName,
                    ManagerId = x.Key.ManagerId ?? 0,
                    ManagerName = x.Key.ManagerName ?? "",
                    CityId = x.Key.CityId ?? 0,
                    CityName = x.Key.CityName ?? "",
                    TotalItemsCount = x.Sum(c => c.Amount),
                    TotalPrice = x.Sum(c => c.Amount * c.Price),
                    TotalPriceRub = x.Sum(c => c.Amount * c.PriceInRub)
                })
                .ToListAsync();
        }

        public Task<int> GetTotalAmountAsync(int userId) {
            return All.Where(x => x.UserId == userId).SumAsync(x => x.Amount);
        }

        public async Task<List<int>> GetUserIdsWithNotEmptyCarts() {
            return await All.Where(x => x.UserId != 0)
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetArticulsListInCartsAsync() {
            return await All.Select(x => x.Articul)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<LookUpItem<int, string>>> GetUsersListInCartsAsync() {
            return await All
                .Select(x => new LookUpItem<int, string>(x.UserId, x.User.FullName))
                .Distinct()
                .ToListAsync();
        }

        public async Task MoveCartItemsToOtherUserAsync(int fromUserId, int toUserId) {
            var items = await All.Where(x => x.UserId == fromUserId).ToListAsync();
            foreach (var item in items) {
                item.UserId = toUserId;
            }

            UpdateRange(items);
        }

        public async Task ClearUserCartAsync(int userId) {
            DeleteRange(await All.Where(x => x.UserId == userId).ToListAsync());
        }

        public async Task ClearAllOldCartsAsync(DateTime beforeDate) {
            if (beforeDate == default) {
                throw new ArgumentOutOfRangeException(nameof(beforeDate),
                    "Date should be set to delete cart items properly");
            }

            DeleteRange(await All.Where(x => x.CreatedDate < beforeDate).ToListAsync());
        }
    }
}
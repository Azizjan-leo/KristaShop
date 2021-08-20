using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Views.Partners;

namespace KristaShop.DataAccess.Interfaces.Repositories.Partners {
    public interface IPartnerStorehouseHistoryRepository : IRepository<PartnerStorehouseHistoryItem, Guid> {
        Task<IEnumerable<PartnerStorehouseHistoryItemSqlView>> GetHistoryItemsAsync(int? userId,
            DateTimeOffset date = default, MovementDirection movementDirection = MovementDirection.None,
            MovementType movementType = MovementType.None, bool isAmountPositive = false);

        Task<IEnumerable<PartnerStorehouseHistoryItemSqlView>> GetGroupedHistoryItems(int? userId,
            DateTimeOffset date = default, MovementDirection movementDirection = MovementDirection.None,
            MovementType movementType = MovementType.None, bool isAmountPositive = false);
    }
}

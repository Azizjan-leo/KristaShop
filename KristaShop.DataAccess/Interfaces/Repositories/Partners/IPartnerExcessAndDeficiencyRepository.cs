using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.Partners;

namespace KristaShop.DataAccess.Interfaces.Repositories.Partners {
    public interface IPartnerExcessAndDeficiencyRepository : IRepository<PartnerExcessAndDeficiencyHistoryItem, Guid> { }
}

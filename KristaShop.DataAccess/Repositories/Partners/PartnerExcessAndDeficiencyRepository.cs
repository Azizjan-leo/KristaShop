using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.Partners {
    public class PartnerExcessAndDeficiencyRepository : Repository<PartnerExcessAndDeficiencyHistoryItem, Guid>,
        IPartnerExcessAndDeficiencyRepository {
        public PartnerExcessAndDeficiencyRepository(DbContext context) : base(context) { }
    }
}

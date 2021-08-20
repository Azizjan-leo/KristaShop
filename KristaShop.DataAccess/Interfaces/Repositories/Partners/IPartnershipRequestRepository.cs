using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Views.Partners;

namespace KristaShop.DataAccess.Interfaces.Repositories.Partners {
    public interface IPartnershipRequestRepository : IRepository<PartnershipRequest, Guid> {
        Task<IEnumerable<PartnershipRequestSqlView>> GetRequests(bool approved = false);
    }
}

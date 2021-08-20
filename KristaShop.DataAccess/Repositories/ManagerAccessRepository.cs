using KristaShop.Common.Enums;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KristaShop.DataAccess.Repositories {
    public class ManagerAccessRepository : Repository<ManagerAccess, Guid>, IManagerAccessRepository<ManagerAccess, Guid> {
        public ManagerAccessRepository(DbContext context) : base(context) { }

        public IQueryable<int> GetManagerIdsAccessesFor(int managerId, ManagerAccessToType accessTo) {
            return All.Where(x => x.ManagerId == managerId && x.AccessTo == accessTo)
                .Select(x => x.AccessToManagerId);
        }
    }
}

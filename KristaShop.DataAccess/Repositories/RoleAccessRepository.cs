using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KristaShop.DataAccess.Repositories {
    public class RoleAccessRepository : Repository<RoleAccess, Guid>, IRoleAccessRepository {

        public RoleAccessRepository(DbContext context) : base(context) { }

        public async Task<Role> GetRoleAsync(Guid id) {
            return await Context.Set<Role>().FindAsync(id);
        }

        public async Task<bool> HasAccessAsync(Guid roleId, RouteValue routeValue) {
            return await All
              .AnyAsync(x => x.RoleId == roleId
                          && x.Area == routeValue.Area
                          && x.Controller == routeValue.Controller
                          && x.Action == routeValue.Action
                          && x.IsAccessGranted == true);
        }

        public async Task<List<RoleAccess>> HasAccessToRoutesAsync(Guid roleId, List<RouteValue> routeValues) {
            var routeValueKeys = routeValues.Select(x => x.ToKeyString());
            var result = await All
                .Where(x => x.RoleId == roleId
                        && x.IsAccessGranted == true
                        && routeValueKeys.Contains(x.Area + "." + x.Controller + "." + x.Action))
                .ToListAsync();
            return result;
        }
    }
}

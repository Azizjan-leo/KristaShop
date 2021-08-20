#nullable enable
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KristaShop.DataAccess.Interfaces.Repositories {
    public interface IRoleAccessRepository : IRepository<RoleAccess, Guid> {
        Task<bool> HasAccessAsync(Guid roleId, RouteValue routeValue);
        Task<List<RoleAccess>> HasAccessToRoutesAsync(Guid roleId, List<RouteValue> routeValues);
        Task<Role> GetRoleAsync(Guid id);
    }
}
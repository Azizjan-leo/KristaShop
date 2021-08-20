using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KristaShop.DataAccess.CacheRepositories {
    public class RoleAccessCacheRepository : CacheRepository<RoleAccess, Guid, IRoleAccessRepository>, IRoleAccessRepository {
        
        public RoleAccessCacheRepository(IMemoryCache memoryCache, IRoleAccessRepository repository) : base(memoryCache, repository) { }

        public async Task<Role> GetRoleAsync(Guid id) {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<Guid, Role> cachedEntry)) {
                if (cachedEntry.TryGetValue(id, out var entity)) {
                    return entity;
                }
            }

            return await Repository.GetRoleAsync(id);
        }

        public async Task<bool> HasAccessAsync(Guid roleId, RouteValue routeValue) {

            if (!MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<Guid, RoleAccess> cachedEntry)) {
                cachedEntry = (await Repository.GetAllAsync()).ToConcurrentDictionary(k => k.Id, v => v);
                MemoryCache.Set(GetKey(), cachedEntry, GetCacheOptions());
            }

            return cachedEntry.Values
               .Any(x => x.RoleId == roleId
                           && x.Area == routeValue.Area
                           && x.Controller == routeValue.Controller
                           && x.Action == routeValue.Action
                           && x.IsAccessGranted == true);
        }

        public async Task<List<RoleAccess>> HasAccessToRoutesAsync(Guid roleId, List<RouteValue> routeValues) {
            if (!MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<Guid, RoleAccess> cachedEntry)) {
                cachedEntry = (await Repository.GetAllAsync()).ToConcurrentDictionary(k => k.Id, v => v);
                MemoryCache.Set(GetKey(), cachedEntry, GetCacheOptions());
            }

            var routeValueKeys = routeValues.Select(x => x.ToKeyString());
            return cachedEntry.Values
                .Where(x => x.RoleId == roleId
                        && x.IsAccessGranted == true
                        && routeValueKeys.Contains(x.Area + "." + x.Controller + "." + x.Action))
                .ToList();
        }
    }
}

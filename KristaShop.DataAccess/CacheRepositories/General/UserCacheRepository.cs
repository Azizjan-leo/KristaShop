using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.Extensions.Caching.Memory;

namespace KristaShop.DataAccess.CacheRepositories.General {
    public class UserCacheRepository : CacheRepository<User, int, IUserRepository>, IUserRepository {
        public UserCacheRepository(IMemoryCache memoryCache, IUserRepository repository) : base(memoryCache, repository) { }
        
        public async Task<IEnumerable<User>> GetAllByManagersAsync(IEnumerable<int> managerIds) {
            if (!MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                cachedEntry = (await Repository.GetAllAsync()).ToConcurrentDictionary(k => k.Id, v => v);
                MemoryCache.Set(GetKey(), cachedEntry, GetCacheOptions());
            }

            return cachedEntry.Values.Where(x =>
                    !x.IsManager && x.ManagerId == null ||
                    (x.ManagerId != null && managerIds.Contains((int) x.ManagerId)))
                .ToList();
        }

        public async Task<IEnumerable<User>> GetAllManagersAsync() {
            if (!MemoryCache.TryGetValue(GetManagersKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                cachedEntry = (await Repository.GetAllManagersAsync()).ToConcurrentDictionary(k => k.Id, v => v);
                MemoryCache.Set(GetManagersKey(), cachedEntry, GetCacheOptions());
            }

            return cachedEntry.Values.ToList();
        }

        public async Task<User?> GetManagerAsync(int userId) {
            if (!MemoryCache.TryGetValue(GetManagersKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                cachedEntry = (await Repository.GetAllManagersAsync()).ToConcurrentDictionary(k => k.Id, v => v);
                MemoryCache.Set(GetManagersKey(), cachedEntry, GetCacheOptions());
            }

            return cachedEntry.ContainsKey(userId) ? cachedEntry[userId] : null;
        }

        public async Task<User?> GetByIdWithDataAsync(int userId) {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                if (cachedEntry.TryGetValue(userId, out var entity)) {
                    return entity;
                }
            }

            return await Repository.GetByIdWithDataAsync(userId);
        }

        public async Task<User?> GetByLoginAsync(string login) {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                var user = cachedEntry.FirstOrDefault(x => x.Value.Login.Equals(login)).Value;
                if (user != null) {
                    return user;
                }
            }

            return await Repository.GetByLoginAsync(login);
        }

        public async Task<bool> IsActiveAsync(int userId) {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                return cachedEntry.TryGetValue(userId, out _);
            }

            return await Repository.IsActiveAsync(userId);
        }

        public async Task<bool> IsAlreadyRegisteredAsync(string login, string phone = "") {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                return cachedEntry.Any(x => x.Value.Login.Equals(login) &&
                                       (string.IsNullOrEmpty(phone) || !string.IsNullOrEmpty(phone) && x.Value.Phone.Equals(phone)));
            }

            return await Repository.IsAlreadyRegisteredAsync(login, phone);
        }

        public async Task<IEnumerable<CatalogType>> GetUserAvailableCatalogsOrOpenCatalogAsync(int userId) {
            if (!MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                cachedEntry = (await Repository.GetAllAsync()).ToConcurrentDictionary(k => k.Id, v => v);
                MemoryCache.Set(GetKey(), cachedEntry, GetCacheOptions());
            }
            
            if (cachedEntry.TryGetValue(userId, out var entity))
                return entity.GetOnlyAvailableCatalogs();

            return new List<CatalogType> {CatalogType.Open};
        }

        public async Task<Dictionary<CatalogType, bool>> GetUserCatalogsAsync(int userId) {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                if (cachedEntry.TryGetValue(userId, out var entity)) {
                    return entity.GetAccessesToCatalogs();
                }
            }

            return await Repository.GetUserCatalogsAsync(userId);
        }

        public async Task<bool> HasAccessToCatalogAsync(int userId, CatalogType catalogId) {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                if (cachedEntry.TryGetValue(userId, out var entity)) {
                    return entity.HasAccessTo(catalogId);
                }
            }

            return await Repository.HasAccessToCatalogAsync(userId, catalogId);
        }

        public async Task SetCatalogVisibilityAsync(int userId, CatalogType catalogId, bool hasAccess) {
            await Repository.SetCatalogVisibilityAsync(userId, catalogId, hasAccess);

            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, User> cachedEntry)) {
                if (cachedEntry.TryGetValue(userId, out var entity)) {
                    entity.SetCatalogAccess(catalogId, hasAccess);
                } else {
                    MemoryCache.Remove(GetKey());
                }
            }
        }

        protected string GetManagersKey() {
            return GetKey("managers");
        }
    }
}
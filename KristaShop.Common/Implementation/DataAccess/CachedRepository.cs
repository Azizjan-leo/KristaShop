using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Interfaces.DataAccess;
using Microsoft.Extensions.Caching.Memory;

namespace KristaShop.Common.Implementation.DataAccess {
    public class CacheRepository<TEntity, TKey, TRepository> : IRepository<TEntity, TKey>
        where TEntity : class, IEntityBase<TKey>
        where TKey : notnull
        where TRepository : IRepository<TEntity, TKey> {
        private static readonly string RepositoryKey = $"{Guid.NewGuid()}_{typeof(TEntity).Name}";
        protected readonly IMemoryCache MemoryCache;
        protected readonly TRepository Repository;

        public CacheRepository(IMemoryCache memoryCache, TRepository repository) {
            MemoryCache = memoryCache;
            Repository = repository;
        }

        public IQueryable<TEntity> All => Repository.All;
        public IOrderedQueryable<TEntity> AllOrdered => Repository.AllOrdered;

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() {
            if (!MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                cachedEntry = (await Repository.GetAllAsync()).ToConcurrentDictionary(k => k.GetId(), v => v);
                MemoryCache.Set(GetKey(), cachedEntry, GetCacheOptions());
            }

            return cachedEntry.Values.ToList();
        }

        public IEnumerable<TEntity> GetAllFromCache() {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                return cachedEntry.Values.ToList();
            }

            return new List<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity, bool generateKey = false) {
            var savedEntity = await Repository.AddAsync(entity, generateKey);

            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                if (!cachedEntry.TryAdd(savedEntity.GetId(), entity)) {
                    MemoryCache.Remove(GetKey());
                }
            }

            return savedEntity;
        }

        public TEntity Update(TEntity entity) {
            var updatedEntity = Repository.Update(entity);
            
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                if (!cachedEntry.TryUpdate(updatedEntity.GetId(), updatedEntity, updatedEntity)) {
                    MemoryCache.Remove(GetKey());
                }
            }

            return updatedEntity;
        }

        public TEntity Delete(TEntity entity) {
            var deletedEntity = Repository.Delete(entity);
            
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                cachedEntry.TryRemove(deletedEntity.GetId(), out _);
            }

            return deletedEntity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities) {
            await Repository.AddRangeAsync(entities);
            
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                if (!_addRangeToCachedItems(cachedEntry, entities)) {
                    MemoryCache.Remove(GetKey());
                }
            }
        }

        public void UpdateRange(IEnumerable<TEntity> entities) {
            Repository.UpdateRange(entities);
            
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                if (!_updateCachedItems(cachedEntry, entities)) {
                    MemoryCache.Remove(GetKey());
                }
            }
        }

        public void DeleteRange(IEnumerable<TEntity> entities) {
            Repository.DeleteRange(entities);

            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                if (!_removeCachedItems(cachedEntry, entities)) {
                    MemoryCache.Remove(GetKey());
                }
            }
        }

        public async Task<TEntity?> GetByIdAsync(TKey id) {
            if (MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                if (cachedEntry.TryGetValue(id, out var entity)) {
                    return entity;
                }
            }

            return await Repository.GetByIdAsync(id);
        }

        public Task<TEntity?> DeleteAsync(TKey id) {
            var deletedItem = Repository.DeleteAsync(id);
            if (deletedItem != null && MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<TKey, TEntity> cachedEntry)) {
                cachedEntry.TryRemove(id, out _);
            }

            return deletedItem;
        }

        private bool _addRangeToCachedItems(ConcurrentDictionary<TKey, TEntity> dictionary, IEnumerable<TEntity> values) {
            foreach (var value in values) {
                if (!dictionary.TryAdd(value.GetId(), value)) return false;
            }

            return true;
        }

        private bool _updateCachedItems(ConcurrentDictionary<TKey, TEntity> dictionary, IEnumerable<TEntity> values) {
            foreach (var value in values) {
                if (!dictionary.TryUpdate(value.GetId(), value, value)) return false;
            }

            return true;
        }
        
        private bool _removeCachedItems(ConcurrentDictionary<TKey, TEntity> dictionary, IEnumerable<TEntity> values) {
            foreach (var value in values) {
                if (!dictionary.TryRemove(value.GetId(), out _)) return false;
            }

            return true;
        }
        
        protected string GetKey(string key = "all") {
            return $"{RepositoryKey}_{key}";
        }
        
        protected static MemoryCacheEntryOptions GetCacheOptions() {
            return new() {
                SlidingExpiration = TimeSpan.FromMinutes(30),
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60),
            };
        }
    }
}
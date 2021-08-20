using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.Extensions.Caching.Memory;

namespace KristaShop.DataAccess.CacheRepositories.General {
    public class ModelsCacheRepository : CacheRepository<Model, int, IModelsRepository>, IModelsRepository {
        public ModelsCacheRepository(IMemoryCache memoryCache, IModelsRepository repository) : base(memoryCache, repository) { }

        public async Task<IReadOnlyList<string>> GetAllSizesAsync() {
            if (!MemoryCache.TryGetValue(GetKey("sizes"), out IReadOnlyList<string> cachedEntry)) {
                cachedEntry = await Repository.GetAllSizesAsync();
                MemoryCache.Set(GetKey("sizes"), cachedEntry, GetCacheOptions());
            }

            return cachedEntry;
        }

        public async Task<IReadOnlyList<string>> GetAllArticulsAsync() {
            if (!MemoryCache.TryGetValue(GetKey("articuls"), out IReadOnlyList<string> cachedEntry)) {
                cachedEntry = await Repository.GetAllArticulsAsync();
                MemoryCache.Set(GetKey("articuls"), cachedEntry, GetCacheOptions());
            }

            return cachedEntry;
        }
    }
}
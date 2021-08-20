using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using KristaShop.DataAccess.Views.Partners;
using Microsoft.Extensions.Caching.Memory;

namespace KristaShop.DataAccess.CacheRepositories.Partners {
    public class PartnersCacheRepository : CacheRepository<Partner, int, IPartnersRepository>, IPartnersRepository {
        public PartnersCacheRepository(IMemoryCache memoryCache, IPartnersRepository repository) : base(memoryCache, repository) { }
        public async Task<List<PartnerSalesReportItem>> GetSalesReportItems(ReportsFilter filter) {
            return await Repository.GetSalesReportItems(filter);
        }

        public async Task<List<PartnerSqlView>> GetAllPartnersAsync(PartnersFilter filter) {
            return await Repository.GetAllPartnersAsync(filter);
        }

        public async Task<List<LookUpItem<int, string>>> GetPartnersLookUpAsync() {
            return await Repository.GetPartnersLookUpAsync();
        }

        public async Task<double> GetPartnerPaymentRateAsync(int userId) {
            if (!MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, Partner> cachedEntry)) {
                cachedEntry = (await Repository.GetAllAsync()).ToConcurrentDictionary(k => k.GetId(), v => v);
                MemoryCache.Set(GetKey(), cachedEntry, GetCacheOptions());
            }

            if (!cachedEntry.ContainsKey(userId)) {
                throw new EntityNotFoundException($"Partner {userId} not found");
            }

            return cachedEntry[userId].PaymentRate;
        }

        public async Task<bool> IsPartnerAsync(int userId) {
            if (!MemoryCache.TryGetValue(GetKey(), out ConcurrentDictionary<int, Partner> cachedEntry)) {
                cachedEntry = (await Repository.GetAllAsync()).ToConcurrentDictionary(k => k.GetId(), v => v);
                MemoryCache.Set(GetKey(), cachedEntry, GetCacheOptions());
            }

            return cachedEntry.ContainsKey(userId);
        }

        public async Task<List<DocumentItem>> GetDecryptedSalesReportItems(ReportsFilter filter) {
            return await Repository.GetDecryptedSalesReportItems(filter);
        }
    }
}
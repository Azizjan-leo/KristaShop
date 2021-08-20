using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using KristaShop.DataAccess.CacheRepositories.General;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Repositories.General;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Business.DataAccess.Repositories {
    public class UserCacheRepositoryTests {
        private KristaShopDbContext _context;
        private MemoryCache _memoryCache;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _context.Set<User>().RemoveRange(_context.Set<User>());
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }
        
        [Test]
        public async Task Should_CreateDifferentCacheKeys_When_UsersAndManagersAreCached() {
            var userCacheRepository = new UserCacheRepository(_memoryCache, new UserRepository(_context));
            await userCacheRepository.AddRangeAsync(new List<User> {
                new() {FullName = "Test 1"},
                new() {FullName = "Test 2"},
                new() {FullName = "Test 3", IsManager = true}
            });
            await _context.SaveChangesAsync();
            
            var actualUsers = await userCacheRepository.GetAllAsync();
            var actualManagers = await userCacheRepository.GetAllManagersAsync();

            var actualCacheKeys = _memoryCache.Count;

            actualUsers.Should().HaveCount(2);
            actualManagers.Should().HaveCount(1);
            actualCacheKeys.Should().Be(2);
        }
    }
}
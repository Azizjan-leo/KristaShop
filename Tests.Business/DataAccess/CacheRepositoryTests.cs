using System;
using System.Threading.Tasks;
using FluentAssertions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Business.DataAccess {
    public class CacheRepositoryTests {
        private KristaShopDbContext _context;
        private MemoryCache _memoryCache;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task Should_CreateDifferentCacheKey_When_RepositoryUsedForDifferentEntities() {
            var rolesRepository = new Repository<Role, Guid>(_context);
            var blogRepository = new Repository<BlogItem, Guid>(_context);
            await rolesRepository.AddAsync(new Role(Guid.NewGuid(), "Test role"));
            await blogRepository.AddAsync(new BlogItem {
                Id = Guid.NewGuid(),
                Title = "Test blog",
                Description = "",
                ImagePath = "",
                Link = "",
                LinkText = "",
            });
            await _context.SaveChangesAsync();

            var rolesCacheRepository = new CacheRepository<Role, Guid, Repository<Role, Guid>>(_memoryCache, rolesRepository);
            var blogCacheRepository = new CacheRepository<BlogItem, Guid, Repository<BlogItem, Guid>>(_memoryCache, blogRepository);
            var actualRoles = await rolesCacheRepository.GetAllAsync();
            var actualUserData = await blogCacheRepository.GetAllAsync();

            actualRoles.Should().HaveCount(1);
            actualUserData.Should().HaveCount(1);
            _memoryCache.Count.Should().Be(2);
        }
        
        [Test]
        public async Task Should_CreateSameCacheKey_When_RepositoryUsedSameEntity() {
            var rolesRepository = new Repository<Role, Guid>(_context);
            await rolesRepository.AddAsync(new Role(Guid.NewGuid(), "Test role"));
            await _context.SaveChangesAsync();
            
            var rolesCacheRepository = new CacheRepository<Role, Guid, Repository<Role, Guid>>(_memoryCache, rolesRepository);
            var rolesNewCacheRepository = new CacheRepository<Role, Guid, Repository<Role, Guid>>(_memoryCache, new Repository<Role, Guid>(_context));
            var actual = await rolesCacheRepository.GetAllAsync();
            var actualNew = await rolesNewCacheRepository.GetAllAsync();

            actual.Should().HaveCount(1);
            actualNew.Should().HaveCount(1);
            _memoryCache.Count.Should().Be(1);
        }
        
        [Test]
        public async Task Should_ClearWholeCache_When_CompactCalledOnCache() {
            var rolesRepository = new Repository<Role, Guid>(_context);
            var blogRepository = new Repository<BlogItem, Guid>(_context);
            await rolesRepository.AddAsync(new Role(Guid.NewGuid(), "Test role"));
            await blogRepository.AddAsync(new BlogItem {
                Id = Guid.NewGuid(),
                Title = "Test blog",
                Description = "",
                ImagePath = "",
                Link = "",
                LinkText = "",
            });
            await _context.SaveChangesAsync();

            var rolesCacheRepository = new CacheRepository<Role, Guid, Repository<Role, Guid>>(_memoryCache, rolesRepository);
            var blogCacheRepository = new CacheRepository<BlogItem, Guid, Repository<BlogItem, Guid>>(_memoryCache, blogRepository);
            await rolesCacheRepository.GetAllAsync();
            await blogCacheRepository.GetAllAsync();

            var actualCountInitial = _memoryCache.Count;
            _memoryCache.Compact(1.0);
            var actualCount = _memoryCache.Count;

            actualCountInitial.Should().Be(2);
            actualCount.Should().Be(0);
        }
    }
}
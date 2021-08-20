using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Business.DataAccess {
    public class CacheRepositorySingleEntityTests {
        private KristaShopDbContext _context;
        private MemoryCache _memoryCache;
        private Repository<Role, Guid> _basicRepository;
        private CacheRepository<Role, Guid, Repository<Role, Guid>> _cacheRepository;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _basicRepository = new Repository<Role, Guid>(_context);
            _cacheRepository = new CacheRepository<Role, Guid, Repository<Role, Guid>>(_memoryCache, _basicRepository);
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }
        
        [Test]
        public async Task Should_SaveDataToCache_When_GetEntitiesList() {
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Test"));
            await _context.SaveChangesAsync();

            var actualInCache = (await _cacheRepository.GetAllAsync()).Count(); 
            var actualCacheKeys = _memoryCache.Count;

            actualInCache.Should().Be(1);
            actualCacheKeys.Should().Be(1);
        }
        
        [Test]
        public async Task Should_ContainDataInCache_When_DataBeenCachedAndDataSourceWasCleaned() {
            var entity = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(entity);
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            
            _basicRepository.Delete(entity);
            await _context.SaveChangesAsync();
            
            var actualInDb = await _basicRepository.All.CountAsync();
            var actualInCache = _cacheRepository.GetAllFromCache().Count();
            var actualCacheKeys = _memoryCache.Count;
            
            actualInDb.Should().Be(0);
            actualInCache.Should().Be(1);
            actualCacheKeys.Should().Be(1);
        }

        [Test]
        public async Task Should_AddOnlyToDb_When_CacheEntryNotExist() {
            await _cacheRepository.AddAsync(new Role(Guid.NewGuid(), "Test"));
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().Be(1);
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_AddToDbAndCache_When_CacheEntryExist() {
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Test"));
            await _context.SaveChangesAsync();
            
            await _cacheRepository.GetAllAsync();
            await _cacheRepository.AddAsync(new Role(Guid.NewGuid(), "Test 2"));
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualInCache = _cacheRepository.GetAllFromCache().Count();
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().Be(2);
            actualInCache.Should().Be(2);
            actualCacheKeys.Should().Be(1);
        }

        [Test]
        public async Task Should_UpdateOnlyInDb_When_CacheEntryNotExist() {
            var role = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();

            role.Name = "Test 2";
            _cacheRepository.Update(role);
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.GetByIdAsync(role.Id);
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Name.Should().Be("Test 2");
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_UpdateInDbAndCache_When_CacheEntryExist() {
            var role = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();
            
            await _cacheRepository.GetAllAsync();
            role.Name = "Test 2";
            _cacheRepository.Update(role);
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.GetByIdAsync(role.Id);
            var actualInCache = _cacheRepository.GetAllFromCache().First();
            var actualCacheKeys = _memoryCache.Count;
            
            ReferenceEquals(actualInDb, actualInCache).Should().Be(true);
            actualInDb.Name.Should().Be(actualInCache.Name);
            actualCacheKeys.Should().Be(1);
        }

        [Test]
        public async Task Should_CleanCache_When_TryUpdateItemThatNotInTheCache() {
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Test"));
            await _context.SaveChangesAsync();
            await _cacheRepository.GetAllAsync();

            var actualCacheKeysBeforeUpdate = _memoryCache.Count;
            
            var role = new Role(Guid.NewGuid(), "Test 2");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();
            
            role.Name = "Test 3";
            _cacheRepository.Update(role);
            await _context.SaveChangesAsync();

            var actualCacheKeys = _memoryCache.Count;

            actualCacheKeysBeforeUpdate.Should().Be(1);
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_NotThrowException_When_DeleteEntityAndCacheEntryNotExist() {
            var role = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(role);
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Test 2"));
            await _context.SaveChangesAsync();
            
            _cacheRepository.Delete(role);
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualCacheKeys = _memoryCache.Count;
            
            actualInDb.Should().Be(1);
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_RemoveEntityFromCache_When_CacheEntryNotExist() {
            var role = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(role);
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Test 2"));
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            _cacheRepository.Delete(role);
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualInCache = _cacheRepository.GetAllFromCache().Count();
            
            actualInDb.Should().Be(1);
            actualInCache.Should().Be(1);
        }
        
        [Test]
        public async Task Should_NotThrowException_When_TryDeleteEntityThatExistInDbAndNotInCache() {
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Test"));
            await _context.SaveChangesAsync();
            await _cacheRepository.GetAllAsync();

            var role = new Role(Guid.NewGuid(), "Test 2");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();

            _cacheRepository.Delete(role);
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualCacheKeys = _memoryCache.Count; 
            
            actualInDb.Should().Be(1);
            actualCacheKeys.Should().Be(1);
        }
        
        [Test]
        public async Task Should_AddRangeOnlyToDb_When_CacheEntryNotExist() {
            await _cacheRepository.AddRangeAsync(_createRolesList());
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualCacheKeys = _memoryCache.Count; 
            
            actualInDb.Should().Be(3);
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_AddRangeToDbAndCache_When_CacheEntryExist() {
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Test"));
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            await _cacheRepository.AddRangeAsync(_createRolesList());
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualInCache = _cacheRepository.GetAllFromCache().Count();
            var actualCacheKeys = _memoryCache.Count; 
            
            actualInDb.Should().Be(4);
            actualInCache.Should().Be(4);
            actualCacheKeys.Should().Be(1);
        }
        
        [Test]
        public async Task Should_CleanCache_When_AddRangeToCacheFailed() {
            var role = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            await _cacheRepository.AddRangeAsync(new List<Role> {role});

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualCacheKeys = _memoryCache.Count; 
            
            actualInDb.Should().Be(1);
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_UpdateRangeOnlyInDb_When_CacheEntryNotExist() {
            var roles = _createRolesList();
            await _basicRepository.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            _cacheRepository.UpdateRange(_updatedRolesList(roles));
            await _context.SaveChangesAsync();
            
            var actualInDb = await _basicRepository.GetAllAsync();
            var actualCacheKeys = _memoryCache.Count; 
            
            actualInDb.Should().OnlyContain(x => x.Name.StartsWith("Updated"));
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_UpdateRangeInDbAndCache_When_CacheEntryExist() {
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Updated Test"));
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            var roles = _createRolesList();
            await _cacheRepository.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            _cacheRepository.UpdateRange(_updatedRolesList(roles));
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.ToListAsync();
            var actualInCache = _cacheRepository.GetAllFromCache();
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().OnlyContain(x => x.Name.StartsWith("Updated"));
            actualInCache.Should().OnlyContain(x => x.Name.StartsWith("Updated"));
            actualCacheKeys.Should().Be(1);
        }

        [Test]
        public async Task Should_CleanCache_When_UpdateRangeToCacheFailed() {
            await _basicRepository.AddAsync( new Role(Guid.NewGuid(), "Test 1"));
            await _context.SaveChangesAsync();
            await _cacheRepository.GetAllAsync();
            
            var role = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();

            role.Name = $"Updated {role.Name}";
            _cacheRepository.UpdateRange(new List<Role> {role});

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualCacheKeys = _memoryCache.Count;
            
            actualInDb.Should().Be(2);
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_DeleteRangeOnlyFromDb_When_CacheEntryNotExist() {
            var roles = _createRolesList();
            await _basicRepository.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            _cacheRepository.DeleteRange(roles.Skip(1));
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().Be(1);
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_DeleteRangeFromDbAndCache_When_CacheEntryExist() {
            var roles = _createRolesList();
            await _basicRepository.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            _cacheRepository.DeleteRange(roles.Skip(1));
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualInCache = _cacheRepository.GetAllFromCache().Count();
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().Be(1);
            actualInCache.Should().Be(1);
            actualCacheKeys.Should().Be(1);
        }
        
        [Test]
        public async Task Should_CleanCache_When_DeleteRangeForCacheFailed() {
            var roles = _createRolesList();
            await _basicRepository.AddRangeAsync(roles.Take(2));
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            _cacheRepository.DeleteRange(roles);

            var actualCacheKeys = _memoryCache.Count;
            actualCacheKeys.Should().Be(0);
        }

        [Test]
        public async Task Should_GetEntityFromDb_When_CacheEntryNotExist() {
            var role = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();

            var actualInDb = await _cacheRepository.GetByIdAsync(role.Id);
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().NotBeNull();
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_GetEntityFromCache_When_CacheEntryExist() {
            var role = new Role(Guid.NewGuid(), "Test");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            _basicRepository.Delete(role);
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.GetByIdAsync(role.Id);
            var actualInCache = await _cacheRepository.GetByIdAsync(role.Id);
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().BeNull();
            actualInCache.Should().NotBeNull();
            actualCacheKeys.Should().Be(1);
        }
        
        [Test]
        public async Task Should_GetEntityFromDb_When_GetEntityFromCacheFailed() {
            await _basicRepository.AddAsync(new Role(Guid.NewGuid(), "Test"));
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            var role = new Role(Guid.NewGuid(), "Test 2");
            await _basicRepository.AddAsync(role);
            await _context.SaveChangesAsync();

            var actualInDb = await _cacheRepository.GetByIdAsync(role.Id);
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().NotBeNull();
            actualCacheKeys.Should().Be(1);
        }
        
        [Test]
        public async Task Should_GetEntityById_When_ThereAreSeveralEntitiesWithDifferentIdsInCache() {
            var roles = _createRolesList();
            await _basicRepository.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();

            var actualInCache = await _cacheRepository.GetByIdAsync(roles.First().Id);
            var actualCacheKeys = _memoryCache.Count;

            actualInCache.Id.Should().Be(roles.First().Id);
            actualInCache.Name.Should().Be(roles.First().Name);
            actualCacheKeys.Should().Be(1);
        }
        
        [Test]
        public async Task Should_DeleteEntityByIdFromDb_When_CacheEntryNotExist() {
            var roles = _createRolesList();
            await _basicRepository.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            await _cacheRepository.DeleteAsync(roles.First().Id);
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().Be(2);
            actualCacheKeys.Should().Be(0);
        }
        
        [Test]
        public async Task Should_DeleteEntityByIdFromDbAndCache_When_CacheEntryNotExist() {
            var roles = _createRolesList();
            await _basicRepository.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            await _cacheRepository.DeleteAsync(roles.First().Id);
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualInCache = _cacheRepository.GetAllFromCache().Count();
            var actualCacheKeys = _memoryCache.Count;

            actualInDb.Should().Be(2);
            actualInCache.Should().Be(2);
            actualCacheKeys.Should().Be(1);
        }
        
        [Test]
        public async Task Should_ReturnNull_When_TryToDeleteNonExistingEntityFromDbAndCache() {
            var roles = _createRolesList();
            await _basicRepository.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            await _cacheRepository.GetAllAsync();
            var actualDeleted = await _cacheRepository.DeleteAsync(Guid.NewGuid());
            await _context.SaveChangesAsync();

            var actualInDb = await _basicRepository.All.CountAsync();
            var actualInCache = _cacheRepository.GetAllFromCache().Count();
            var actualCacheKeys = _memoryCache.Count;

            actualDeleted.Should().BeNull();
            actualInDb.Should().Be(3);
            actualInCache.Should().Be(3);
            actualCacheKeys.Should().Be(1);
        }
        
        private static List<Role> _createRolesList() {
            return new() {
                new Role(Guid.NewGuid(), "Test 1"),
                new Role(Guid.NewGuid(), "Test 2"),
                new Role(Guid.NewGuid(), "Test 3")
            };
        }

        private static List<Role> _updatedRolesList(List<Role> roles) {
            foreach (var role in roles) {
                role.Name = "Updated " + role.Name;
            }

            return roles;
        }
    }
}
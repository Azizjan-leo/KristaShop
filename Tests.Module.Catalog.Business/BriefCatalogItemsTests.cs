using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Module.Catalogs.Business.Models.Mappings;
using Module.Catalogs.Business.Services;
using Module.Catalogs.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;

namespace Tests.Module.Catalog.Business {
    [TestFixture]
    public class BriefCatalogItemsTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private EntityGenerator _entityGenerator;
        private CatalogItemsService _service;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null);
            _service = new CatalogItemsService(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>())));
            _entityGenerator = new EntityGenerator();
            await _initDbDataAsync();
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }

        private async Task _initDbDataAsync() {
            await _context.Set<Model>().AddRangeAsync(new List<Model> {
                _entityGenerator.CreateModel(1, "10-01", "42-44-46", 15),
                _entityGenerator.CreateModel(2, "20-01", "42-44-46", 15),
                _entityGenerator.CreateModel(3, "30-01", "42-44-46", 15),
                _entityGenerator.CreateModel(4, "40-01", "42-44-46", 15),
            });
            await _context.Set<Color>().AddRangeAsync(new List<Color> {
                _entityGenerator.CreateColorWithGroup(1, "Айвори"),
                _entityGenerator.CreateColorWithGroup(2, "Пудра")
            });
            await _uow.CatalogItemDescriptors.AddRangeAsync(new List<CatalogItemDescriptor> {
                _entityGenerator.CreateDescriptor("10-01"),
                _entityGenerator.CreateDescriptor("20-01"),
                _entityGenerator.CreateDescriptor("30-01"),
                _entityGenerator.CreateDescriptor("40-01"),
            });
            await _context.ModelCatalogOrder.AddRangeAsync(new List<ModelCatalogOrder> {
                _entityGenerator.CreateModelOrder("10-01", CatalogType.Open, 2),
                _entityGenerator.CreateModelOrder("20-01", CatalogType.Open, 1),
                _entityGenerator.CreateModelOrder("30-01", CatalogType.Open, 3),
                _entityGenerator.CreateModelOrder("40-01", CatalogType.Open, 4),
            });
            await _context.SaveChangesAsync();
        }
        
        [Test]
        public async Task Should_NotGetLastCatalogItem_When_GetTopThreeCatalogItems() {
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 3, 1, "10-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 4, 2, "20-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 6, 2, "20-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 7, 3, "30-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 9, 3, "30-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 10, 4, "40-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 12, 4, "40-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.SaveChangesAsync();

            var actual = await _service.GetCatalogTopModelItemsAsync(CatalogType.Open, 3);
            actual.Should().HaveCount(3);
        }

        [Test]
        public async Task Should_HaveItemsOrderedByOrderPosition_When_GetTopThreeCatalogItems() {
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 1, 1, "10-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 3, 1, "10-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 4, 2, "20-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 6, 2, "20-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 7, 3, "30-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 9, 3, "30-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 10, 4, "40-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 12, 4, "40-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.SaveChangesAsync();

            var actual = await _service.GetCatalogTopModelItemsAsync(CatalogType.Open, 3);
            actual.Should().BeInAscendingOrder(x => x.Order);
        }
        
        [Test]
        public async Task Should_GetCatalogItemsIfDescriptorNotExist_When_GetTopCatalogItems() {
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 1, 1, "10-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 3, 1, "10-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 4, 2, "20-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 6, 2, "20-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 7, 3, "30-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 9, 3, "30-01", "", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(2, 10, 4, "40-01", "42", 1, 3, CatalogType.Open, 30));
            await _uow.CatalogItems.AddRangeAsync(_entityGenerator.GenerateCatalogItemsEntity(1, 12, 4, "40-01", "", 1, 3, CatalogType.Open, 30));
            _uow.CatalogItemDescriptors.Delete(_uow.CatalogItemDescriptors.All.First(x => x.Articul == "20-01"));
            await _uow.SaveChangesAsync();

            var a = await _uow.CatalogItems.All.Include(x => x.Model).ThenInclude(x => x.Descriptor).ToListAsync();
            var actualList = await _service.GetCatalogTopModelItemsAsync(CatalogType.Open, 3);
            actualList.Should().HaveCount(3);
            actualList.Should().Contain(x => x.Articul.Equals("20-01"));
        }
    }
}
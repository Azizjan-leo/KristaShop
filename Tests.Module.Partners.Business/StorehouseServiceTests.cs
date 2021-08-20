using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.Extensions.Caching.Memory;
using Module.Partners.Business.DTOs.Mappings;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.Services;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using Tests.Module.Partners.Business.DataGenerators;

namespace Tests.Module.Partners.Business {
    [TestFixture]
    public partial class PartnerStorehouseServiceTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private IPartnerStorehouseService _partnerStorehouseService;
        private ModuleDtoItemsGenerator _generator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _partnerStorehouseService = new PartnerStorehouseService(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
            _generator = new ModuleDtoItemsGenerator();
            await _initDbData();
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }
        
        private async Task _initDbData() {
            _context.Set<Model>().RemoveRange(_context.Set<Model>());
            _context.Set<Barcode>().RemoveRange(_context.Set<Barcode>());
            _context.Set<Shipment>().RemoveRange(_context.Set<Shipment>());
            _context.Set<Color>().RemoveRange(_context.Set<Color>());

            var entityGenerator = new EntityGenerator();
            await _context.Set<Model>().AddRangeAsync(new List<Model> {
                entityGenerator.CreateModel(1, "10-01", "42-44-46", 15)
            });

            await _context.Set<Color>().AddRangeAsync(new List<Color> {
                entityGenerator.CreateColor(1, "Айвори")
            });

            await _context.Set<Barcode>().AddRangeAsync(entityGenerator.CreateBarcodesForLine(1, 1, 1, "42-44-46", 1));
            
            await _uow.Partners.AddRangeAsync(new List<Partner> {
                entityGenerator.CreatePartner(1),
                entityGenerator.CreatePartner(2),
            });

            await _context.Set<CatalogItemDescriptor>().AddAsync(entityGenerator.CreateDescriptor("10-01"));

            await _context.SaveChangesAsync();
        }
        
        [Test]
        public async Task Should_FindStorehouseItemByBarcode_When_ThereIsAnItemWithThisBarcodeInTheStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1));
            await _uow.SaveChangesAsync();

            var actual = await _partnerStorehouseService.GetStorehouseItemAsync("001", 1);
            actual.Should().NotBeNull();
            actual.Articul.Should().Be("10-01");
        }
        
        [Test]
        public async Task Should_NotFindStorehouseItemByBarcode_When_ThereIsNoItemWithThisBarcodeInTheStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1));
            await _uow.SaveChangesAsync();

            var actual = await _partnerStorehouseService.GetStorehouseItemAsync("004", 1);
            actual.Should().BeNull();
        }
        
        [Test]
        public async Task Should_FindStorehouseItemBySpecifiedUser_When_SeveralUsersHasItemsWithSameBarcode() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 5));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(2, 1, 1, "10-01", 42, 1, 1));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(3, 1, 1, "10-01", 42, 1, 3));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(4, 1, 1, "10-01", 42, 1, 8));
            await _uow.SaveChangesAsync();

            var actual = await _partnerStorehouseService.GetStorehouseItemAsync("001", 1);
            actual.UserId.Should().Be(1);
            actual.Amount.Should().Be(5);
        }
        
        [Test]
        public async Task Should_FindGroupedStorehouseItemBySpecifiedUser_When_UserHasSameItemsInTheStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 5));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(2, 1, 1, "10-01", 42, 1, 3));
            await _uow.SaveChangesAsync();

            var actual = await _partnerStorehouseService.GetStorehouseItemAsync("001", 1);
            actual.UserId.Should().Be(1);
            actual.Amount.Should().Be(6);
        }
        
        [Test]
        public async Task Should_ReturnEmptyList_When_UserHasNotStorehouseItems() {
            var actual = await _partnerStorehouseService.GetStorehouseItemsAsync(1);
            actual.Should().BeEmpty();
        }
        
        [Test]
        public async Task Should_ReturnEmptyList_When_CurrentUserHasNoStorehouseItemsButOtherUsersHave() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(2, 1, 1, "10-01", 42, 1, 5));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(2, 1, 1, "10-01", 42, 1, 1));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(3, 1, 1, "10-01", 42, 1, 3));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(4, 1, 1, "10-01", 42, 1, 8));
            await _uow.SaveChangesAsync();
            
            var actual = await _partnerStorehouseService.GetStorehouseItemsAsync(1);
            actual.Should().BeEmpty();
        }
        
        [Test]
        public async Task Should_ReturnGroupedUserStorehouseItems_When_UserHasSameItemsInTheStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 5));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 4));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(2, 1, 1, "10-01", 42, 1, 3));
            await _uow.SaveChangesAsync();
            
            var actual = await _partnerStorehouseService.GetStorehouseItemsAsync(1);
            actual.Should().NotBeEmpty();
            actual.Should().HaveCount(1);
            actual.First().Amount.Should().Be(10);
        }
        
        [Test]
        public async Task Should_NotGroupUserStorehouseItems_When_UserHasNoSameItemsInTheStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 5));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 44, 1, 5));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 46, 1, 5));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(2, 1, 1, "10-01", 42, 1, 3));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(3, 1, 1, "10-01", 42, 1, 8));
            await _uow.SaveChangesAsync();
            
            var actual = await _partnerStorehouseService.GetStorehouseItemsAsync(1);
            actual.Should().NotBeEmpty();
            actual.Should().HaveCount(3);
            actual.Sum(x => x.Amount).Should().Be(15);
        }
    }
}
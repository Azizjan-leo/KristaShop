using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Exceptions;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using Microsoft.Extensions.Caching.Memory;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.DTOs.Mappings;
using Module.Partners.Business.Services.Operations;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using Tests.Module.Partners.Business.DataGenerators;

namespace Tests.Module.Partners.Business.Operations {
    public class WriteOffSingleItemFromStorehouseTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private WriteOffSingleItemFromStorehouseOperation _writeOffFromStorehouseOperation;
        private ModuleDtoItemsGenerator _generator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _writeOffFromStorehouseOperation = new WriteOffSingleItemFromStorehouseOperation(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
            _generator = new ModuleDtoItemsGenerator();
            await _initDbData();
        }
        
        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }

        private async Task _initDbData() {
            var entityGenerator = new EntityGenerator();
            await _uow.Partners.AddAsync(entityGenerator.CreatePartner(1));
            await _context.Set<CatalogItemDescriptor>().AddAsync(entityGenerator.CreateDescriptor("10-01"));
            await _context.SaveChangesAsync();
        }

        [Test]
        public async Task Should_ThrowException_When_WriteOffNotExistentItem() {
            Func<Task> action = async () => {
                await _writeOffFromStorehouseOperation.HandleAsync(new SellingDTO {
                    ModelId = 1,
                    ColorId = 1,
                    SizeValue = "42",
                    UserId = 1
                });
            };
            await action.Should().ThrowAsync<StorehouseItemNotFound>();
        }
        
        [Test]
        public async Task Should_DecreaseStorehouseItemAmount_When_WriteOffExistingItem() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 3));
            await _uow.SaveChangesAsync();

            await _writeOffFromStorehouseOperation.HandleAsync(new SellingDTO {
                ModelId = 1,
                ColorId = 1,
                SizeValue = "42",
                UserId = 1
            });
            await _uow.SaveChangesAsync();

            var actualItems = await _uow.PartnerStorehouseItems.GetAllAsync();
            var actual = actualItems.First();

            actualItems.Should().HaveCount(1);
            actual.Amount.Should().Be(2);
        }
        
        [Test]
        public async Task Should_RemoveStorehouseItem_When_WriteOffExistingItemAndItemAmountChangesToZero() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 2, 1, "10-01", 42, 1, 1));
            await _uow.SaveChangesAsync();

            await _writeOffFromStorehouseOperation.HandleAsync(new SellingDTO {
                ModelId = 1,
                ColorId = 1,
                SizeValue = "42",
                UserId = 1
            });
            await _uow.SaveChangesAsync();

            var actualItems = await _uow.PartnerStorehouseItems.GetAllAsync();
            var actual = actualItems.First();

            actualItems.Should().HaveCount(1);
            actual.Amount.Should().Be(1);
        }
    }
}
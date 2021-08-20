using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Exceptions;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.DTOs.Mappings;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.Services;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using Tests.Module.Partners.Business.DataGenerators;

namespace Tests.Module.Partners.Business {
    public class PartnerStorehouseServiceSellingTests {
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

            await _context.Set<Barcode>().AddRangeAsync(entityGenerator.CreateBarcode(1, 1, 1, "42", "001"));
            await _context.Set<CatalogItemDescriptor>().AddAsync(entityGenerator.CreateDescriptor("10-01"));

            await _uow.Partners.AddRangeAsync(new List<Partner> {
                entityGenerator.CreatePartner(1),
                entityGenerator.CreatePartner(2),
            });
            
            await _context.SaveChangesAsync();
        }

        [Test]
        public async Task SellStorehouseItemAsync_Should_ThrowException_When_ThereIsNoRequestedItemInTheStorehouse() {
            Func<Task> action = async () => {
                await _partnerStorehouseService.SellStorehouseItemAsync(new SellingDTO {
                    ModelId = 1,
                    ColorId = 1,
                    SizeValue = "42",
                    UserId = 1
                });
            };
            await action.Should().ThrowAsync<StorehouseItemNotFound>();
        }

        [Test]
        public async Task SellStorehouseItemAsync_Should_SubtractOneFromAmount_When_ThereAreEnoughAmountForItemInTheStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 3));
            await _uow.SaveChangesAsync();

            await _partnerStorehouseService.SellStorehouseItemAsync(new SellingDTO {
                ModelId = 1,
                ColorId = 1,
                SizeValue = "42",
                UserId = 1
            });
            
            var actual = await _uow.PartnerStorehouseItems.GetStorehouseItemAsync("001", 1);
            actual.Amount.Should().Be(2);
        }

        [Test]
        public async Task SellStorehouseItemAsync_Should_RemoveItemFromStorehouse_WhenItsAmountEqualsZero() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 2, 1, "10-01", 42, 1, 1));
            await _uow.SaveChangesAsync();

            await _partnerStorehouseService.SellStorehouseItemAsync(new SellingDTO {
                ModelId = 1,
                ColorId = 1,
                SizeValue = "42",
                UserId = 1
            });
            var actual = await _uow.PartnerStorehouseItems.GetStorehouseItems(1).ToListAsync();

            actual.Should().HaveCount(1);
            actual.First().Amount.Should().Be(1);
        }
        
        [Test]
        public async Task SellStorehouseItemAsync_Should_ThrowException_When_ThereIsNoRequestedItemInTheCurrentUserStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 5));
            await _uow.SaveChangesAsync();

            Func<Task> action = async () => {
                await _partnerStorehouseService.SellStorehouseItemAsync(new SellingDTO {
                    ModelId = 1,
                    ColorId = 1,
                    SizeValue = "42",
                    UserId = 2
                });
            };
            var actual = await _uow.PartnerStorehouseItems.GetStorehouseItemAsync("001", 1);
            
            await action.Should().ThrowAsync<StorehouseItemNotFound>();
            actual.Amount.Should().Be(5);
        }
        
        [Test]
        public async Task SellStorehouseItemAsync_Should_SaveHistoryItem_When_SuccessfullySaleItem() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 2, 1, "10-01", 42, 1, 5));
            await _uow.SaveChangesAsync();

            await _partnerStorehouseService.SellStorehouseItemAsync(new SellingDTO {
                ModelId = 1,
                ColorId = 1,
                SizeValue = "42",
                UserId = 1
            });
            
            await _partnerStorehouseService.SellStorehouseItemAsync(new SellingDTO {
                ModelId = 1,
                ColorId = 1,
                SizeValue = "42",
                UserId = 1
            });

            var actual = (await _uow.PartnerStorehouseHistoryItems.GetHistoryItemsAsync(1)).ToList();
            
            actual.Should().HaveCount(2);
            actual.Should().OnlyContain(x => x.UserId == 1);
            actual.Should().OnlyContain(x => x.Amount == -1);
            actual.Should().OnlyContain(x => x.ModelId == 1 && x.ColorId == 1 && x.Size.Value == "42");
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Structs;
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
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Tests.Module.Partners.Business {
    [TestFixture]
    public class PartnerStorehouseServiceAuditTests {
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
        public async Task Should_AddItemsToStorehouse_When_IncomeExcess() {
            await _partnerStorehouseService.AuditStorehouseItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 2), new("002", 2), new("003", 2)}
            });

            var a = await _uow.PartnerStorehouseItems.GetAllAsync();
            var actual = await _partnerStorehouseService.GetStorehouseItemsAsync(1);
            
            actual.Should().HaveCount(3);
            actual.Should().OnlyContain(x => x.Amount == 2);
        }
        
        [Test]
        public async Task Should_AddItemToStorehouseHistory_When_IncomeExcess() {
            await _partnerStorehouseService.AuditStorehouseItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 2), new("002", 2), new("003", 2)}
            });

            var actual = await _uow.PartnerStorehouseHistoryItems.GetHistoryItemsAsync(1);
            
            actual.Should().HaveCount(3);
            actual.Should().OnlyContain(x => x.Amount == 2);
            actual.Should().OnlyContain(x => x.Direction == MovementDirection.In && x.MovementType == MovementType.IncomeAudit);
        }

        [Test]
        public async Task Should_RemoveItemsFromStorehouse_When_WriteOffDeficiency() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntityFromLine(1, 1, 1, "10-01", new SizeValue("42-44-46"), 1, 3));
            await _uow.SaveChangesAsync();
            
            await _partnerStorehouseService.AuditStorehouseItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO>()
            });

            var actual = await _partnerStorehouseService.GetStorehouseItemsAsync(1);
            actual.Should().HaveCount(0);
        }

        [Test]
        public async Task Should_AddItemToStorehouseHistory_When_WriteOffDeficiency() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntityFromLine(1, 1, 1, "10-01", new SizeValue("42-44-46"), 1, 3));
            await _uow.SaveChangesAsync();
            
            await _partnerStorehouseService.AuditStorehouseItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO>()
            });

            var actual = await _uow.PartnerStorehouseHistoryItems.GetHistoryItemsAsync(1);
            
            actual.Should().HaveCount(3);
            actual.Should().OnlyContain(x => x.Amount == -3);
            actual.Should().OnlyContain(x => x.Direction == MovementDirection.Out && x.MovementType == MovementType.WriteOffAudit);
        }

        [Test]
        public async Task Should_WriteOffItemAmountFromStorehouse_When_WriteOffDeficiency() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntityFromLine(1, 2, 1, "10-01", new SizeValue("42-44-46"), 1, 4));
            await _uow.SaveChangesAsync();

            await _partnerStorehouseService.AuditStorehouseItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 3)}
            });

            var actual = await _partnerStorehouseService.GetStorehouseItemsAsync(1);
            actual.Should().HaveCount(1);
            actual.First().Amount.Should().Be(3);
        }
        
        [Test]
        public async Task Should_AddItemsToStorehouseHistory_When_WriteOffDeficiency() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntityFromLine(1, 2, 1, "10-01", new SizeValue("42-44-46"), 1, 4));
            await _uow.SaveChangesAsync();

            await _partnerStorehouseService.AuditStorehouseItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 3)}
            });

            var actual = await _uow.PartnerStorehouseHistoryItems.GetHistoryItemsAsync(1);
            
            actual.Should().HaveCount(3, "Because storehouse items are grouped before selected");
        }
    }
}
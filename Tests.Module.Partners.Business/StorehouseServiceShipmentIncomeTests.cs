using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models.DTOs;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.Extensions.Caching.Memory;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.DTOs.Mappings;
using Module.Partners.Business.Services;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Tests.Module.Partners.Business {
    [TestFixture]
    public partial class PartnerStorehouseServiceShipmentIncomeTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private PartnerStorehouseService _partnerStorehouseService;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _partnerStorehouseService = new PartnerStorehouseService(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));

            await _initDbData();
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

            await _uow.Shipments.AddRangeAsync(new List<Shipment> {
                entityGenerator.CreateShipment(1, 1, 1, 1, "42", 3, 15),
                entityGenerator.CreateShipment(2, 1, 1, 1, "44", 3, 15),
                entityGenerator.CreateShipment(3, 2, 1, 1, "44", 1, 15),
                entityGenerator.CreateShipment(4, 2, 1, 1, " ", 2, 15),
            });

            await _uow.Partners.AddRangeAsync(new List<Partner> {
                entityGenerator.CreatePartner(1),
                entityGenerator.CreatePartner(2),
            });
            
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }
        
        [Test]
        public async Task Should_ThrowException_When_ShipmentNotFoundForProvidedDate() {
            Func<Task> action = async () => {
                await _partnerStorehouseService.IncomeShipmentItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                    UserId = 1,
                    ReservationDate = DateTime.Today.AddDays(-1)
                });
            };
            await action.Should().ThrowAsync<ShipmentNotFoundException>();
        }

        [Test]
        public async Task Should_CreateIncomeDocument_When_IncomeExistingShipment() {
            await _partnerStorehouseService.IncomeShipmentItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 3), new("002", 3)},
                ReservationDate = DateTime.Today
            });

            var actual = await _uow.PartnerDocuments.GetAllAsync();
            actual.Should().HaveCount(1);
            actual.First().Should().BeOfType<IncomeDocument>();
        }
        
        [Test]
        public async Task Should_AddItemsToTheStorehouse_When_IncomeExistingShipment() {
            await _partnerStorehouseService.IncomeShipmentItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 3), new("002", 3)},
                ReservationDate = DateTime.Today
            });

            var actual = (await _uow.PartnerStorehouseItems.GetAllAsync()).ToList();

            actual.Should().HaveCount(2);
            actual.Should().OnlyContain(x => x.UserId == 1);
            actual.Should().OnlyContain(x => x.Amount > 0);
        }
        
        [Test]
        public async Task Should_AddItemsToTheStorehouseHistory_When_IncomeExistingShipment() {
            await _partnerStorehouseService.IncomeShipmentItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 3), new("002", 3)},
                ReservationDate = DateTime.Today
            });

            var actual = (await _uow.PartnerStorehouseHistoryItems.GetAllAsync()).ToList();

            actual.Should().HaveCount(2);
            actual.Should().OnlyContain(x => x.UserId == 1);
            actual.Should().OnlyContain(x => x.MovementType == MovementType.Income && x.Direction == MovementDirection.In);
            actual.Should().OnlyContain(x => x.Amount > 0);
        }
        
        [Test]
        public async Task Should_CreateIncomeDocument_When_IncomeExistingShipmentWithLineItems() {
            await _partnerStorehouseService.IncomeShipmentItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 2,
                Items = new List<BarcodeAmountDTO> {new("004", 2), new("002", 1)},
                ReservationDate = DateTime.Today
            });

            var actual = await _uow.PartnerDocuments.GetAllAsync();
            actual.Should().HaveCount(1);
            actual.First().Should().BeOfType<IncomeDocument>();
        }
        
        [Test]
        public async Task Should_AddItemsToTheStorehouse_When_IncomeExistingShipmentWithLineItems() {
            await _partnerStorehouseService.IncomeShipmentItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 2,
                Items = new List<BarcodeAmountDTO> {new("004", 2), new("002", 1)},
                ReservationDate = DateTime.Today
            });

            var actual = (await _uow.PartnerStorehouseItems.GetAllAsync()).ToList();

            actual.Should().HaveCount(4);
            actual.Should().OnlyContain(x => x.UserId == 2);
            actual.Should().OnlyContain(x => x.Amount > 0);
        }
        
        [Test]
        public async Task Should_AddItemsToTheStorehouseHistory_When_IncomeExistingShipmentWithLineItems() {
            await _partnerStorehouseService.IncomeShipmentItemsAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 2,
                Items = new List<BarcodeAmountDTO> {new("004", 2), new("002", 1)},
                ReservationDate = DateTime.Today
            });

            var actual = (await _uow.PartnerStorehouseHistoryItems.GetAllAsync()).ToList();

            actual.Should().HaveCount(4);
            actual.Should().OnlyContain(x => x.UserId == 2);
            actual.Should().OnlyContain(x => x.MovementType == MovementType.Income && x.Direction == MovementDirection.In);
            actual.Should().OnlyContain(x => x.Amount > 0);
        }
        
        [Test]
        public async Task AutoIncomeShipmentItemsAsync_Should_AddItemsToTheStorehouse_When_IncomeExistingShipment() {
            await _partnerStorehouseService.AutoIncomeShipmentItemsAsync(1, DateTime.Today);

            var actual = await _partnerStorehouseService.GetStorehouseItemsAsync(1);
            var actualDocument = _uow.PartnerDocuments.All.First();
            
            actualDocument.Should().BeOfType<IncomeDocument>();
            actual.Should().HaveCount(2);
            actual.Sum(x => x.Amount).Should().Be(6);
        }
        
        [Test]
        public async Task AutoIncomeShipmentItemsAsync_Should_ThrowException_When_IncomeNonExistingShipment() {
            Func<Task> action = async () => { await _partnerStorehouseService.AutoIncomeShipmentItemsAsync(1, DateTime.Today.AddDays(-1)); };
            await action.Should().ThrowAsync<ShipmentNotFoundException>();
        }
    }
}
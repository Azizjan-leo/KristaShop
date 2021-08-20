using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models.DTOs;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.Extensions.Caching.Memory;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Services.Operations;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Tests.Module.Partners.Business.Operations {
    [TestFixture]
    public class CreateIncomeListTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings
                .AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
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

            await _uow.Shipments.AddRangeAsync(new List<Shipment> {
                entityGenerator.CreateShipment(1, 1, 1, 1, "42", 3, 15),
                entityGenerator.CreateShipment(2, 1, 1, 1, "44", 3, 15),
                entityGenerator.CreateShipment(3, 2, 1, 1, " ", 2, 15),
            });

            await _uow.Partners.AddRangeAsync(new List<Partner> {
                entityGenerator.CreatePartner(1),
                entityGenerator.CreatePartner(2),
            });
            
            await _context.SaveChangesAsync();
        }

        [Test]
        public async Task Should_ThrowException_When_ShipmentNotFoundForProvidedDate() {
            var operation = new CreateIncomeListOperation(_uow);

            Func<Task> action = async () => {
                await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                    UserId = 1,
                    ReservationDate = DateTime.Today.AddDays(-1)
                });
            };
            await action.Should().ThrowAsync<ShipmentNotFoundException>();
        }

        [Test]
        public async Task Should_ThrowException_When_ShipmentNotFoundForProvidedUserId() {
            var operation = new CreateIncomeListOperation(_uow);

            Func<Task> action = async () => {
                await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                    UserId = 3,
                    ReservationDate = DateTime.Today
                });
            };
            await action.Should().ThrowAsync<ShipmentNotFoundException>();
        }

        [Test]
        public async Task Should_CreateDocumentModel_When_ValidDataProvided() {
            var operation = new CreateIncomeListOperation(_uow);
            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 3), new("002", 3)},
                ReservationDate = DateTime.Today
            });

            var actual = operation.Result;

            actual.UserId.Should().Be(1);
            actual.Date.Should().Be(DateTime.Today);
            actual.Items.Should().HaveCount(2);
            actual.ExcessItems.Should().HaveCount(0);
            actual.DeficiencyItems.Should().HaveCount(0);
        }

        [Test]
        public async Task Should_CreateDocumentModelWithDeficiency_When_ProvidedLessItemsToIncomeThenInTheShipment() {
            var operation = new CreateIncomeListOperation(_uow);
            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 1), new("002", 3)},
                ReservationDate = DateTime.Today
            });

            var actual = operation.Result;

            actual.UserId.Should().Be(1);
            actual.Date.Should().Be(DateTime.Today);
            actual.Items.Should().HaveCount(2);
            actual.ExcessItems.Should().HaveCount(0);
            actual.DeficiencyItems.Should().HaveCount(1);
            actual.DeficiencyItems.Sum(x => x.Amount).Should().Be(2);
        }

        [Test]
        public async Task Should_CreateDocumentModelWithExcess_When_ProvidedMoreItemsToIncomeThenInTheShipment() {
            var operation = new CreateIncomeListOperation(_uow);
            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> {new("001", 4), new("002", 3)},
                ReservationDate = DateTime.Today
            });

            var actual = operation.Result;

            actual.UserId.Should().Be(1);
            actual.Date.Should().Be(DateTime.Today);
            actual.Items.Should().HaveCount(2);
            actual.ExcessItems.Should().HaveCount(1);
            actual.ExcessItems.Sum(x => x.Amount).Should().Be(1);
            actual.DeficiencyItems.Should().HaveCount(0);
        }

        [Test]
        public async Task Should_UnCombineItemsAndCreateDocumentModel_When_ValidDataProvided() {
            var operation = new CreateIncomeListOperation(_uow);
            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 2,
                Items = new List<BarcodeAmountDTO> {new("004", 2)},
                ReservationDate = DateTime.Today
            });

            var actual = operation.Result;

            actual.UserId.Should().Be(2);
            actual.Date.Should().Be(DateTime.Today);
            actual.Items.Should().HaveCount(3);
            actual.Items.Should().OnlyContain(x => x.Amount == 2);
            actual.ExcessItems.Should().HaveCount(0);
            actual.DeficiencyItems.Should().HaveCount(0);
        }

        [Test]
        public async Task Should_UnCombineItemsAndCreateDocumentModel_When_ValidDataProvided2() {
            var operation = new CreateIncomeListOperation(_uow);
            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 2,
                Items = new List<BarcodeAmountDTO> {new("001", 2), new("002", 2), new("003", 2)},
                ReservationDate = DateTime.Today
            });

            var actual = operation.Result;

            actual.UserId.Should().Be(2);
            actual.Date.Should().Be(DateTime.Today);
            actual.Items.Should().HaveCount(3);
            actual.Items.Should().OnlyContain(x => x.Amount == 2);
            actual.ExcessItems.Should().HaveCount(0);
            actual.DeficiencyItems.Should().HaveCount(0);
        }
    }
}
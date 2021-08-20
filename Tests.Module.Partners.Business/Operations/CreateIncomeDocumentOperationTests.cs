using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.DTOs.Mappings;
using Module.Partners.Business.Services.Operations;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Module.Partners.Business.DataGenerators;

namespace Tests.Module.Partners.Business.Operations {
    public class CreateIncomeDocumentOperationTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private CreateIncomeDocumentOperation _createIncomeDocumentOperation;
        private ModuleDtoItemsGenerator _generator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _createIncomeDocumentOperation = new CreateIncomeDocumentOperation(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
            _generator = new ModuleDtoItemsGenerator();
        }
        
        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }
        
        [Test]
        public async Task Should_SaveIncomeDocumentToDb_When_CreateIncomeDocument() {
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO());
            await _uow.SaveChangesAsync();
            var actual = _createIncomeDocumentOperation.Result;
            
            actual.Should().NotBeNull();
            actual.Should().BeOfType<IncomeDocument>();
        }
        
        [Test]
        public async Task Should_CreateIncomeDocumentForUser_When_CreateIncomeDocumentWithUserId() {
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {UserId = 1});
            await _uow.SaveChangesAsync();
            var actual = _createIncomeDocumentOperation.Result;
            
            actual.UserId.Should().Be(1);
        }
        
        [Test]
        public async Task Should_GenerateDocumentNumber_When_CreateIncomeDocument() {
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO());
            await _uow.SaveChangesAsync();
            var actual = _createIncomeDocumentOperation.Result;
            
            actual.Number.Should().Be(1);
        }
        
        [Test]
        public async Task Should_ContainShipmentDate_When_CreateIncomeDocument() {
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO { Date = new DateTime(2021, 05, 10)});
            await _uow.SaveChangesAsync();
            var actual = _createIncomeDocumentOperation.Result;
        
            actual.ShipmentDate.Should().Be(new DateTimeOffset(2021, 05, 10, 0, 0, 0, TimeSpan.Zero));
        }
        
        [Test]
        public async Task Should_ContainItems_When_CreateIncomeDocumentWithItems() {
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var actual = _createIncomeDocumentOperation.Result;
        
            actual.Items.Count.Should().Be(1);
        }
        
         
        [Test]
        public async Task Should_SaveItemsWithParentDocument_When_CreateIncomeDocumentWithItems() {
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var documentId = _createIncomeDocumentOperation.Result.Id;
            
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(2, "20-02", 50)
            });
            await _uow.SaveChangesAsync();

            var actual = await _uow.PartnerDocumentItems.All.Where(x => x.DocumentId == documentId).ToListAsync();
        
            actual.Count.Should().Be(3);
            actual.All(x => x.Articul.Equals("10-01")).Should().Be(true, "Select items only for this documents");
        }
        
        [Test]
        public async Task Should_HaveInDirection_When_CreateIncomeDocument() {
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {UserId = 1});
            await _uow.SaveChangesAsync();
            var actual = _createIncomeDocumentOperation.Result;
        
            actual.UserId.Should().Be(1);
            actual.Direction.Should().Be(MovementDirection.In);
        }
    }
}
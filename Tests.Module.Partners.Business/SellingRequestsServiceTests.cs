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
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.DTOs.Mappings;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.Services;
using Module.Partners.Business.Services.Operations;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using Tests.Module.Partners.Business.DataGenerators;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Tests.Module.Partners.Business {
    [TestFixture]
    public class SellingRequestsServiceTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private ISellingRequestsService _service;
        private ModuleDtoItemsGenerator _generator;
        private CreateSellingRequestDocumentOperation _createSellingRequestDocumentOperation;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _service = new SellingRequestsService(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
            _createSellingRequestDocumentOperation = new CreateSellingRequestDocumentOperation(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
            _generator = new ModuleDtoItemsGenerator();
            await _initDbData();
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }

        private async Task _initDbData() {
            var entityGenerator = new EntityGenerator();
            
            await _context.Set<Model>().AddRangeAsync(new List<Model> {
                entityGenerator.CreateModel(1, "10-01", "42-44-46", 15),
                entityGenerator.CreateModel(2, "20-01", "42-44-46", 15),
            });

            await _context.Set<Color>().AddRangeAsync(new List<Color> {
                entityGenerator.CreateColor(1, "Айвори")
            });

            await _context.Set<Barcode>().AddRangeAsync(entityGenerator.CreateBarcodesForLine(1, 1, 1, "42-44-46", 1));
            await _context.Set<Barcode>().AddRangeAsync(entityGenerator.CreateBarcodesForLine(5, 2, 1, "42-44-46", 1));

            await _uow.Partners.AddRangeAsync(new List<Partner> {
                entityGenerator.CreatePartner(1),
                entityGenerator.CreatePartner(2),
            });
            
            await _context.SaveChangesAsync();
        }

        [Test]
        public async Task Should_CreateDocument_When_CreateSellingRequestAndUserHasRequestedStorehouseItems() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 2, 1, "10-01", 42, 1, 1, 30));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1, 15));
            await _uow.SaveChangesAsync();
            
            await _service.CreateSellingRequestAsync(1, new List<BarcodeAmountDTO> {
                new("001", 3)
            });

            var actual = await _uow.PartnerDocuments.GetDocumentsAsync(1);
            
            actual.Should().HaveCount(1);
            actual.First().Should().BeOfType<SellingRequestDocument>();
        }
        
        [Test]
        public async Task Should_ThrowException_When_TryUpdateNonExistentSellingRequest() {
            Func<Task> action = async () => {
                await _service.UpdateSellingRequestStatusAsync(Guid.NewGuid());
            };
            await action.Should().ThrowAsync<DocumentNotFoundException>();
        }
        
        [Test]
        public async Task Should_SetApproveAwaitStatus_When_UpdateSellingRequestStatus() {
            await _createSellingRequestDocumentOperation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42)
            });
            var document = _createSellingRequestDocumentOperation.Result;
            await _uow.SaveChangesAsync();
            
            await _service.UpdateSellingRequestStatusAsync(document.Id);
            
            var actual = await _uow.PartnerDocuments.GetDocumentsAsync(1);
            actual.Should().HaveCount(1);
            actual.First().State.Should().Be(State.ApproveAwait);
        }
        
        [Test]
        public async Task Should_SetApprovedStatus_When_UpdateSellingRequestStatus() {
            await _createSellingRequestDocumentOperation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42)
            });
            var document = _createSellingRequestDocumentOperation.Result;
            document.State = State.ApproveAwait;
            await _uow.SaveChangesAsync();
            await _service.UpdateSellingRequestStatusAsync(document.Id);
            
            var actual = await _uow.PartnerDocuments.GetDocumentsAsync(1);
            actual.Should().HaveCount(1);
            actual.First().State.Should().Be(State.Approved);
        }
    }
}
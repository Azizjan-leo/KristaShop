using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Exceptions;
using KristaShop.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
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
    [TestFixture]
    public class CreateSellingRequestDocumentOperationTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private CreateSellingRequestDocumentOperation _operation;
        private ModuleDtoItemsGenerator _generator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _operation = new CreateSellingRequestDocumentOperation(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
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
            await _context.SaveChangesAsync();
        }
        
        [Test]
        public async Task Should_SaveSellingDocumentToDb_When_CreateSellingRequestDocument() {
            Func<Task> action = async () => {
                await _operation.HandleAsync(new DocumentEditDTO {UserId = 1});
            };

            await action.Should().ThrowAsync<DocumentItemsException>();
        }
        
        [Test]
        public async Task Should_CreateSellingRequestDocumentForUser_When_CreateSellingRequestDocumentWithUserId() {
            await _operation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var actual = _operation.Result;
            
            actual.UserId.Should().Be(1);
        }
        
        [Test]
        public async Task Should_GenerateDocumentNumber_When_CreateSellingRequestDocument() {
            await _operation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var actual = _operation.Result;
            
            actual.Number.Should().Be(1);
        }
        
        [Test]
        public async Task Should_ContainItems_When_CreateSellingRequestDocumentWithItems() {
            await _operation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var actual = _operation.Result;
            
            actual.Items.Count.Should().Be(1);
        }
        
        [Test]
        public async Task Should_SaveItemsWithParentDocument_When_CreateSellingRequestDocumentWithItems() {
            await _operation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var documentId = _operation.Result.Id;
            
            await _operation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(2, "20-02", 50)
            });
            await _uow.SaveChangesAsync();

            var actual = await _uow.PartnerDocumentItems.All.Where(x => x.DocumentId == documentId).ToListAsync();
        
            actual.Count.Should().Be(3);
            actual.All(x => x.Articul.Equals("10-01")).Should().Be(true, "Select items only for this documents");
        }
    }
}
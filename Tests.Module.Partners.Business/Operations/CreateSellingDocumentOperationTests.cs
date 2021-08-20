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
using Tests.Common.DataGenerators;
using Tests.Module.Partners.Business.DataGenerators;

namespace Tests.Module.Partners.Business.Operations {
    public class CreateSellingDocumentTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private CreateSellingDocumentOperation _createSellingDocumentOperation;
        private ModuleDtoItemsGenerator _generator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _createSellingDocumentOperation = new CreateSellingDocumentOperation(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
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
        public async Task Should_SaveSellingDocumentToDb_When_CreateSellingDocument() {
            await _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {UserId = 1});
            var actual = _createSellingDocumentOperation.Result;
            await _uow.SaveChangesAsync();

            actual.Should().NotBeNull();
            actual.Should().BeOfType<SellingDocument>();
        }
        
        [Test]
        public async Task Should_CreateSellingDocumentForUser_When_CreateSellingDocumentWithUserId() {
            await _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {UserId = 1});
            await _uow.SaveChangesAsync();
            var actual = _createSellingDocumentOperation.Result;
            
            actual.UserId.Should().Be(1);
        }
        
        [Test]
        public async Task Should_GenerateDocumentNumber_When_CreateSellingDocument() {
            await _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {UserId = 1});
            await _uow.SaveChangesAsync();
            var actual = _createSellingDocumentOperation.Result;
            
            actual.Number.Should().Be(2);
        }
          
        [Test]
        public async Task Should_ContainItems_When_CreateSellingDocumentWithItems() {
            await _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var actual = _createSellingDocumentOperation.Result;
            
            actual.Items.Count.Should().Be(1);
        }
        
        [Test]
        public async Task Should_SaveItemsWithParentDocument_When_CreateSellingDocumentWithItems() {
            await _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var documentId = _createSellingDocumentOperation.Result.Id;
            
            await _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(2, "20-02", 50)
            });
            await _uow.SaveChangesAsync();

            var actual = await _uow.PartnerDocumentItems.All.Where(x => x.DocumentId == documentId).ToListAsync();
        
            actual.Count.Should().Be(3);
            actual.All(x => x.Articul.Equals("10-01")).Should().Be(true, "Select items only for this documents");
        }
        
        [Test]
        public async Task Should_CreatePaymentDocument_When_CreateSellingDocumentAndNotPaidPaymentDocumentNotExist() {
            await _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
        
            var actualDocuments = await _uow.PartnerDocuments.GetAllAsync();
            var actualPaymentDocument = await _uow.PartnerDocuments.GetLastNotPaidPaymentDocumentAsync(1);
        
            actualDocuments.Should().HaveCount(2);
            actualDocuments.Should().Contain(x => x.Items.Count > 0);
            actualPaymentDocument.Should().NotBeNull();
            actualPaymentDocument!.UserId.Should().Be(1);
        }
        
        [Test]
        public async Task Should_AddItemsToExistingPaymentDocument_When_CreateSellingDocumentAndNotPaidPaymentDocumentExist() {
            await _uow.PartnerDocuments.AddAsync(new PaymentDocument(1, 15, _generator.CreateDocumentItems(3, "10-02", 42), State.Paid));
            await _uow.SaveChangesAsync();
            
            await _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
        
            var actualDocuments = await _uow.PartnerDocuments.GetAllAsync();
            var actualPaymentDocument = await _uow.PartnerDocuments.GetLastNotPaidPaymentDocumentAsync(1);
        
            actualDocuments.Should().HaveCount(2);
            actualDocuments.Should().Contain(x => x.Items.Count > 0);
            actualPaymentDocument.Should().NotBeNull();
            actualPaymentDocument!.UserId.Should().Be(1);
            actualPaymentDocument.Items.Should().HaveCount(6);
        }
        
        [Test]
        public async Task Should_CreateOnlyOnePaymentDocument_When_RunParallelCreateSellingDocumentsAndNotPaidPaymentDocumentNotExist() {
            var context2 = ContextManager.DuplicateContextWithNewConnection(_context);
            var uow2 = new UnitOfWork(context2, new MemoryCache(new MemoryCacheOptions()), null, null);
            var createSellingDocument2 = new CreateSellingDocumentOperation(uow2, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));

            Parallel.Invoke(
                () => {
                    _createSellingDocumentOperation.HandleAsync(new DocumentEditDTO {
                        UserId = 1,
                        Items = _generator.CreateDocumentItemsDto(3, "10-01", 42)
                    }).Wait();
                    _uow.SaveChangesAsync().Wait();
                },
                () => {
                    createSellingDocument2.HandleAsync(new DocumentEditDTO {
                        UserId = 1,
                        Items = _generator.CreateDocumentItemsDto(3, "10-01", 42)
                    }).Wait();
                    uow2.SaveChangesAsync().Wait();
                }
            );
        
            var actualDocuments = await _uow.PartnerDocuments.All.Include(x => x.Items).ToListAsync();
            var actualPaymentDocument = actualDocuments.First(x => x is PaymentDocument);
        
            actualDocuments.Should().HaveCount(3);
            actualDocuments.Should().Contain(x => x.Items.Count > 0);
            actualPaymentDocument.Should().NotBeNull();
            actualPaymentDocument!.UserId.Should().Be(1);
            actualPaymentDocument.Items.Should().HaveCount(6);
        }
    }
}
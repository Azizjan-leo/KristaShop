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
    public class CreateRevisionDocumentTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private ModuleDtoItemsGenerator _generator;
        private CreateRevisionDocumentOperation _createRevisionDocumentOperation;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _createRevisionDocumentOperation = new CreateRevisionDocumentOperation(_uow,
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
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
        public async Task Should_SaveRevisionDocumentToDb_When_CreateRevisionDocument() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO());
            await _uow.SaveChangesAsync();
            
            var actual = _createRevisionDocumentOperation.Result;
            
            actual.Should().NotBeNull();
            actual.Should().BeOfType<RevisionDocument>();
        }
        
        [Test]
        public async Task Should_CreateRevisionDocumentForUser_When_CreateRevisionDocumentWithUserId() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {UserId = 1});
            await _uow.SaveChangesAsync();
            
            var actual = _createRevisionDocumentOperation.Result;

            actual.UserId.Should().Be(1);
        }
        
        [Test]
        public async Task Should_GenerateDocumentNumber_When_CreateRevisionDocument() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO());
            await _uow.SaveChangesAsync();
            
            var actual = _createRevisionDocumentOperation.Result;
            
            actual.Number.Should().Be(1);
        }
        
        [Test]
        public async Task Should_ContainItems_When_CreateRevisionDocumentWithItems() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            
            var actual = _createRevisionDocumentOperation.Result;

            actual.Items.Count.Should().Be(1);
        }
        
        [Test]
        public async Task Should_SaveItemsWithParentDocument_When_CreateRevisionDocumentWithItems() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {
                UserId = 1,
                Items = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var documentId = _createRevisionDocumentOperation.Result.Id;
            
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(2, "20-02", 50)
            });
            await _uow.SaveChangesAsync();
        
            var actual = await _uow.PartnerDocumentItems.All.Where(x => x.DocumentId == documentId).ToListAsync();
        
            actual.Count.Should().Be(3);
            actual.All(x => x.Articul.Equals("10-01")).Should().Be(true, "Select items only for this documents");
        }
        
        [Test]
        public async Task Should_CreateRevisionExcessDocument_When_CreateRevisionDocumentWithExcessItems() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {
                UserId = 1,
                ExcessItems = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var documentId = _createRevisionDocumentOperation.Result.Id;

            var actual = await _uow.PartnerDocuments.All.Include(x => x.Children).FirstAsync(x => x.Id == documentId);
            actual.Children.Count.Should().Be(1);
            actual.Children.First().Should().BeOfType<RevisionExcessDocument>();
        }
        
        [Test]
        public async Task Should_CreateRevisionDeficiencyDocument_When_CreateRevisionDocumentWithDeficiencyItems() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {
                UserId = 1,
                DeficiencyItems = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var documentId = _createRevisionDocumentOperation.Result.Id;
        
            var actual = await _uow.PartnerDocuments.All.Include(x => x.Children).FirstAsync(x => x.Id == documentId);
            actual.Children.Count.Should().Be(1);
            actual.Children.First().Should().BeOfType<RevisionDeficiencyDocument>();
        }
        
        [Test]
        public async Task Should_CreateRevisionDeficiencyAndExcessDocument_When_CreateRevisionDocumentWithDeficiencyAndExcessItems() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {
                UserId = 1,
                DeficiencyItems = _generator.CreateDocumentItemsDto(3, "10-01", 42),
                ExcessItems = _generator.CreateDocumentItemsDto(2, "20-01", 50)
            });
            await _uow.SaveChangesAsync();
            var documentId = _createRevisionDocumentOperation.Result.Id;

            var actual = await _uow.PartnerDocuments.All.Include(x => x.Children).FirstAsync(x => x.Id == documentId);
            var actualTypes = actual.Children.Select(x => x.GetType()).ToList();
            
            actual.Children.Count.Should().Be(2);
            actualTypes.Should().Contain(typeof(RevisionDeficiencyDocument));
            actualTypes.Should().Contain(typeof(RevisionExcessDocument));
        }
        
        [Test]
        public async Task Should_CreatePaymentDocument_When_CreateRevisionDeficiencyDocumentAndNotPaidPaymentDocumentNotExist() {
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {
                UserId = 1,
                DeficiencyItems = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            
            var actualDocuments = await _uow.PartnerDocuments.GetAllAsync();
            var actualPaymentDocument = await _uow.PartnerDocuments.GetLastNotPaidPaymentDocumentAsync(1);
        
            actualDocuments.Should().HaveCount(3);
            actualDocuments.Should().Contain(x => x.Items.Count > 0);
            actualPaymentDocument.Should().NotBeNull();
            actualPaymentDocument!.UserId.Should().Be(1);
            actualPaymentDocument.Items.Should().HaveCount(3);
        }
        
        [Test]
        public async Task Should_AddItemsToExistingPaymentDocument_When_CreateRevisionDocumentAndNotPaidPaymentDocumentExist() {
            await _uow.PartnerDocuments.AddAsync(new PaymentDocument(1, 15, _generator.CreateDocumentItems(3, "10-02", 42), State.NotPaid));
            await _uow.SaveChangesAsync();
            
            await _createRevisionDocumentOperation.HandleAsync(new RevisionDocumentEditDTO {
                UserId = 1,
                DeficiencyItems = _generator.CreateDocumentItemsDto(3, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
        
            var actualDocuments = await _uow.PartnerDocuments.GetAllAsync();
            var actualPaymentDocument = await _uow.PartnerDocuments.GetLastNotPaidPaymentDocumentAsync(1);
        
            actualDocuments.Should().HaveCount(3);
            actualDocuments.Should().Contain(x => x.Items.Count > 0);
            actualPaymentDocument.Should().NotBeNull();
            actualPaymentDocument!.UserId.Should().Be(1);
            actualPaymentDocument.Items.Should().HaveCount(6);
        }
    }
}
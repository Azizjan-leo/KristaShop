using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
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

namespace Tests.Module.Partners.Business {
    public class AllDocumentsTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private ModuleDtoItemsGenerator _generator;
        private CreateIncomeDocumentOperation _createIncomeDocumentOperation;
        private EntityGenerator _entityGenerator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _createIncomeDocumentOperation = new CreateIncomeDocumentOperation(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
            _generator = new ModuleDtoItemsGenerator();
            _entityGenerator = new EntityGenerator();
            
            _context.Set<Model>().RemoveRange(_context.Set<Model>());
            _context.Set<Collection>().RemoveRange(_context.Set<Collection>());
            await _uow.SaveChangesAsync();
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task Should_HaveModelAndCollection_When_IncludeModelWithCollection() {
            await _context.Set<Collection>().AddAsync(_entityGenerator.CreateCollection(1, "1", 30));
            await _context.Set<Model>().AddAsync(_entityGenerator.CreateModel(1, "10-01", "42-44-46", 15, 1));
            await _uow.SaveChangesAsync();
            
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(1, 1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var id = _createIncomeDocumentOperation.Result.Id;

            var actual = await _context.Set<DocumentItem>()
                .Where(x => x.DocumentId == id)
                .Include(x => x.Model).ThenInclude(x => x.Collection)
                .FirstAsync();

            actual.Model.Should().NotBeNull();
            actual.Model.Id.Should().Be(1);
            actual.Model.Articul.Should().Be("10-01");
            actual.Model.Collection.Should().NotBeNull();
            actual.Model.Collection.Id.Should().Be(1);
        }

        [Test]
        public async Task Should_HaveModelAndNotHaveCollection_When_IncludeModelWithoutCollection() {
            await _context.Set<Collection>().AddAsync(_entityGenerator.CreateCollection(1, "1", 30));
            await _context.Set<Model>().AddAsync(_entityGenerator.CreateModel(1, "10-01", "42-44-46", 15));
            await _uow.SaveChangesAsync();
            
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(1, 1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var id = _createIncomeDocumentOperation.Result.Id;

            var actual = await _context.Set<DocumentItem>()
                .Where(x => x.DocumentId == id)
                .Include(x => x.Model).ThenInclude(x => x.Collection)
                .FirstAsync();
            
            actual.Model.Should().NotBeNull();
            actual.Model.Id.Should().Be(1);
            actual.Model.Articul.Should().Be("10-01");
            actual.Model.Collection.Should().BeNull();
        }
        
        [Test]
        public async Task Should_HaveDescriptor_When_IncludeModelDescriptor() {
            await _context.Set<Model>().AddAsync(_entityGenerator.CreateModel(1, "10-01", "42-44-46", 15));
            await _context.Set<CatalogItemDescriptor>().AddAsync(_entityGenerator.CreateDescriptor("10-01"));
            await _uow.SaveChangesAsync();

            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(1, 1, "10-01", 42)
            });
            await _uow.SaveChangesAsync();
            var id = _createIncomeDocumentOperation.Result.Id;

            var actual = await _context.Set<DocumentItem>()
                .Where(x => x.DocumentId == id)
                .Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .FirstAsync();
            
            actual.Model.Should().NotBeNull();
            actual.Model.Id.Should().Be(1);
            actual.Model.Articul.Should().Be("10-01");
            actual.Model.Descriptor.Should().NotBeNull();
            actual.Model.Descriptor.Articul.Should().Be("10-01");
        }

        [Test]
        public async Task Should_HaveColorAndColorGroup_When_IncludeColorAndColorGroup() {
            await _context.Set<Model>().AddAsync(_entityGenerator.CreateModel(1, "10-01", "42-44-46", 15));
            await _context.Set<Color>().AddAsync(_entityGenerator.CreateColorWithGroup(1, "Айвори"));
            await _uow.SaveChangesAsync();
            
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42, 1)
            });
            await _uow.SaveChangesAsync();
            var id = _createIncomeDocumentOperation.Result.Id;
            
            var actual = await _context.Set<DocumentItem>()
                .Where(x => x.DocumentId == id)
                .Include(x => x.Color).ThenInclude(x => x.Group)
                .FirstAsync();

            actual.Color.Should().NotBeNull();
            actual.Color.Id.Should().Be(1);
            actual.Color.Group.Should().NotBeNull();
            actual.Color.Group.Id.Should().Be(1);
        }
        
        [Test]
        public async Task Should_HaveColorAndNotHaveColorGroup_When_IncludeColorWithoutColorGroup() {
            await _context.Set<Model>().AddAsync(_entityGenerator.CreateModel(1, "10-01", "42-44-46", 15));
            await _context.Set<Color>().AddAsync(_entityGenerator.CreateColor(1, "Айвори"));
            await _uow.SaveChangesAsync();
            
            await _createIncomeDocumentOperation.HandleAsync(new IncomeDocumentEditDTO {
                Items = _generator.CreateDocumentItemsDto(1, "10-01", 42, 1)
            });
            await _uow.SaveChangesAsync();
            var id = _createIncomeDocumentOperation.Result.Id;

            var actual = await _context.Set<DocumentItem>()
                .Where(x => x.DocumentId == id)
                .Include(x => x.Color).ThenInclude(x => x.Group)
                .FirstAsync();

            actual.Color.Should().NotBeNull();
            actual.Color.Id.Should().Be(1);
            actual.Color.Group.Should().BeNull();
        }
    }
}
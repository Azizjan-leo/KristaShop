using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using KristaShop.DataAccess.Domain;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Module.Partners.Business {
    public class DocumentNumberGeneratorTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, null, null, null);
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task Should_GenerateOne_When_CallGenerateForFirstTime() {
            var actual = await _uow.PartnerDocumentSequence.NextAsync();
            actual.Should().Be(1);
        }

        [Test]
        public async Task Should_IncrementForOne_When_CallGenerateSequentially() {
            var actual = new List<ulong> {
                await _uow.PartnerDocumentSequence.NextAsync(),
                await _uow.PartnerDocumentSequence.NextAsync(),
                await _uow.PartnerDocumentSequence.NextAsync()
            };
            
            var expected = new List<ulong> {1, 2, 3};

            actual.Should().Equal(expected).And.OnlyHaveUniqueItems();
        }
        
        [Test]
        public async Task Should_IncrementForOne_When_CallGenerateSequentiallyInSingleTransaction() {
            var actual = new List<ulong>();
            await using (await _uow.BeginTransactionAsync()) {
                actual.Add(await _uow.PartnerDocumentSequence.NextAsync());
                actual.Add(await _uow.PartnerDocumentSequence.NextAsync());
                actual.Add(await _uow.PartnerDocumentSequence.NextAsync());
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }
            
            var expected = new List<ulong> {1, 2, 3};

            actual.Should().Equal(expected).And.OnlyHaveUniqueItems();
        }
    }
}
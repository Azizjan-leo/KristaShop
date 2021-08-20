using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.Extensions.Caching.Memory;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.DTOs.Mappings;
using Module.Partners.Business.Services.Operations;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using Tests.Module.Partners.Business.DataGenerators;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Tests.Module.Partners.Business.Operations {
    public class CreateRevisionListTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private ModuleDtoItemsGenerator _generator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>()));
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
        public async Task Should_GroupItemsAndCreateDocumentModelWithoutExcessAndDeficiency_When_RealItemsAreSameAsDbItems() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 3));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 44, 1, 3));
            await _uow.SaveChangesAsync();
            
            var operation = new CreateRevisionListOperation(_uow, _mapper);

            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> { new("001", 1), new("001", 1), new("001", 1), new("002", 3) }
            });

            var actual = operation.Result;
            actual.Should().NotBeNull();
            actual.Items.Should().HaveCount(2);
            actual.Items.Should().OnlyContain(x => x.Amount == 3);
            actual.DeficiencyItems.Should().HaveCount(0);
            actual.ExcessItems.Should().HaveCount(0);
        }
        
        [Test]
        public async Task Should_GroupDbItemsAndCreateDocumentModelWithoutExcessAndDeficiency_When_RealItemsAreSameAsDbItems() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 44, 1, 3));
            await _uow.SaveChangesAsync();
            
            var operation = new CreateRevisionListOperation(_uow, _mapper);

            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> { new("001", 3), new("002", 3) }
            });

            var actual = operation.Result;
            actual.Should().NotBeNull();
            actual.Items.Should().HaveCount(2);
            actual.Items.Should().OnlyContain(x => x.Amount == 3);
            actual.DeficiencyItems.Should().HaveCount(0);
            actual.ExcessItems.Should().HaveCount(0);
        }
        
        [Test]
        public async Task Should_CreateDocumentModelWithoutExcessAndDeficiency_When_RealItemsAreSameAsDbItems() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 3));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 44, 1, 3));
            await _uow.SaveChangesAsync();
            
            var operation = new CreateRevisionListOperation(_uow, _mapper);

            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> { new("001", 3), new("002", 3) }
            });

            var actual = operation.Result;
            actual.Should().NotBeNull();
            actual.Items.Should().HaveCount(2);
            actual.Items.Should().OnlyContain(x => x.Amount == 3);
            actual.DeficiencyItems.Should().HaveCount(0);
            actual.ExcessItems.Should().HaveCount(0);
        }

        [Test]
        public async Task Should_ThrowException_When_RevisionInputContainsLines() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntityFromLine(1, 2, 1, "10-01", new SizeValue("42-44-46"), 1, 1));
            await _uow.SaveChangesAsync();

            var operation = new CreateRevisionListOperation(_uow, _mapper);

            Func<Task> action = async () => {
                await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                    UserId = 1,
                    Items = new List<BarcodeAmountDTO> {new("001", 1), new("002", 1), new("003", 1), new("004", 1)}
                });
            };

            action.Should().Throw<ExceptionBase>();
        }

        [Test]
        public async Task Should_CreateDocumentWithExcess_When_RealItemsHasMoreAmountThenDbItems() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 3, 1, "10-01", 42, 1, 1));
            await _uow.SaveChangesAsync();
            
            var operation = new CreateRevisionListOperation(_uow, _mapper);

            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> { new("001", 3), new("002", 3) }
            });

            var actual = operation.Result;
            actual.Should().NotBeNull();
            actual.Items.Should().HaveCount(2);
            actual.Items.Should().OnlyContain(x => x.Amount == 3);
            actual.DeficiencyItems.Should().HaveCount(0);
            actual.ExcessItems.Should().HaveCount(1);
            actual.ExcessItems.Should().OnlyContain(x => x.Amount == 3);
        }
        
        [Test]
        public async Task Should_CreateDocumentWithExcess_When_NoDbItems() {
            var operation = new CreateRevisionListOperation(_uow, _mapper);

            await operation.HandleAsync(new global::Module.Partners.Business.DTOs.BarcodeAmountDTO {
                UserId = 1,
                Items = new List<BarcodeAmountDTO> { new("001", 3), new("002", 3) }
            });

            var actual = operation.Result;
            actual.Should().NotBeNull();
            actual.Items.Should().HaveCount(2);
            actual.Items.Should().OnlyContain(x => x.Amount == 3);
            actual.DeficiencyItems.Should().HaveCount(0);
            actual.ExcessItems.Should().HaveCount(2);
            actual.ExcessItems.Should().OnlyContain(x => x.Amount == 3);
        }
    }
}
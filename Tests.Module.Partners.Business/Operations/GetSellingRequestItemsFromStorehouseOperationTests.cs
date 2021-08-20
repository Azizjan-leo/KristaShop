using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models.DTOs;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.Extensions.Caching.Memory;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs.Mappings;
using Module.Partners.Business.Services.Operations;
using Module.Partners.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using Tests.Module.Partners.Business.DataGenerators;

namespace Tests.Module.Partners.Business.Operations {
    [TestFixture]
    public class GetSellingRequestItemsFromStorehouseOperationTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private GetSellingRequestItemsFromStorehouseOperation _operation;
        private ModuleDtoItemsGenerator _generator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null, null);
            _operation = new GetSellingRequestItemsFromStorehouseOperation(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PartnersMappingProfile>())));
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
            await _context.Set<Barcode>().AddRangeAsync(entityGenerator.CreateBarcodesForLine(5, 2, 1, "42-44-46", 5));

            await _uow.Partners.AddRangeAsync(new List<Partner> {
                entityGenerator.CreatePartner(1),
                entityGenerator.CreatePartner(2),
            });
            
            await _context.SaveChangesAsync();
        }

        [Test]
        public async Task Should_ThrowEmptyListException_When_InputListIsEmpty() {
            Func<Task> action = async () => {
                await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO>()));
            };

            await action.Should().ThrowAsync<EmptyListException>();
        }
        
        [Test]
        public async Task Should_ThrowNotFoundException_When_NoSuchItemInUserStorehouse() {
            Func<Task> action = async () => {
                await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                    new("001", 3)
                }));
            };

            await action.Should().ThrowAsync<StorehouseItemNotFound>();
        }
        
        [Test]
        public async Task Should_ThrowNotFoundException_When_ItemExistInOtherUserStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(2, 1, 1, "10-01", 42, 1, 3));
            await _uow.SaveChangesAsync();

            Func<Task> action = async () => {
                await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                    new("001", 3)
                }));
            };

            await action.Should().ThrowAsync<StorehouseItemNotFound>();
        }
        
        [Test]
        public async Task Should_ThrowNotEnoughAmountException_When_ItemExistInUserStorehouseButHasLessThenRequiredAmount() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1));
            await _uow.SaveChangesAsync();

            Func<Task> action = async () => {
                await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                    new("001", 3)
                }));
            };

            await action.Should().ThrowAsync<NotEnoughAmountException>();
        }

        [Test]
        public async Task Should_CreateDocumentModel_When_ItemExistInUserStorehouseButHasSeveralRecords() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 2, 1, "10-01", 42, 1, 1, 30));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1, 15));
            await _uow.SaveChangesAsync();

            await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                new("001", 3)
            }));
            var actual = _operation.Result;

            actual.Items.Should().HaveCount(1);
        }
        
        [Test]
        public async Task Should_HaveCorrectFirstItem_When_GetSellingRequestItemsAndItemExistInUserStorehouseButHasSeveralRecords() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 2, 1, "10-01", 42, 1, 1, 30));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 1, 15));
            await _uow.SaveChangesAsync();

            await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                new("001", 3)
            }));
            var actual = _operation.Result.Items.First();
            
            actual.ModelId.Should().Be(1);
            actual.Size.Value.Should().Be("42");
            actual.ColorId.Should().Be(1);
            actual.Amount.Should().Be(3);
            actual.Price.Should().Be(25);
            actual.PriceInRub.Should().Be(25 * DtoItemsGenerator.RubRate);
        }
        
        [Test]
        public async Task Should_CreateDocumentModel_When_ItemExistInUserStorehouseButThereAreDifferentModelsInUserStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 3, 1, "10-01", 42, 1, 1, 30));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 3, 2, "20-01", 42, 1, 1, 15));
            await _uow.SaveChangesAsync();

            await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                new("001", 3)
            }));
            var actual = _operation.Result;

            actual.Items.Should().HaveCount(1);
        }
        
        [Test]
        public async Task Should_HaveCorrectFirstItem_When_ItemExistInUserStorehouseButThereAreDifferentModelsInUserStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 3, 1, "10-01", 42, 1, 1, 30));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 3, 2, "20-01", 42, 1, 1, 15));
            await _uow.SaveChangesAsync();

            await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                new("001", 3)
            }));
            var actual = _operation.Result.Items.First();
            
            actual.ModelId.Should().Be(1);
            actual.Size.Value.Should().Be("42");
            actual.ColorId.Should().Be(1);
            actual.Amount.Should().Be(3);
            actual.Price.Should().Be(30);
            actual.PriceInRub.Should().Be(30 * DtoItemsGenerator.RubRate);
        }
        
        [Test]
        public async Task Should_GroupInputItems_When_RequestedSimilarItemsAndItemExistInUserStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 3, 1, "10-01", 42, 1, 1, 30));
            await _uow.SaveChangesAsync();

            await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                new("001", 1),
                new("001", 2)
            }));
            var actual = _operation.Result;

            actual.Items.Should().HaveCount(1);
            actual.Items.First().Amount.Should().Be(3);
        }
        
        [Test]
        public async Task Should_CreateDocumentModel_When_RequestedSeveralItemsAndItemsExistInUserStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 3, 1, "10-01", 42, 1, 1));
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 2, "20-01", 42, 1, 3));
            await _uow.SaveChangesAsync();
 
            await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                new("001", 3),
                new("005", 3)
            }));
            var actual = _operation.Result;

            actual.Items.Should().HaveCount(2);
        }

        [Test]
        public async Task Should_HaveUserId_When_GetSellingRequestItemAndItemExistInUserStorehouse() {
            await _uow.PartnerStorehouseItems.AddRangeAsync(_generator.GenerateStorehouseItemsEntity(1, 1, 1, "10-01", 42, 1, 3));
            await _uow.SaveChangesAsync();

            await _operation.HandleAsync(new UserItems<BarcodeAmountDTO>(1, new List<BarcodeAmountDTO> {
                new("001", 3)
            }));
            var actual = _operation.Result;

            actual.UserId.Should().Be(1);
        }
    }
}
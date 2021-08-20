using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.Extensions.Caching.Memory;
using Module.Order.Business.Models.Mappings;
using Module.Order.Business.Services;
using Module.Order.Business.UnitOfWork;
using NUnit.Framework;
using Tests.Common;
using Tests.Common.DataGenerators;
using Tests.Module.Order.Business.DataGenerators;

namespace Tests.Module.Order.Business {
    [TestFixture]
    public class CartServiceTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private CartService _cartService;
        private EntityGenerator _entityGenerator;
        private ModuleDtoItemsGenerator _generator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null);
            _cartService = new CartService(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>())));
            _entityGenerator = new EntityGenerator();
            _generator = new ModuleDtoItemsGenerator();
            await _initDbDataAsync();
        }

        [TearDown]
        public void Dispose() {
            _context.Database.EnsureDeleted();
        }
        
        private async Task _initDbDataAsync() {
            await _uow.Users.AddRangeAsync(new List<User> {
                _entityGenerator.CreateUsers(1, 1, 1),
                _entityGenerator.CreateUsers(2, 2, 2)
            });
            await _context.Set<City>().AddRangeAsync(new List<City>() {
                _entityGenerator.CreateCity(1, "Бишкек"),
                _entityGenerator.CreateCity(2, "Москва")
            });
            await _context.Set<Model>().AddRangeAsync(new List<Model> {
                _entityGenerator.CreateModel(1, "10-01", "42-44-46", 15),
                _entityGenerator.CreateModel(2, "20-01", "42-44-46", 15)
            });
            await _context.Set<Color>().AddRangeAsync(new List<Color> {
                _entityGenerator.CreateColorWithGroup(1, "Айвори"),
                _entityGenerator.CreateColorWithGroup(2, "Пудра")
            });
            await _context.Set<CatalogItemDescriptor>().AddRangeAsync(new List<CatalogItemDescriptor> {
                _entityGenerator.CreateDescriptor("10-01"),
                _entityGenerator.CreateDescriptor("20-01")
            });
            
            await _context.SaveChangesAsync();
        }
        
        [Test]
        public async Task Should_ThrowException_When_InsertCartItemWithZeroAmount() {
            Func<Task> action = async () => {
                await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 0, CatalogType.InStockLines, 10));
            };
            
            await action.Should().ThrowAsync<ExceptionBase>();
        }
        
        [Test]
        public async Task Should_ThrowException_When_InsertCartItemWithWrongAmount() {
            Func<Task> action = async () => {
                await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 2, CatalogType.InStockLines, 10));
            };
            
            await action.Should().ThrowAsync<ExceptionBase>();
        }

        [Test]
        public async Task Should_CreateCartItem_When_CartItemForThisModelNotExist() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));

            var actual = await _uow.Carts.GetAllAsync();

            actual.Should().HaveCount(1);
            actual.First().Amount.Should().Be(3);
        }

        [Test]
        public async Task Should_UpdateCartItemAmount_When_CartItemForThisModelExist() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines, 10));

            var actual = await _uow.Carts.GetAllAsync();

            actual.Should().HaveCount(1);
            actual.First().Amount.Should().Be(6);
        }
        
        [Test]
        public async Task Should_CreateCartItem_When_CartItemForThisModelExistButForOtherUser() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(2, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines, 10));

            var actual = await _uow.Carts.GetAllAsync();

            actual.Should().HaveCount(2);
            actual.First(x => x.UserId == 1).Amount.Should().Be(3);
            actual.First(x => x.UserId == 2).Amount.Should().Be(6);
        }
        
        [Test]
        public async Task Should_UpdateCartItemAmount_When_CartItemForThisModelExistForBothUsers() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(2, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(2, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines, 10));

            var actual = await _uow.Carts.GetAllAsync();

            actual.Should().HaveCount(2);
            actual.First(x => x.UserId == 1).Amount.Should().Be(3);
            actual.First(x => x.UserId == 2).Amount.Should().Be(6);
        }
        
        [Test]
        public async Task Should_CreateCartItem_When_CartItemForOtherModel() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42", 1, 1, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));

            var actual = await _uow.Carts.GetAllAsync();

            actual.Should().HaveCount(2);
            actual.Sum(x => x.Amount).Should().Be(4);
        }
        
        [Test]
        public async Task Should_UpdateCartItemAmount_When_ManyCartItemsExist() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42", 1, 1, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "44", 1, 1, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "46", 1, 1, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(2, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines, 10));

            var actual = await _uow.Carts.GetAllAsync();

            actual.Should().HaveCount(5);
            actual.First(x => x.UserId == 1 && x.CatalogId == CatalogType.InStockLines).Amount.Should().Be(6);
            actual.Sum(x => x.Amount).Should().Be(12);
        }

        [Test]
        public async Task Should_ThrowNotFoundException_When_TryDeleteNotExistingItem() {
            Func<Task> action = async () => {
                await _cartService.RemoveCartItemByIdAsync(1, 1);
            };
            
            await action.Should().ThrowAsync<EntityNotFoundException>();
        }
        
        [Test]
        public async Task Should_ThrowNotFoundException_When_TryDeleteItemThatBelongsToOtherUser() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));

            Func<Task> action = async () => {
                await _cartService.RemoveCartItemByIdAsync(1, 2);
            };
            
            await action.Should().ThrowAsync<ExceptionBase>();
        }
        
        [Test]
        public async Task Should_DeleteCartItem_When_TryDeleteItemThatExistAndBelongsToThisUser() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));

            await _cartService.RemoveCartItemByIdAsync(1, 1);

            var actual = await _uow.Carts.GetAllAsync();
            actual.Should().HaveCount(0);
        }
        
        [Test]
        public async Task Should_DeleteCartItem_When_TryDeleteItemAndUserHasOtherItems() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "44", 1, 3, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));

            await _cartService.RemoveCartItemByIdAsync(3, 1);

            var actual = await _uow.Carts.GetAllAsync();
            actual.Should().HaveCount(2);
            actual.Should().OnlyContain(x => x.CatalogId == CatalogType.InStockParts);
        }

        [Test]
        public async Task Should_ThrowException_When_TrySetCartItemAmountToZero() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            
            Func<Task> action = async () => {
                await _cartService.UpdateCartItemAmountByIdAsync(1, 0, 0);
            };
            
            await action.Should().ThrowAsync<ExceptionBase>().WithMessage("Can not set cart item amount = 0");
        }
        
        [Test]
        public async Task Should_ThrowException_When_CartItemNotFound() {
            Func<Task> action = async () => {
                await _cartService.UpdateCartItemAmountByIdAsync(1, 1, 3);
            };
            
            await action.Should().ThrowAsync<EntityNotFoundException>();
        }
        
        [Test]
        public async Task Should_UpdateCartItemAmount_When_CartItemExistAndMaxAmountNotSet() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _cartService.UpdateCartItemAmountByIdAsync(1, 1, 1);
            
            var actual = await _uow.Carts.GetAllAsync();
            actual.Should().HaveCount(1);
            actual.First().Amount.Should().Be(1);
        }
        
        [Test]
        public async Task Should_ClearUserCart_When_CartItemsExist() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "44", 1, 3, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "46", 1, 3, CatalogType.InStockParts, 10));

            await _cartService.ClearUserCartAsync(1);
            
            var actual = await _uow.Carts.GetAllAsync();
            
            actual.Should().HaveCount(0);
        }
        
        [Test]
        public async Task Should_ClearUserCart_When_CartItemsExistForOtherUser() {
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "44", 1, 3, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(1, 1, "10-01", "46", 1, 3, CatalogType.InStockParts, 10));
            await _cartService.InsertOrUpdateCartItemAsync(_generator.GenerateCartItem(2, 1, "10-01", "46", 1, 3, CatalogType.InStockParts, 10));

            await _cartService.ClearUserCartAsync(2);
            
            var actual = await _uow.Carts.GetAllAsync();
            
            actual.Should().HaveCount(3);
        }

        [Test] 
        public async Task Should_NotThrowException_When_TryCleanEmptyCart() {
            await _cartService.ClearUserCartAsync(1);
            
            var actual = await _uow.Carts.GetAllAsync();
            
            actual.Should().HaveCount(0);
        }
        
        [Test] 
        public async Task Should_DeleteAllCartItems_When_CartItemsWereAddedMoreThanTwoWeeksAgo() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10, DateTime.Today.AddDays(-15)));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 2, 1, "10-01", "44", 1, 3, CatalogType.InStockParts, 10, DateTime.Today.AddDays(-15)));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 3, 1, "10-01", "46", 1, 3, CatalogType.InStockParts, 10, DateTime.Now));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 4, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10, DateTime.Today.AddDays(-15)));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 5, 1, "10-01", "44", 1, 3, CatalogType.InStockParts, 10, DateTime.Now));
            await _uow.SaveChangesAsync();

            await _cartService.ClearOldItemsAsync();
            var actual = await _uow.Carts.GetAllAsync();
            
            actual.Should().HaveCount(2);
        }
    }
}
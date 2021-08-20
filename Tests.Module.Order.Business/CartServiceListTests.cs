using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models.Filters;
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

namespace Tests.Module.Order.Business {
    [TestFixture]
    public class CartServiceListTests {
        private KristaShopDbContext _context;
        private IUnitOfWork _uow;
        private CartService _cartService;
        private EntityGenerator _entityGenerator;

        [SetUp]
        public async Task Setup() {
            _context = await ContextManager.InitializeTestContextWithMigrationsAsync(GlobalAppSettings.AppContextConnection);
            _uow = new UnitOfWork(_context, new MemoryCache(new MemoryCacheOptions()), null);
            _cartService = new CartService(_uow, new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>())));
            _entityGenerator = new EntityGenerator();
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
        public async Task Should_GetDetailedCartItem_When_GetExistingCartItem() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartItemByIdAsync(1, 1);
            actual.Should().NotBeNull();
        }

        [Test]
        public async Task Should_GetNull_When_TryGetNonExistingItem() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartItemByIdAsync(2, 1);
            actual.Should().BeNull();
        }
        
        [Test]
        public async Task Should_ThrowException_When_TryGetExistingCartItemOfAnotherUser() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.SaveChangesAsync();
            
            Func<Task> action = async () => {
                await _cartService.GetCartItemByIdAsync(1, 2);
            };
            
            await action.Should().ThrowAsync<ExceptionBase>();
        }
        
        [Test]
        public async Task Should_GetUserCartItems_When_CartItemsExistForUser() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 3, 1, "10-01", "44", 1, 5, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 4, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 5, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartItemsAsync(1);
            actual.Should().HaveCount(4);
            actual.Sum(x => x.Amount).Should().Be(17);
        }
        
        [Test]
        public async Task Should_GetEmptyList_When_CartItemsNotExistForUser() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 3, 1, "10-01", "44", 1, 5, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 4, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartItemsAsync(2);
            actual.Should().HaveCount(0);
        }

        [Test]
        public async Task Should_GetUserCartTotalAmount_When_CartItemsExistForUser() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 3, 1, "10-01", "44", 1, 5, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 4, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 5, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartTotalAmountAsync(1);
            actual.Should().Be(17);
        }
        
        [Test]
        public async Task Should_GetZero_When_CartItemsNotExistForUser() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 3, 1, "10-01", "44", 1, 5, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 1, 4, 1, "10-01", "42-44-46", 1, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartTotalAmountAsync(2);
            actual.Should().Be(0);
        }

        [Test]
        public async Task Should_GroupCartItems_When_CartItemsExistAndFilterByArticul() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 7, 2, "20-01", "42-44-46", 1, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartsItemsGroupedAsync(new ReportsFilter {
                SelectedArticuls = new List<string> {"20-01"}
            });

            actual.Should().HaveCount(2);
            actual.Sum(x => x.Amount).Should().Be(18);
        }
        
        [Test]
        public async Task Should_GroupCartItems_When_CartItemsExistAndFilterByColor() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartsItemsGroupedAsync(new ReportsFilter {
                SelectedColorIds = new List<int> { 1 }
            });

            actual.Should().HaveCount(3);
            actual.Sum(x => x.Amount).Should().Be(18);
        }
        
          
        [Test]
        public async Task Should_GroupCartItems_When_CartItemsExistAndFilterByCity() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartsItemsGroupedAsync(new ReportsFilter {
                SelectedCityIds = new List<int> { 2 }
            });

            actual.Should().HaveCount(2);
            actual.Sum(x => x.Amount).Should().Be(18);
        }
        
        [Test]
        public async Task Should_GroupCartItems_When_CartItemsExistAndFilterByUser() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartsItemsGroupedAsync(new ReportsFilter {
                SelectedUserIds = new List<int> { 2 }
            });

            actual.Should().HaveCount(2);
            actual.Sum(x => x.Amount).Should().Be(18);
        }
        
        [Test]
        public async Task Should_GroupCartItems_When_CartItemsExistAndFilterByManager() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartsItemsGroupedAsync(new ReportsFilter {
                SelectedManagerIds = new List<int> { 2 }
            });

            actual.Should().HaveCount(2);
            actual.Sum(x => x.Amount).Should().Be(18);
        }
        
        [Test]
        public async Task Should_GroupCartItems_When_CartItemsExistAndFilterByCatalogId() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartsItemsGroupedAsync(new ReportsFilter {
                SelectedCatalogIds = new List<int> { (int) CatalogType.InStockLines }
            });

            actual.Should().HaveCount(2);
            actual.Sum(x => x.Amount).Should().Be(18);
        }

        [Test]
        public async Task Should_GroupCartItemsByUser_When_CartItemsExists() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartTotalsGroupedByUsersAsync(new ReportsFilter());

            actual.Should().HaveCount(2);
            actual.First(x => x.UserId == 1).TotalItemsCount.Should().Be(12);
            actual.First(x => x.UserId == 2).TotalItemsCount.Should().Be(18);
        }
        
        [Test]
        public async Task Should_GetEmptyList_When_GroupCartItemsByUserAndCartItemsNotExists() {
            var actual = await _cartService.GetCartTotalsGroupedByUsersAsync(new ReportsFilter());

            actual.Should().HaveCount(0);
        }
        
        [Test]
        public async Task Should_GroupCartItemsByUser_When_CartItemsExistsAndFilterByArticul() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartTotalsGroupedByUsersAsync(new ReportsFilter {
                SelectedArticuls = new List<string> { "20-01" }
            });

            actual.Should().HaveCount(1);
            actual.Sum(x => x.TotalItemsCount).Should().Be(18);
        }
        
        [Test]
        public async Task Should_GroupCartItemsByUser_When_CartItemsExistsAndFilterByColor() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartTotalsGroupedByUsersAsync(new ReportsFilter {
                SelectedColorIds = new List<int> { 2 }
            });

            actual.Should().HaveCount(1);
            actual.Sum(x => x.TotalItemsCount).Should().Be(18);
        }
        
        [Test]
        public async Task Should_GroupCartItemsByUser_When_CartItemsExistsAndFilterByCity() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartTotalsGroupedByUsersAsync(new ReportsFilter {
                SelectedCityIds = new List<int> { 2 }
            });

            actual.Should().HaveCount(1);
            actual.Sum(x => x.TotalItemsCount).Should().Be(18);
        }
        
        [Test]
        public async Task Should_GroupCartItemsByUser_When_CartItemsExistsAndFilterByUser() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartTotalsGroupedByUsersAsync(new ReportsFilter {
                SelectedUserIds = new List<int> { 2 }
            });

            actual.Should().HaveCount(1);
            actual.Sum(x => x.TotalItemsCount).Should().Be(18);
        }
        
        [Test]
        public async Task Should_GroupCartItemsByUser_When_CartItemsExistsAndFilterByCatalog() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines));
            await _uow.SaveChangesAsync();

            var actual = await _cartService.GetCartTotalsGroupedByUsersAsync(new ReportsFilter {
                SelectedCatalogIds = new List<int> { (int) CatalogType.InStockLines }
            });

            actual.Should().HaveCount(2);
            actual.Sum(x => x.TotalItemsCount).Should().Be(18);
        }

        [Test]
        public async Task Should_HaveZeroTotalAmount_When_CartItemsNotExist() {
            var actual = await _cartService.GetCartsTotalsAsync(new ReportsFilter());

            actual.Totals.TotalAmount.Should().Be(0);
            actual.Totals.TotalSum.Should().Be(0);
        }
        
        [Test]
        public async Task Should_HaveTotalAmount_When_CartItemsExist() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 9, 2, "20-01", "42", 2, 3, CatalogType.Preorder, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 11, 2, "20-01", "42-44-46", 2, 6, CatalogType.Preorder, 10));
            await _uow.SaveChangesAsync();
            
            var actual = await _cartService.GetCartsTotalsAsync(new ReportsFilter());

            actual.Totals.TotalAmount.Should().Be(39);
            actual.Totals.TotalSum.Should().Be(390);
            actual.PreorderTotals.TotalAmount.Should().Be(9);
            actual.PreorderTotals.TotalSum.Should().Be(90);
            actual.InStockLinesTotals.TotalAmount.Should().Be(18);
            actual.InStockLinesTotals.TotalSum.Should().Be(180);
            actual.InStockPartsTotals.TotalAmount.Should().Be(12);
            actual.InStockPartsTotals.TotalSum.Should().Be(120);
        }
        
        [Test]
        public async Task Should_HaveTotalAmount_When_CartItemsExistAndFilterByArticul() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 1, "10-01", "42", 2, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 9, 2, "20-01", "42", 2, 3, CatalogType.Preorder, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 11, 1, "10-01", "42-44-46", 2, 6, CatalogType.Preorder, 10));
            await _uow.SaveChangesAsync();
            
            var actual = await _cartService.GetCartsTotalsAsync(new ReportsFilter {
                SelectedArticuls = new List<string> { "10-01" }
            });

            actual.Totals.TotalAmount.Should().Be(24);
            actual.Totals.TotalSum.Should().Be(240);
            actual.PreorderTotals.TotalAmount.Should().Be(6);
            actual.PreorderTotals.TotalSum.Should().Be(60);
            actual.InStockLinesTotals.TotalAmount.Should().Be(6);
            actual.InStockLinesTotals.TotalSum.Should().Be(60);
            actual.InStockPartsTotals.TotalAmount.Should().Be(12);
            actual.InStockPartsTotals.TotalSum.Should().Be(120);
        }
        
        [Test]
        public async Task Should_HaveTotalAmount_When_CartItemsExistAndFilterByColor() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 9, 2, "20-01", "42", 2, 3, CatalogType.Preorder, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 11, 2, "20-01", "42-44-46", 2, 6, CatalogType.Preorder, 10));
            await _uow.SaveChangesAsync();
            
            var actual = await _cartService.GetCartsTotalsAsync(new ReportsFilter {
                SelectedColorIds = new List<int> { 2 }
            });

            actual.Totals.TotalAmount.Should().Be(27);
            actual.Totals.TotalSum.Should().Be(270);
            actual.PreorderTotals.TotalAmount.Should().Be(9);
            actual.PreorderTotals.TotalSum.Should().Be(90);
            actual.InStockLinesTotals.TotalAmount.Should().Be(12);
            actual.InStockLinesTotals.TotalSum.Should().Be(120);
            actual.InStockPartsTotals.TotalAmount.Should().Be(6);
            actual.InStockPartsTotals.TotalSum.Should().Be(60);
        }
        
        [Test]
        public async Task Should_HaveTotalAmount_When_CartItemsExistAndFilterByCity() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 9, 2, "20-01", "42", 2, 3, CatalogType.Preorder, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 11, 2, "20-01", "42-44-46", 2, 6, CatalogType.Preorder, 10));
            await _uow.SaveChangesAsync();
            
            var actual = await _cartService.GetCartsTotalsAsync(new ReportsFilter {
                SelectedCityIds = new List<int> { 2 }
            });

            actual.Totals.TotalAmount.Should().Be(27);
            actual.Totals.TotalSum.Should().Be(270);
            actual.PreorderTotals.TotalAmount.Should().Be(9);
            actual.PreorderTotals.TotalSum.Should().Be(90);
            actual.InStockLinesTotals.TotalAmount.Should().Be(12);
            actual.InStockLinesTotals.TotalSum.Should().Be(120);
            actual.InStockPartsTotals.TotalAmount.Should().Be(6);
            actual.InStockPartsTotals.TotalSum.Should().Be(60);
        }
        
        [Test]
        public async Task Should_HaveTotalAmount_When_CartItemsExistAndFilterByUser() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 9, 2, "20-01", "42", 2, 3, CatalogType.Preorder, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 11, 2, "20-01", "42-44-46", 2, 6, CatalogType.Preorder, 10));
            await _uow.SaveChangesAsync();
            
            var actual = await _cartService.GetCartsTotalsAsync(new ReportsFilter {
                SelectedUserIds = new List<int> { 2 }
            });

            actual.Totals.TotalAmount.Should().Be(27);
            actual.Totals.TotalSum.Should().Be(270);
            actual.PreorderTotals.TotalAmount.Should().Be(9);
            actual.PreorderTotals.TotalSum.Should().Be(90);
            actual.InStockLinesTotals.TotalAmount.Should().Be(12);
            actual.InStockLinesTotals.TotalSum.Should().Be(120);
            actual.InStockPartsTotals.TotalAmount.Should().Be(6);
            actual.InStockPartsTotals.TotalSum.Should().Be(60);
        }
        
        [Test]
        public async Task Should_HaveTotalAmount_When_CartItemsExistAndFilterByManager() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 9, 2, "20-01", "42", 2, 3, CatalogType.Preorder, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 11, 2, "20-01", "42-44-46", 2, 6, CatalogType.Preorder, 10));
            await _uow.SaveChangesAsync();
            
            var actual = await _cartService.GetCartsTotalsAsync(new ReportsFilter {
                SelectedManagerIds = new List<int> { 2 }
            });

            actual.Totals.TotalAmount.Should().Be(27);
            actual.Totals.TotalSum.Should().Be(270);
            actual.PreorderTotals.TotalAmount.Should().Be(9);
            actual.PreorderTotals.TotalSum.Should().Be(90);
            actual.InStockLinesTotals.TotalAmount.Should().Be(12);
            actual.InStockLinesTotals.TotalSum.Should().Be(120);
            actual.InStockPartsTotals.TotalAmount.Should().Be(6);
            actual.InStockPartsTotals.TotalSum.Should().Be(60);
        }
        
        [Test]
        public async Task Should_HaveTotalAmount_When_CartItemsExistAndFilterByCatalog() {
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 1, 1, "10-01", "42", 1, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(1, 2, 3, 1, "10-01", "42-44-46", 1, 3, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 5, 2, "20-01", "42", 2, 3, CatalogType.InStockParts, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 2, 7, 2, "20-01", "42-44-46", 2, 6, CatalogType.InStockLines, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 9, 2, "20-01", "42", 2, 3, CatalogType.Preorder, 10));
            await _uow.Carts.AddRangeAsync(_entityGenerator.GenerateCartItemsEntity(2, 1, 11, 2, "20-01", "42-44-46", 2, 6, CatalogType.Preorder, 10));
            await _uow.SaveChangesAsync();
            
            var actual = await _cartService.GetCartsTotalsAsync(new ReportsFilter {
                SelectedCatalogIds = new List<int> { (int) CatalogType.Preorder }
            });

            actual.Totals.TotalAmount.Should().Be(9);
            actual.Totals.TotalSum.Should().Be(90);
            actual.PreorderTotals.TotalAmount.Should().Be(9);
            actual.PreorderTotals.TotalSum.Should().Be(90);
            actual.InStockLinesTotals.TotalAmount.Should().Be(0);
            actual.InStockLinesTotals.TotalSum.Should().Be(0);
            actual.InStockPartsTotals.TotalAmount.Should().Be(0);
            actual.InStockPartsTotals.TotalSum.Should().Be(0);
        }
    }
}
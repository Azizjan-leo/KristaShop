using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;
using Module.Common.Business.Models;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Models;
using Module.Order.Business.Models.Adapters;
using Module.Order.Business.UnitOfWork;

namespace Module.Order.Business.Services {
    public class CartService : ICartService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CartItem1CDTO> GetCartItemByIdAsync(int cartId, int userId) {
            var entity = await _uow.Carts.GetDetailedCartItemByIdAsync(cartId);

            if (entity != null && entity.UserId != userId) {
                throw new ExceptionBase($"User {userId} can not access cart item {cartId} of another user {entity.UserId}");
            }
            
            return _mapper.Map<CartItem1CDTO>(entity);
        }

        public async Task<List<CartItem1CDTO>> GetCartItemsAsync(int userId) {
            return _mapper.Map<List<CartItem1CDTO>>(await _uow.Carts.GetAllByUserIdAsync(userId));
        }

        public async Task<int> GetCartTotalAmountAsync(int userId) {
            return await _uow.Carts.GetTotalAmountAsync(userId);
        }

        public async Task<List<CartItem1CDTO>> GetCartsItemsGroupedAsync(ReportsFilter filter) {
            return _mapper.Map<List<CartItem1CDTO>>(await _uow.Carts.GetGroupedAllItemsAsync(filter));
        }

        public async Task<List<UserCartTotalsDTO>> GetCartTotalsGroupedByUsersAsync(ReportsFilter filter) {
            return _mapper.Map<List<UserCartTotalsDTO>>(await _uow.Carts.GetCartTotalsForAllUsersAsync(filter));
        }

        public async Task<ReportTotalsDTO> GetCartsTotalsAsync(ReportsFilter filter) {
            var cartTotals = await _uow.Carts.GetTotalsByAllCartsAsync(filter);
            return new OrderTotalsAdapter().Convert(cartTotals.ToList());
        }

        public async Task<List<int>> GetUserIdsWithFilledCarts() {
            return await _uow.Carts.GetUserIdsWithNotEmptyCarts();
        }

        public async Task<List<string>> GetArticulsAsync() {
            return await _uow.Carts.GetArticulsListInCartsAsync();
        }

        public async Task<List<LookUpItem<int, string>>> GetUsersAsync() {
            return await _uow.Carts.GetUsersListInCartsAsync();
        }

        public async Task<int> InsertOrUpdateCartItemAsync(CartItem1CDTO dto) {
            if (dto.Amount <= 0 || dto.Amount % new SizeValue(dto.SizeValue).Parts != 0) {
                throw new ExceptionBase($"Can not set cart item amount = {dto.Amount}");
            }
            
            if (!await _uow.Users.HasAccessToCatalogAsync(dto.UserId, dto.CatalogId)) {
                throw new CatalogAccessException(dto.UserId, dto.CatalogId);
            }

            var catalogItems = await _uow.CatalogItems.GetCatalogItems(dto.CatalogId, dto.Articul, dto.SizeValue, dto.ColorId);
            if (!catalogItems.Any()) {
                throw new CatalogItemNotFoundException(dto.CatalogId, dto.ModelId, dto.ColorId, dto.SizeValue);
            }

            var totalItemsCount = catalogItems.Sum(x => x.Amount);
            if (dto.CatalogId != CatalogType.Preorder && totalItemsCount < dto.Amount) {
                throw new NotEnoughAmountException(dto.CatalogId, dto.ModelId, dto.ColorId, dto.SizeValue, dto.Amount, totalItemsCount); 
            }
            
            var entity = await _uow.Carts.GetCartItemAsync(dto.UserId, dto.ModelId, dto.ColorId, (int) dto.CatalogId,
                dto.SizeValue);
            var initialAmount = entity?.Amount ?? 0;
            
            if (entity != null) {
                entity.Amount = dto.Amount;
            } else {
                entity = _mapper.Map<CartItem>(dto);
                await _uow.Carts.AddAsync(entity);
            }

            await _uow.SaveChangesAsync();
            return entity.Amount - initialAmount;
        }

        public async Task RemoveCartItemByIdAsync(int cartId, int userId) {
            var entity = await _uow.Carts.GetByIdAsync(cartId);
            if (entity == null) {
                throw new EntityNotFoundException($"Cart item {cartId} not found");
            }

            if (entity.UserId != userId) {
                throw new ExceptionBase($"User {userId} can not delete cart item {cartId} of another user {entity.UserId}");
            }

            _uow.Carts.Delete(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task<int> UpdateCartItemAmountByIdAsync(int cartId, int userId, int quantity) {
            if (quantity == 0) {
                throw new ExceptionBase($"Can not set cart item amount = 0");
            }
            
            var entity = await _uow.Carts.GetByIdAsync(cartId);
            if (entity == null) {
                throw new EntityNotFoundException($"Cart item {cartId} not found");
            }
            
            if (entity.UserId != userId) {
                throw new ExceptionBase($"User {userId} can not access cart item {cartId} of another user {entity.UserId}");
            }

            var size = new SizeValue(entity.SizeValue);

            var initialAmount = entity.Amount;
            if (entity.Amount <= 0) {
                entity.Amount = 1 * size.Parts;
            }

            var amountToAdd = quantity * size.Parts;

            var maxAmount = await _uow.CatalogItems.GetCatalogItemMaxAmountAsync(entity.CatalogId, entity.ModelId, entity.ColorId);
            if (maxAmount <= 0) {
                entity.Amount = 1 * size.Parts;
            } else {
                if (entity.Amount == maxAmount && entity.Amount + amountToAdd > maxAmount) {
                    return 0;
                }

                if (entity.Amount > maxAmount) {
                    entity.Amount = maxAmount;
                } else if (entity.Amount + amountToAdd > 0) {
                    entity.Amount += amountToAdd;
                }
            }
            
            _uow.Carts.Update(entity);
            await _uow.SaveChangesAsync();
            return entity.Amount - initialAmount;
        }

        public async Task ClearUserCartAsync(int userId) {
            await _uow.Carts.ClearUserCartAsync(userId);
            await _uow.SaveChangesAsync();
        }

        public async Task ClearOldItemsAsync() {
            await _uow.Carts.ClearAllOldCartsAsync(DateTime.Now.AddDays(-14));
            await _uow.SaveChangesAsync();
        }
    }
}
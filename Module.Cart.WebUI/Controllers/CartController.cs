using System;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Cart.WebUI.Models;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Infrastructure;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Models;
using Serilog;

namespace Module.Cart.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.AllFrontSchemes)]
    public class CartController : AppControllerBase {
        private readonly ICartService _cartService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CartController(IMapper mapper, ICartService cartService, ILogger logger) {
            _cartService = cartService; 
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromServices] ICollectionService collectionService) {
            try {
                var cartItems = await _cartService.GetCartItemsAsync(UserSession.UserId);
                if (cartItems.Count == 0) {
                    return RedirectToAction("CartEmpty", "Error");
                }

                return View((cartItems, await collectionService.GetCurrentPrepayPercentAsync()));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get user {userId} cart. {message}", UserSession.UserId, ex.Message);
                SetFailureNotification();
                return BadRequest();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ShowGuestCart(int userId, string hash, [FromServices] ICollectionService collectionService) {
            try {
                if (userId >= 0 || hash != HashHelper.TransformPassword(userId.ToString())) {
                    return Forbid();
                }

                var cartItems = await _cartService.GetCartItemsAsync(userId);
                if (cartItems.Count == 0) {
                    return RedirectToAction("CartEmpty", "Error");
                }

                return View((cartItems, await collectionService.GetCurrentPrepayPercentAsync()));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get user {userId} cart. {message}", UserSession.UserId, ex.Message);
                SetFailureNotification();
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CartItemViewModel model) {
            try {
                if (model.Amount <= 0) {
                    return Ok(new CartUpdateResponse(false, "Не удалось положить в корзину товар в количестве 0 единиц", 0));
                }

                if (model.CatalogId == CatalogType.Open) {
                    return Ok(new CartUpdateResponse(false,
                        "Добавление в корзину товаров из открытого каталога не допускается", 0));
                }

                model.UserId = UserSession.UserId;
                var amountAdded = await _cartService.InsertOrUpdateCartItemAsync(_mapper.Map<CartItem1CDTO>(model));
                return Ok(new CartUpdateResponse(true, "Товар успешно добавлен в корзину!", amountAdded));
            } catch (ExceptionBase ex) {
                return Ok(new CartUpdateResponse(false, ex.ReadableMessage, 0));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to insert cart item. {message}", ex.Message);
                return Ok(new CartUpdateResponse(false, "Ошибка при добавлении товара в корзину.", 0));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int cartId) {
            try {
                await _cartService.RemoveCartItemByIdAsync(cartId, UserSession.UserId);
                SetNotification(OperationResult.Success("Товар удален из корзины."));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to remove cart item. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Не удалось удалить товар из корзины."));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ItemAmountChange(int id, string dir) {
            dir = dir.Trim().ToLower();
            if (dir != "up") dir = "dn";

            try {
                var addedAmount = await _cartService.UpdateCartItemAmountByIdAsync(id, UserSession.UserId, dir == "dn" ? -1 : 1);
                return Ok(new {
                    Success = true,
                    AddedAmount = addedAmount,
                    Message = "OK",
                });
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to change cart item amount. {message}", ex.Message);
                return Ok(new {
                    Success = false,
                    NewAmount = -1,
                    Message = "Не удалось изменить количество товара в корзине."
                });
            }
        }
    }
}
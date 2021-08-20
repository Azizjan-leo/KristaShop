using System;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;
using Module.Common.WebUI.Infrastructure;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Models;
using Module.Order.WebUI.Models;
using Serilog;

namespace Module.Order.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.AllFrontSchemes)]
    public class MakeOrderController : AppControllerBase {
        private readonly ICartService _cartService;
        private readonly ILogger _logger;

        public MakeOrderController(ICartService cartService, ILogger logger) {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> OrderProducts(OrderViewModel model, [FromServices] IOrder1CService order1CService) {
            try {
                if (!ModelState.IsValid) {
                    SetNotification(ModelState.Values.ErrorsAsOperationResult());
                    return RedirectToAction("Index", "Cart");
                }

                var cartItems = await _cartService.GetCartItemsAsync(UserSession.UserId);
                if (cartItems.Count == 0) {
                    return RedirectToAction("CartEmpty", "Error");
                }

                var newOrder = new CreateOrderDTO {
                    CartItems = cartItems,
                    UserId = UserSession.UserId,
                    UserLogin = UserSession.Login,
                    HasExtraPack = model.HasExtraPackage,
                    Description = model.Description
                };

                var result = await order1CService.CreateOrderAsync(newOrder, UserSession);
                if (result.IsSuccess) {
                    return Success(string.Join(" ", result.Messages), "Спасибо за покупку");
                }

                SetNotification(result);
                return RedirectToAction("Index", "Cart");
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create order. {message} {user}", ex.Message, UserSession);
                SetNotification(OperationResult.Failure("Произошла ошибка при оформлении заказа"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> OrderProductsAsGuest(GuestOrderViewModel model,
            [FromServices] IOrder1CService order1CService) { //, [FromServices] ISignInService signInService TODO: sing out guest
            try {
                if (!ModelState.IsValid) {
                    SetNotification(ModelState.Values.ErrorsAsOperationResult());
                    return RedirectToAction("Index", "Cart");
                }

                var cartItems = await _cartService.GetCartItemsAsync(UserSession.UserId);
                if (cartItems.Count == 0) {
                    return RedirectToAction("CartEmpty", "Error");
                }

                if (UserSession is UnauthorizedSession || !UserSession.IsGuest()) {
                    SetNotification(OperationResult.Failure("В доступе отказано"));
                    return RedirectToAction("Index", "Cart");
                }

                var result = await order1CService.CreateGuestOrderAsync(model.GuestFullName, model.GuestPhone,
                    UserSession as GuestSession);
                if (result.IsSuccess) {
                    // await signInService.SignOutGuest();

                    TempData["fromGuestOrderCheckout"] = "1";
                    return RedirectToAction(nameof(SuccessGuest));
                }

                SetNotification(result);
                return RedirectToAction("Index", "Cart");
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create order. {message} {user}", ex.Message, UserSession);
                SetNotification(OperationResult.Failure("Произошла ошибка при оформлении заказа"));
                return RedirectToAction(nameof(Index));
            }
        }
        
        [NonAction]
        private IActionResult Success(string header, string message) {
            return View("Success", (header, message));
        }

        [AllowAnonymous]
        public IActionResult SuccessGuest() {
            if ((string) TempData["fromGuestOrderCheckout"] != "1") return RedirectToAction("Index", "Home");

            var header = "Заказ успешно оформлен! В ближайшее время с Вами свяжется наш менеджер и уточнит детали.";
            var message = "Спасибо за покупку";
            return View("Success", (header, message));
        }
    }
}
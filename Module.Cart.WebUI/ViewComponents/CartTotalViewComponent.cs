using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Models;
using Module.Order.Business.Interfaces;

namespace Module.Cart.WebUI.ViewComponents {
    public class CartTotalViewComponent : ViewComponentBase {
        private readonly ICartService _cartService;

        public CartTotalViewComponent(ICartService cartService) {
            _cartService = cartService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(ComponentOutValue<int> cartTotalAmount) {
            cartTotalAmount.Value = await _cartService.GetCartTotalAmountAsync(UserSession.UserId);
            return Content(string.Empty);
        }
    }
}
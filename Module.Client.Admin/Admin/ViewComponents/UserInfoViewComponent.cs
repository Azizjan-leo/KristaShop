using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Client.Business.Interfaces;
using Module.Common.WebUI.Base;

namespace Module.Client.Admin.Admin.ViewComponents {
    public class UserInfoViewComponent : ViewComponentBase {
        private readonly IUserService _userService;

        public UserInfoViewComponent(IUserService userService) {
            _userService = userService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(int userId) {
            return View(await _userService.GetUserAsync(userId));
        }
    }
}
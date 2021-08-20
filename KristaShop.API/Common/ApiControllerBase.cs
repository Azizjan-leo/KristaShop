using KristaShop.API.Сonfiguration;
using Microsoft.AspNetCore.Mvc;

namespace KristaShop.API.Common {
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase {
        public int CurrentUserId => int.Parse(User.FindFirst(JwtConfig.UserId)?.Value);
    }
}

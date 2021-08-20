using KristaShop.API.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using KristaShop.API.Infrastructure;
using KristaShop.API.Models;
using KristaShop.API.Models.Requests;
using KristaShop.API.Сonfiguration;
using Module.Client.Business.Interfaces;

namespace KristaShop.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthorizationController : ControllerBase {
        private readonly ISignInService _signInService;
        private readonly ILogger _logger;
        private readonly JwtConfig _jwtConfig;

        public AuthorizationController(ISignInService signInService, ILogger logger,
            IOptionsMonitor<JwtConfig> optionsMonitor) {
            _signInService = signInService;
            _logger = logger;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest model, [FromServices] IUserService userService) {
            try {
                var result = await _signInService.SignIn(model.Login, model.Password, isPersistent: true,
                    isBackend: false, isFromApi: true);
                if (result.IsSuccess) {
                    var user = await userService.GetUserAsync(model.Login); // TODO: return user from signin service
                    var jwtToken = TokenGenerator.GenerateJwtToken(user, _jwtConfig);

                    return Ok(new LoginResult() {
                        Name = user.FullName,
                        Token = jwtToken
                    });
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to login {@user}. {message}", model, ex.Message);
            }

            return BadRequest(new BadRequestResponse("Авторизация"));
        }

        [HttpGet]
        [Route("test")]
        [AllowAnonymous]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Test() {
            if (!User.Identity.IsAuthenticated) {
                return StatusCode(StatusCodes.Status403Forbidden, "Please, login!");
            }

            return StatusCode(StatusCodes.Status200OK, "You are already logged in!");
        }
    }
}
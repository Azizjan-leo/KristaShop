using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using KristaShop.API.Common;
using KristaShop.API.Models.Responses;
using Serilog;
using KristaShop.Common.Models.Requests;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;

namespace KristaShop.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SellController : ApiControllerBase {
        private readonly IPartnerStorehouseService _storehouseService;
        private readonly ILogger _logger;
        public SellController(IPartnerStorehouseService storehouseService, ILogger logger) {
            _storehouseService = storehouseService;
            _logger = logger;
        }

        /// <summary>
        /// Sell model from user storehouse
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SellModelItem(SellModelRequest model) {
            try {
                await _storehouseService.SellStorehouseItemAsync(new SellingDTO {
                    ModelId = model.ModelId,
                    ColorId = model.ColorId,
                    SizeValue = model.SizeValue,
                    UserId = CurrentUserId
                });
                return Ok();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Sell model {@model} by user {userId}. {message}", model, CurrentUserId, ex.Message);
            }

            return BadRequest(new BadRequestResponse("Продажа модели"));
        }
    }
}
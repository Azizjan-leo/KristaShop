using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.API.Common;
using KristaShop.API.Models.Responses;
using Serilog;
using KristaShop.Common.Models.DTOs;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace KristaShop.API.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RevisionController : ApiControllerBase {
        private readonly IPartnerStorehouseService _storehouseService;
        private readonly ILogger _logger;

        public RevisionController(IPartnerStorehouseService storehouseService, ILogger logger) {
            _storehouseService = storehouseService;
            _logger = logger;
        }

        /// <summary>
        /// Apply revision result to the user storehouse
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AuditAllModels([FromBody] List<BarcodeAmountDTO> actualStorehouseItems) {
            try {
                await _storehouseService.AuditStorehouseItemsAsync(new Module.Partners.Business.DTOs.BarcodeAmountDTO {
                    UserId = CurrentUserId,
                    Items = actualStorehouseItems
                });
                return Ok();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Audit All Models {UserId}. {message}", CurrentUserId, ex.Message);
            }

            return BadRequest(new BadRequestResponse("Аудит моделей склада"));
        }
    }
}

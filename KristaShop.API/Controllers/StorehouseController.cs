using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.API.Common;
using KristaShop.API.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Partners.Business.Interfaces;
using Serilog;

namespace KristaShop.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StorehouseController : ApiControllerBase {
        private readonly IPartnerStorehouseService _storehouseService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public StorehouseController(IPartnerStorehouseService storehouseService, ILogger logger, IMapper mapper) {
            _storehouseService = storehouseService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all models from user storehouse
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get() {
            try {
                var result = _mapper.Map<IEnumerable<PartnerStorehouseItemVM>>((await _storehouseService.GetStorehouseItemsAsync(CurrentUserId)));
                return Ok(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Get All Models {UserId}. {message}", CurrentUserId, ex.Message);
            }

            return BadRequest(new BadRequestResponse("Получение моделей склада"));
        }
        
        /// <summary>
        /// Get model by barcode from user storehouse
        /// </summary>
        [HttpGet("{barcode}", Name = "Get")]
        public async Task<IActionResult> GetByBarcode(string barcode) {
            try {
                var result = await _storehouseService.GetStorehouseItemAsync(barcode, CurrentUserId);
                if (result == null) {
                    return BadRequest(new BadRequestResponse("Получение информации о модели"));
                }
                
                return Ok(_mapper.Map<PartnerStorehouseItemVM>(result));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Get Model Info By Barcode {barcode}. {message}", barcode, ex.Message);
            }

            return BadRequest(new BadRequestResponse("Получение информации о модели"));
        }
    }
}
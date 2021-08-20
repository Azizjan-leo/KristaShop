using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.API.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using KristaShop.API.Models.Responses;
using KristaShop.Common.Models.Requests;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;

namespace KristaShop.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShipmentsController : ApiControllerBase {
        private readonly IPartnerStorehouseService _partnerStorehouseService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ShipmentsController(IPartnerStorehouseService partnerStorehouseService, ILogger logger, IMapper mapper) {
            _partnerStorehouseService = partnerStorehouseService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all shipments available to the user
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get() {
            try {
                var reservations = await _partnerStorehouseService.GetShipmentsAsync(CurrentUserId);
                return Ok(_mapper.Map<List<BarcodeShipmentItemVM>>(reservations));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Get All Reservations {userId}. {message}", CurrentUserId, ex.Message);
            }
            return BadRequest(new BadRequestResponse("Получение отправок"));
        }
        
        /// <summary>
        /// Income shipment to the storehouse
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Income([FromBody] IncomeShipmentItemsRequest model) {
            try {
                await _partnerStorehouseService.IncomeShipmentItemsAsync(new BarcodeAmountDTO {
                    UserId = CurrentUserId,
                    ReservationDate = model.ReservationDate,
                    Items = model.StorehouseIncomes
                });
                return Ok();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Income Shipment Items {@model}. {message}", model, ex.Message);
            }
            return BadRequest(new BadRequestResponse("Оприходование"));
        }
    }
}
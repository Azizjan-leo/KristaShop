using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.FeatureManagement.Mvc;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Infrastructure;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;
using Serilog;
using SmartBreadcrumbs.Nodes;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Module.Partners.WebUI.Partners.Controllers {
    [FeatureGate(GlobalConstant.FeatureFlags.FeaturePartners)]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    [Permission(ForPartnersOnly = true)]
    [Area(GlobalConstant.PartnersArea)]
    public class ShopController : AppControllerBase {
        private readonly IPartnerStorehouseService _partnerStorehouseService;
        private readonly ILookupsService _lookupsService;
        private readonly ILogger _logger;

        public ShopController(IPartnerStorehouseService partnerStorehouseService, ILookupsService lookupsService, ILogger logger) {
            _partnerStorehouseService = partnerStorehouseService;
            _lookupsService = lookupsService;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            try {
                var result = await _partnerStorehouseService.GetStorehouseItemsGroupedAsync(UserSession.UserId);
                ViewBag.Sizes = new SelectList(await _lookupsService.GetSizesListAsync());

                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner storehouse view. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        public async Task<IActionResult> GetModelInfoByBarcode(string barcode) {
            try {
                var result = await _partnerStorehouseService.GetStorehouseItemAsync(barcode, UserSession.UserId);
                return Ok(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Get Model Info By Barcode {barcode}. {message}", barcode, ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> Reservations() {
            try {
                var result = await _partnerStorehouseService.GetShipmentsGroupedAsync(UserSession.UserId);
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner reservations view. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        public async Task<IActionResult> IncomeShipment([FromBody] string reservationDate) {
            try {
                var date = DateTime.ParseExact(reservationDate, DateTimeExtension.BasicFormatter, CultureInfo.InvariantCulture);
                await _partnerStorehouseService.AutoIncomeShipmentItemsAsync(UserSession.UserId, date);
                return Ok();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner storehouse view. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> IncomeShipmentItems(IncomeShipmentItemsRequest model) {
            if (ModelState.IsValid) {
                try {
                    await _partnerStorehouseService.IncomeShipmentItemsAsync(new Business.DTOs.BarcodeAmountDTO {
                        UserId = UserSession.UserId,
                        ReservationDate = model.ReservationDate,
                        Items = model.StorehouseIncomes
                    });
                    return Ok();
                } catch (Exception ex) {
                    _logger.Error(ex, "Failed to Income Shipment Items {@model}. {message}", model, ex.Message);
                }
            }
            return BadRequest();
        }

        public async Task<IActionResult> Selling() {
            try {
                var items = await _partnerStorehouseService.GetHistoryItems(UserSession.UserId, DateTimeOffset.Now, MovementDirection.Out, MovementType.Selling, isAmountPositive: true);
                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View(items);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner selling view. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        public async Task<IActionResult> SellModelItem([FromBody]SellModelRequest model, bool includeHistoryItems = true) {
            try {
                await _partnerStorehouseService.SellStorehouseItemAsync(new SellingDTO {
                    ModelId = model.ModelId,
                    ColorId = model.ColorId,
                    SizeValue = model.SizeValue,
                    UserId = UserSession.UserId
                });
                
                if (!includeHistoryItems) {
                    return Ok();
                }
                return Ok(await _partnerStorehouseService.GetHistoryItems(UserSession.UserId, DateTimeOffset.Now, MovementDirection.Out, MovementType.Selling, isAmountPositive: true));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Sell model {@model}. {message}", model, ex.Message);
            }
            return BadRequest();
        }

        public async Task<IActionResult> Revision() {
            try {
                var result = await _partnerStorehouseService.GetStorehouseItemsGroupedAsync(UserSession.UserId);
                ViewBag.Sizes = new SelectList(await _lookupsService.GetSizesListAsync());

                SetBreadcrumbs("Order", "Index", "Личный кабинет", null, new MvcBreadcrumbNode(ActionName, ControllerName, "Мой магазин", areaName: ""));
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load partner revision view. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AuditStorehouseItems([FromBody] List<BarcodeAmountDTO> items) {
            try {
                await _partnerStorehouseService.AuditStorehouseItemsAsync(new Business.DTOs.BarcodeAmountDTO {
                    UserId = UserSession.UserId,
                    Items = items
                });
                return Ok(await _partnerStorehouseService.GetStorehouseItemsGroupedAsync(UserSession.UserId));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load audit. {message}", ex.Message);
                return Json(OperationResult.Failure("Произошла ошибка во время сохранения ревизии"));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Order.Admin.Admin.Models;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Models;
using Serilog;

namespace Module.Order.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    public class OrdersController : AppControllerBase {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IOrder1CService _order1CService;
        private readonly ILookupsService _lookupsService;
        private readonly ICityService _cityService;

        public OrdersController(IMapper mapper, ILogger logger, IOrder1CService order1CService,
            ILookupsService lookupsService, ICityService cityService) {
            _mapper = mapper;
            _logger = logger;
            _order1CService = order1CService;
            _lookupsService = lookupsService;
            _cityService = cityService;
        }

        public async Task<IActionResult> Index(OrdersFilter filter, int? modelId, int? colorId, string mode) {
            try {
                ViewBag.ManagersList = new SelectList(await _lookupsService.GetManagersLookupListAsync(), "Value", "Value");
                ViewBag.CatalogsMode = (string.IsNullOrEmpty(mode) ? "" : (mode != "AllCatalogs" ? "" : mode));

                if (modelId.HasValue && colorId.HasValue) {
                    ViewBag.ModelId = modelId.Value;
                    ViewBag.ColorId = colorId.Value;
                    ViewBag.ModelDescription = await _order1CService.GetModelDescriptionAsync(modelId.Value, colorId.Value, mode);
                } else {
                    ViewBag.ModelId = 0;
                    ViewBag.ColorId = 0;
                    ViewBag.ModelDescription = string.Empty;
                }

                return View(filter);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to open order view. {message}", ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsolidatedRequest([FromServices] IClientReportService reportService, [FromServices] ICollectionService collectionService) {
            var lastCollection = await collectionService.GetLastCollectionAsync();
            return await ConsolidatedRequest(new ReportsFilter { SelectedCollectionIds = new List<int> { lastCollection.Id } }, reportService, collectionService);
        }

        [HttpPost]
        public async Task<IActionResult> ConsolidatedRequest(ReportsFilter filter, [FromServices] IClientReportService reportService, [FromServices] ICollectionService collectionService) {
            try {
                filter.Articuls = new SelectList(await _lookupsService.GetArticulsListAsync());
                filter.Colors = await _lookupsService.GetColorsLookupListAsync().AsSelectListAsync();
                filter.Cities = await _cityService.GetCitiesLookupListAsync().AsSelectListAsync();
                filter.Users = await reportService.GetOrderersLookupAsync().AsSelectListAsync();
                filter.Managers = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();

                filter.Catalogs = LookUpItem.FromEnum<CatalogType>().AsSelectList();
                filter.Collections = await collectionService.GetCollectionsLookupListAsync().AsSelectListAsync();

                var result = await reportService.GetAllOrdersAsync(filter);
                return View((filter, result));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get load partner sales decripted report. {message}", ex.Message);
                return BadRequest();
            }
        }
        public async Task<IActionResult> LoadData(int? modelId, int? colorId, string mode) {
            try {
                if (modelId.HasValue && colorId.HasValue) {
                    return Ok(await _order1CService.GetAllOrdersAsync(0, 0, modelId.Value, colorId.Value, null, mode));
                } else {
                    if (UserSession.IsRoot) {
                        return Ok(await _order1CService.GetAllOrdersAsync());
                    }

                    return Ok(await _order1CService.GetAllOrdersAsync(managerId: UserSession.ManagerId, onlyVisibleForManager: true));
                }               
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load orders list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка заказов"));
            }
        }

        public async Task<IActionResult> Details(int id, int? modelId, int? colorId, string mode, [FromServices] ICollectionService collectionService) {
            try {
                ViewBag.CatalogsMode = (string.IsNullOrEmpty(mode) ? "" : (mode != "AllCatalogs" ? "" : mode));

                var order = await _order1CService.GetOrderAdminAsync(id);

                if (order == null) {
                    SetNotification(OperationResult.Failure("Заказ не найден."));
                    return RedirectToAction(nameof(Index));
                } else {

                    if (modelId.HasValue && colorId.HasValue) {
                        ViewBag.ModelId = modelId.Value;
                        ViewBag.ColorId = colorId.Value;
                    } else {
                        ViewBag.ModelId = 0;
                        ViewBag.ColorId = 0;
                    }

                    order.PrepayTotalSum = order.Details
                        .Where(x => x.CatalogId == (int) CatalogType.Preorder)
                        .Sum(x => x.TotalPrice) * (await collectionService.GetCurrentPrepayPercentAsync()).Percent;

                    return View(order);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get Order Details (Id: {id}). {message}", id, ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при получении позиций заказа."));
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> LoadPreOrderReportData() {
            try {
                var reportData = await _order1CService.GetPreorderTotalReportAsync();
                return Ok(reportData);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load preorder report list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении сводного отчета по предзаказу"));
            }
        }

        [NonAction]
        private OrdersTotalFilter _correctFilterDateFields(OrdersTotalFilter filterValues) {
            StringValues tmpStr;
            if (Request.Query.TryGetValue("OrderDateFrom", out tmpStr)) {
                if (!string.IsNullOrEmpty(tmpStr)) {
                    try {
                        filterValues.OrderDateFrom = DateTime.ParseExact(tmpStr, "d.MM.yyyy H:m", CultureInfo.InvariantCulture);
                    } catch (Exception) {
                        filterValues.OrderDateFrom = null;
                    }
                }
            }
            if (Request.Query.TryGetValue("OrderDateTo", out tmpStr)) {
                if (!string.IsNullOrEmpty(tmpStr)) {
                    try {
                        filterValues.OrderDateTo = DateTime.ParseExact(tmpStr, "d.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    } catch (Exception) {
                        filterValues.OrderDateTo = null;
                    }
                }
            }

            return filterValues;
        }

        public async Task<IActionResult> LoadOrdersReportData(OrdersTotalFilter filterValues) {
            try {
                if (filterValues == null) {
                    filterValues = new OrdersTotalFilter();
                }

                filterValues = _correctFilterDateFields(filterValues);

                var reportData = await _order1CService.GetOrdersTotalReportAsync(_mapper.Map<OrdersTotalFilterDTO>(filterValues));
                return Ok(reportData);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load orders report list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении сводного отчета по заказам"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int orderId, int id) {
            try {
                var order = await _order1CService.GetOrderAdminAsync(orderId);

                if (order == null) {
                    SetNotification(OperationResult.Failure("Заказ не найден."));
                    return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
                } else {
                    bool canDelete = true;
                    var itemToDelete = order.Details.Where(x => x.Id == id).FirstOrDefault();
                    if (itemToDelete == null) {
                        SetNotification(OperationResult.Failure("Позиция не найдена в заказе."));
                        canDelete = false;
                    } else {
                        if (itemToDelete.CatalogId == (int)CatalogType.Preorder) {
                            if (order.IsProcessedPreorder) {
                                SetNotification(OperationResult.Failure("Удаление позиции в заказе невозможно."));
                                canDelete = false;
                            }
                        } else {
                            if (order.IsProcessedRetail) {
                                SetNotification(OperationResult.Failure("Удаление позиции в заказе невозможно."));
                                canDelete = false;
                            }
                        }
                    }

                    // TO DO check manager access

                    if (canDelete) {
                        var or = await _order1CService.DeletePositionAsync(orderId, id);
                        if (or.IsSuccess) {
                            SetNotification(OperationResult.Success("Позиция успешно удалена из заказа."));
                        } else {
                            SetNotification(or);
                        }
                    }

                    return Ok(OperationResult.SuccessAjaxRedirect(Url.Action(nameof(Details), new { id = orderId })));
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Delete position from Order (OrderId: {orderId}, Id: {id}). {message}", orderId, id, ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при удалении позиции из заказа."));
                return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id, string mode) {
            try {
                var order = await _order1CService.GetOrderAdminAsync(id);

                if (order == null) {
                    SetNotification(OperationResult.Failure("Заказ не найден."));
                    return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
                } else {
                    bool canDelete = true;

                    if (!string.IsNullOrEmpty(mode)) {
                        mode = mode.Trim().ToLower();
                        if (mode != "preorder" && mode != "retail") {
                            mode = string.Empty;
                        }
                    }

                    if (string.IsNullOrEmpty(mode)) {
                        if (order.IsProcessedPreorder || order.IsProcessedRetail) {
                            SetNotification(OperationResult.Failure("Удаление заказа невозможно."));
                            canDelete = false;
                        }

                        // TO DO check manager access

                        if (canDelete) {
                            var or = await _order1CService.DeleteOrderAsync(id);
                            if (or.IsSuccess) {
                                SetNotification(OperationResult.Success("Заказ успешно удален."));
                            } else {
                                SetNotification(or);
                            }
                        }

                        return Ok(OperationResult.SuccessAjaxRedirect(Url.Action(nameof(Index))));
                    } else {
                        if (mode == "preorder") {
                            if (order.IsProcessedPreorder) {
                                SetNotification(OperationResult.Failure("Удаление заказа невозможно."));
                                canDelete = false;
                            }
                        } else {
                            if (order.IsProcessedRetail) {
                                SetNotification(OperationResult.Failure("Удаление заказа невозможно."));
                                canDelete = false;
                            }
                        }

                        // TO DO check manager access

                        if (canDelete) {
                            var or = await _order1CService.DeleteOrderAsync(id, mode);
                            if (or.IsSuccess) {
                                SetNotification(OperationResult.Success("Заказ успешно удален."));
                            } else {
                                SetNotification(or);
                            }
                        }

                        return Ok(OperationResult.SuccessAjaxRedirect(Url.Action(nameof(Index))));
                    }
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Delete Order (OrderId: {orderId}). {message}", id, ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при удалении заказа."));
                return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Combine(int[] ordersIDs) {
            try {
                if (ordersIDs.Length > 0) {
                    var or = await _order1CService.CombineOrdersAsync(ordersIDs);
                    if (or.IsSuccess) {
                        SetNotification(OperationResult.Success("Заказы успешно объединены."));
                    } else {
                        SetNotification(or);
                    }

                    return Ok(OperationResult.SuccessAjaxRedirect(Url.Action(nameof(Index))));
                } else {
                    SetNotification(OperationResult.Failure("Заказы не найдены."));
                    return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Combine Orders. {message}", ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при объединении заказов."));
                return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddModel(int orderId, int catalogId, int modelId, int colorId, int nomenclatureId, int amount) {
            try {
                var order = await _order1CService.GetOrderAdminAsync(orderId);

                if (order == null) {
                    SetNotification(OperationResult.Failure("Заказ не найден."));
                    return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
                } else {
                    bool canAdd = true;

                    while (true) {
                        if (catalogId < 1 || catalogId > (int)CatalogType.Preorder) {
                            SetNotification(OperationResult.Failure("Некорректное значение каталога."));
                            canAdd = false;
                            break;
                        }

                        if (catalogId == (int)CatalogType.Preorder) {
                            if (order.IsProcessedPreorder) {
                                SetNotification(OperationResult.Failure("Добавление модели в заказ невозможно."));
                                canAdd = false;
                                break;
                            }
                        } else {
                            if (order.IsProcessedRetail) {
                                SetNotification(OperationResult.Failure("Добавление модели в заказ невозможно."));
                                canAdd = false;
                                break;
                            }
                        }

                        if (amount <= 0) {
                            SetNotification(OperationResult.Failure("Некорректное значение количества."));
                            canAdd = false;
                            break;
                        }

                        break;
                    }

                    if (canAdd) {
                        var oResult = await _order1CService.AddModelToOrderAsync(orderId, catalogId, modelId, colorId, nomenclatureId, amount);
                        if (oResult.IsSuccess) {
                            SetNotification(OperationResult.Success("Модель успешно добавлена в заказ."));
                            return Ok(OperationResult.SuccessAjaxRedirect(Url.Action(nameof(Details), new { id = orderId })));
                        } else {
                            SetNotification(oResult);
                            return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Details), new { id = orderId })));
                        }
                    } else {
                        return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Details), new { id = orderId })));
                    }
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Add Model to Order (OrderId: {orderId}, CatalogId: {catalogId}, ModelId: {modelId}, ColorId: {colorId}, NomenclatureId: {nomenclatureId}, Amount: {amount}). {message}", orderId, catalogId, modelId, colorId, nomenclatureId, amount, ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при добавлении модели в заказ."));
                return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ItemAmountChange(int orderId, int id, string dir) {
            dir = dir.Trim().ToLower();
            if (dir != "up") dir = "dn";

            try {
                var order = await _order1CService.GetOrderAdminAsync(orderId);

                OrderDetailsDTO orderItem = null;

                if (order == null) {
                    return Ok(new { Success = false, NeedUpdate = false, Message = "Заказ не найден." });
                } else {
                    orderItem = order.Details.Where(x => x.Id == id).FirstOrDefault();
                    if (orderItem == null) {
                        return Ok(new { Success = false, NeedUpdate = false, Message = "Позиция не найдена в заказе." });
                    } else {
                        if (orderItem.CatalogId == (int)CatalogType.Preorder) {
                            if (order.IsProcessedPreorder) {
                                return Ok(new { Success = false, NeedUpdate = false, Message = "Изменение позиции в заказе невозможно." });
                            }
                        } else {
                            if (order.IsProcessedRetail) {
                                return Ok(new { Success = false, NeedUpdate = false, Message = "Изменение позиции в заказе невозможно." });
                            }
                        }
                    }
                }

                bool zeroFixed = false;

                if (orderItem.Amount <= 0) {
                    orderItem.Amount = 1;
                    zeroFixed = true;
                }

                var maxAmount = await _order1CService.GetMaxItemAmountAsync(orderItem);
                int partsCount = orderItem.PartsCount;
                if (partsCount < 1) partsCount = 1;
                maxAmount = maxAmount / partsCount;

                int linesCount = orderItem.Amount / partsCount;
                if (linesCount <= 0) linesCount = 1;

                if (dir == "dn") {
                    if (linesCount == 1) {
                        if (!zeroFixed) {
                            return Ok(new { Success = true, NeedUpdate = false, Message = "OK" });
                        }
                    } else {
                        if (maxAmount >= 0) {
                            if (linesCount > maxAmount) {
                                linesCount = maxAmount;
                                if (linesCount == 0) linesCount = 1;
                            } else {
                                linesCount--;
                            }
                        } else {
                            linesCount--;
                        }
                    }
                } else {
                    if (!zeroFixed) {
                        if (maxAmount >= 0) {
                            if (linesCount == maxAmount) {
                                return Ok(new { Success = true, NeedUpdate = false, Message = "OK" });
                            } else if (linesCount > maxAmount) {
                                linesCount = maxAmount;
                                if (linesCount == 0) linesCount = 1;
                            } else {
                                linesCount++;
                            }
                        } else {
                            linesCount++;
                        }
                    }
                }
                var oResult = await _order1CService.UpdateOrderItemAmountAsync(orderItem, linesCount);
                if (!oResult.IsSuccess) {
                    return Ok(new { Success = false, NeedUpdate = false, Message = oResult.Messages[0] });
                }

                for (int i = 0; i < order.Details.Count; i++) {
                    if (order.Details[i].Id == orderItem.Id) {
                        order.Details[i].Amount = linesCount * orderItem.PartsCount;
                        break;
                    }
                }

                int orderItemsCount = order.Details.Sum(x => x.Amount);
                int orderTotalAmountInStock = order.Details.Where(x => x.CatalogId != (int)CatalogType.Preorder).Sum(x => x.Amount);
                int orderTotalAmountPreorder = order.Details.Where(x => x.CatalogId == (int)CatalogType.Preorder).Sum(x => x.Amount);
                int totalAmount = linesCount * partsCount;
                string totalPrice = ((double)totalAmount * (orderItem.Price)).ToTwoDecimalPlaces() + " $";

                double valOrderTotalPriceInStock = order.Details.Where(c => c.CatalogId != (int)CatalogType.Preorder).Sum(x => (double)x.Amount * (x.Price));
                double valOrderTotalPricePreorder = order.Details.Where(c => c.CatalogId == (int)CatalogType.Preorder).Sum(x => (double)x.Amount * (x.Price));
                double valOrderTotalPrepayPreorder = valOrderTotalPricePreorder * GlobalConstant.GeneralPrepayPercent;

                string orderTotalPriceInStock = valOrderTotalPriceInStock.ToTwoDecimalPlaces() + " $";
                string orderTotalPricePreorder = valOrderTotalPricePreorder.ToTwoDecimalPlaces() + " $";
                string orderTotalPrepayPreorder = valOrderTotalPrepayPreorder.ToTwoDecimalPlaces() + " $";
                string orderTotalPrice = (valOrderTotalPriceInStock + valOrderTotalPricePreorder).ToTwoDecimalPlaces() + " $";

                return Ok(new {
                    Success = true,
                    NeedUpdate = true,
                    Message = "OK",
                    OrderItemsCount = orderItemsCount,
                    Amount = linesCount,
                    TotalAmount = totalAmount,
                    TotalPrice = totalPrice,
                    OrderTotalPriceInStock = orderTotalPriceInStock,
                    OrderTotalPricePreorder = orderTotalPricePreorder,
                    OrderTotalPrepayPreorder = orderTotalPrepayPreorder,
                    OrderTotalPrice = orderTotalPrice,
                    OrderTotalAmountInStock = orderTotalAmountInStock,
                    OrderTotalAmountPreorder = orderTotalAmountPreorder
                });

            } catch (Exception ex) {
                _logger.Error(ex, "Failed to change order item amount. {message}", ex.Message);

                return Ok(new { Success = false, NeedUpdate = false, Message = "Ошибка на сервере при попытке изменения количество товаров в заказе." });
            }
        }

        public async Task<IActionResult> Requests() {
            ViewBag.ManagersList = new SelectList(await _lookupsService.GetManagersLookupListAsync(), "Value", "Value");
            return View();
        }

        public async Task<IActionResult> LoadDataRequests() {
            try {
                return Ok(await _order1CService.getAllRequestsAsync());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load requests list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка заявок"));
            }
        }

        public async Task<IActionResult> RequestsDetails(int id, [FromServices] IClientReportService reportService) {
            try {
                ViewBag.UserId = id;
                var result = await reportService.GetGroupedUserRequestItemsAsync(id);
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get Request Details (UserId: {id}). {message}", id, ex.Message);
                SetNotification(OperationResult.Failure("Ошибка при получении позиций заявки."));
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Manufacture() {
            ViewBag.ManagersList = new SelectList(await _lookupsService.GetManagersLookupListAsync(), "Value", "Value");
            return View();
        }

        public async Task<IActionResult> LoadDataManufacture() {
            try {
                return Ok(await _order1CService.getAllManufactureAsync());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load manufacture list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка позиций в производстве"));
            }
        }

        public async Task<IActionResult> ManufactureDetails(int id, [FromServices] IClientReportService reportService) {
            try {
                ViewBag.UserId = id;
                return View(await reportService.GetGroupedUserManufactureItemsAsync(id));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get Manufacture Details (UserId: {id}). {message}", id, ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при получении позиций в производстве."));
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Reservations() {
            ViewBag.ManagersList = new SelectList(await _lookupsService.GetManagersLookupListAsync(), "Value", "Value");
            return View();
        }

        public async Task<IActionResult> LoadDataReservations() {
            try {
                return Ok(await _order1CService.getAllReservationsAsync());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load reservations list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка резервов"));
            }
        }

        public async Task<IActionResult> ReservationsDetails(int id, [FromServices] IClientReportService reportService) {
            try {
                ViewBag.UserId = id;
                return View(await reportService.GetGroupedUserReservationsAsync(id));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get Reservations Details (UserId: {id}). {message}", id, ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при получении позиций резервов."));
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Shipings() {
            ViewBag.ManagersList = new SelectList(await _lookupsService.GetManagersLookupListAsync(), "Value", "Value");
            return View();
        }

        public async Task<IActionResult> LoadDataShipings() {
            try {
                return Ok(await _order1CService.getAllShipingsAsync());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load shipings list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка отправок"));
            }
        }

        public async Task<IActionResult> ShipingsDetails(int id, [FromServices] IClientReportService reportService) {
            try {
                ViewBag.UserId = id;
                return View(await reportService.GetGroupedUserShipmentsAsync(id));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get Shipings Details (UserId: {id}). {message}", id, ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при получении позиций отправок."));
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Invoices() {
            ViewBag.ManagersList = new SelectList(await _lookupsService.GetManagersLookupListAsync(), "Value", "Value");
            return View();
        }

        public async Task<IActionResult> LoadDataInvoices() {
            try {
                return Ok(await _order1CService.GetAllInvoicesListAsync());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load invoices list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка отправок"));
            }
        }

        public async Task<IActionResult> InvoiceDetails(int id, int userId) {
            try {
                ViewBag.UserId = id;
                var result = await _order1CService.GetInvoiceByIdAsync(id);
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get Invoice Details (Id: {id}). {message}", id, ex.Message);

                SetNotification(OperationResult.Failure("Ошибка при получении позиций счета на оплату."));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckAsReviewed(int orderId) {
            try {
                var result = await _order1CService.CheckAsReviewedAsync(orderId, UserSession);
                if(!result) {
                    OperationResult.FailureAjax(new List<string>() { "Не удалось отметить заказ как просмотренный.", "Данный заказ пренадлежит другому менеджеру." });
                }
                return Ok(OperationResult.SuccessAjaxRedirect(Url.Action(nameof(Details))));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to check order as reviewed. {message}", ex.Message);
                return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Details))));
            }
        }
    }
}
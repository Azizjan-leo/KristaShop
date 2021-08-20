using System;
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
    public class OrderReportsController : AppControllerBase {
        private readonly IOrder1CService _order1CService;
        private readonly IOrderReportService _reportService;
        private readonly ILookupsService _lookupsService;
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OrderReportsController(IOrder1CService order1CService, IOrderReportService reportService,
            ILookupsService lookupsService, ICityService cityService, IMapper mapper, ILogger logger) {
            _order1CService = order1CService;
            _reportService = reportService;
            _lookupsService = lookupsService;
            _cityService = cityService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> UserUnprocessedOrdersReport(ReportsFilter filter) {
            try {
                var totals = await _reportService.GetOrderTotals(true, filter.SelectedArticuls,
                    filter.SelectedCatalogIds, filter.SelectedUserIds, filter.SelectedColorIds,
                    filter.SelectedCityIds, filter.SelectedManagerIds);

                filter.Articuls = new SelectList(await _reportService.GetUnprocessedOrdersArticulsAsync(true));
                filter.Colors = new SelectList(await _lookupsService.GetColorsLookupListAsync(), "Key", "Value");
                filter.Cities = new SelectList(await _cityService.GetCitiesLookupListAsync(), "Key", "Value");
                filter.Users = new SelectList(await _reportService.GetUnprocessedOrdersUsersAsync(true), "Key", "Value");
                filter.Managers = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();
                filter.Catalogs = LookUpItem.FromEnum<CatalogType>().AsSelectList();

                return View((filter, totals));
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to get User unprocessed orders report. With filter {filter}", filter);
                return BadRequest();
            }
        }

        public async Task<IActionResult> LoadUserUnprocessedOrdersReport(ReportsFilter filter) {
            try {
                return Ok(await _order1CService.GetAllOrdersAsync(unprocessedOnly: true, includeUserCartCheck: true,
                    articuls: filter.SelectedArticuls, catalogIds: filter.SelectedCatalogIds,
                    userIds: filter.SelectedUserIds, colorIds: filter.SelectedColorIds,
                    managerIds: filter.SelectedManagerIds, cityIds: filter.SelectedCityIds));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create unprocessed orders report");
                return BadRequest();
            }
        }

        public async Task<IActionResult> CitiesOrdersReport(OrderReportsFilter filter) {
            var totals = await _reportService.GetOrderTotals(false, filter.SelectedArticuls,
                filter.SelectedCatalogIds, filter.SelectedUserIds, filter.SelectedColorIds,
                filter.SelectedCityIds, filter.SelectedManagerIds);

            filter.Articuls = new SelectList(await _reportService.GetUnprocessedOrdersArticulsAsync(false));
            filter.Colors = new SelectList(await _lookupsService.GetColorsLookupListAsync(), "Key", "Value");
            filter.Cities = new SelectList(await _cityService.GetCitiesLookupListAsync(), "Key", "Value");
            filter.Users = new SelectList(await _reportService.GetUnprocessedOrdersUsersAsync(false), "Key", "Value");
            filter.Managers = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();
            filter.Catalogs = LookUpItem.FromEnum<CatalogType>().AsSelectList();

            return View((filter, totals));
        }

        public async Task<IActionResult> LoadCitiesOrdersReport(OrderReportsFilter filter) {
            try {
                return Ok(await _reportService.GetClientOrdersTotals(filter.OnlyUnprocessedOrders, filter.OnlyNotEmptyUserCarts, filter.SelectedArticuls, filter.SelectedCatalogIds,
                    filter.SelectedUserIds, filter.SelectedColorIds, filter.SelectedCityIds, filter.SelectedManagerIds));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create clients order totals report");
                return BadRequest();
            }
        }

        public async Task<IActionResult> TotalOrdersReport(OrdersTotalFilter filterValues) {
            if (filterValues == null) {
                filterValues = new OrdersTotalFilter();
            }

            filterValues = _correctFilterDateFields(filterValues);

            ViewBag.FilterData = filterValues;

            var reportData = await _order1CService.GetOrdersTotalReportAsync(_mapper.Map<OrdersTotalFilterDTO>(filterValues));

            ViewBag.TotalAmount = reportData.Sum(x => x.Amount);
            ViewBag.TotalSumm = reportData.Sum(x => x.Price * (double) x.Amount).ToTwoDecimalPlaces();

            var uniqColorsList = await _order1CService.GetAllColorsValuesFromOrdersAsync();
            uniqColorsList.Sort((val1, val2) => val1.Name.CompareTo(val2.Name));

            ViewBag.ColorsList = uniqColorsList
                .Select(
                    a => new SelectListItem {
                        Text = a.Name,
                        Value = a.Id.ToString()
                    }
                );

            var uniqSizesList = await _order1CService.GetAllSizesValuesFromOrdersAsync();
            uniqSizesList.Sort(
                (val1, val2) => { return val1.CompareTo(val2); }
            );

            ViewBag.SizesList = uniqSizesList
                .Select(
                    a => new SelectListItem {
                        Text = a,
                        Value = a
                    }
                );

            return View(reportData);
        }
        
        public async Task<IActionResult> TotalPreOrderReport() {
            var reportData = await _order1CService.GetPreorderTotalReportAsync();

            ViewBag.TotalAmount = reportData.Sum(x => x.Amount);
            ViewBag.TotalSumm = reportData.Sum(x => x.Price * (double)x.Amount).ToTwoDecimalPlaces();

            var uniqColorsList = reportData
                .GroupBy(x => x.ColorId)
                .Select(g => g.First())
                .ToList();
            uniqColorsList.Sort((val1, val2) => val1.ColorName.CompareTo(val2.ColorName));

            ViewBag.ColorsList = uniqColorsList
                .Select(
                    a => new SelectListItem {
                        Text = a.ColorName,
                        Value = a.ColorId.ToString()
                    }
                );

            var uniqSizesList = reportData
                .GroupBy(x => x.SizeValue)
                .Select(g => g.First())
                .ToList();
            uniqSizesList.Sort((val1, val2) => val1.SizeValue.CompareTo(val2.SizeValue));

            ViewBag.SizesList = uniqSizesList
                .Select(
                    a => new SelectListItem {
                        Text = a.SizeValue,
                        Value = a.SizeValue
                    }
                );


            return View(reportData);
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
    }
}

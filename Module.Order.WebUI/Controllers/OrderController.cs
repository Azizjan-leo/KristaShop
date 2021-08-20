using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Infrastructure;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Models;
using Serilog;

namespace Module.Order.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.AllFrontSchemes)]
    [Route("[controller]/[action]")]
    [Route("Personal/[action]")] // support old route
    public class OrderController : AppControllerBase {
        private readonly IClientReportService _reportService;
        private readonly ILogger _logger;

        public OrderController(IClientReportService reportService, ILogger logger) {
            _reportService = reportService;
            _logger = logger;
        }
        
        public async Task<IActionResult> Index([FromServices] IOrder1CService order1CService) {
            try {
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) return RedirectToAction("Common403", "Error");

                var result = await _reportService.GetUserUnprocessedItemsAsync(UserSession.UserId);

                try {
                    ViewBag.InvoicesList = await order1CService.GetInvoicesListByUserIdAsync(UserSession.UserId, false);
                } catch (Exception exIn) {
                    ViewBag.InvoicesList = new List<InvoiceDTO>();

                    _logger.Error(exIn, "Failed to get invoices list (userId: {userId}). {message}", UserSession.UserId, exIn.Message);
                }

                SetBreadcrumbs(ControllerName, ActionName, "Личный кабинет");
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get complex order info list (userId: {userId}). {message}", UserSession.UserId, ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> Invoices([FromServices] IOrder1CService order1CService) {
            try {
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) return RedirectToAction("Common403", "Error");

                try {
                    ViewBag.InvoicesList = await order1CService.GetInvoicesListByUserIdAsync(UserSession.UserId);
                } catch (Exception exIn) {
                    ViewBag.InvoicesList = new List<InvoiceDTO>();

                    _logger.Error(exIn, "Failed to get all invoices list (userId: {userId}). {message}", UserSession.UserId, exIn.Message);
                }

                return View();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get all invoices list (userId: {userId}). {message}", UserSession.UserId, ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> InvoiceDetails(int id, [FromServices] IOrder1CService order1CService) {
            try {
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) return RedirectToAction("Common403", "Error");

                var invoice = await order1CService.GetInvoiceByIdAsync(id);
                if (invoice == null) {
                    SetNotification(OperationResult.Failure("Счет не найден в БД."));
                    return RedirectToAction(nameof(Index));
                } else if (invoice.UserId != UserSession.UserId) {
                    return RedirectToAction("Common403", "Error");
                }

                return View(invoice);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get invoice details (userId: {userId}). {message}", UserSession.UserId, ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> InvoiceDownload(int id, [FromServices] IOrder1CService order1CService) {
            try {
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) return RedirectToAction("Common403", "Error");

                var invoice = await order1CService.GetInvoiceByIdAsync(id);
                if (invoice == null) {
                    SetNotification(OperationResult.Failure("Счет не найден в БД."));
                    return RedirectToAction(nameof(Index));
                } else if (invoice.UserId != UserSession.UserId) {
                    return RedirectToAction("Common403", "Error");
                }

                if (!invoice.HasAttachedFile) {
                    return BadRequest();
                }

                var fileName = invoice.AttachedFullFileName;
                if (string.IsNullOrEmpty(fileName)) {
                    return BadRequest();
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
                var outFileName = invoice.AttachedBriefFileName;
                return File(fileBytes, "application/force-download", outFileName);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get invoice details (userId: {userId}). {message}", UserSession.UserId, ex.Message);
                return BadRequest();
            }
        }
        
        
        public async Task<IActionResult> ShipmentDetails([FromServices]IRootFileService fileService) {
            try {
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) return RedirectToAction("Common403", "Error");
                var result = await _reportService.GetGroupedUserShipmentsAsync(UserSession.UserId);
                var directoriesNames = result.Items.Select(x => x.Name).ToList();
                var directoriesFiles = fileService.GetFilesInDirectories(fileService.Settings.ClientReportsFilesPath, directoriesNames, UserSession.UserId);
                
                SetBreadcrumbs(ControllerName, ActionName, "Личный кабинет");
                SetLayout("_PersonalLayout");
                return View((result, directoriesFiles));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get user shimpment items. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> ShipmentDetailsPrint(string date) {
            try {
                var shipmentDate = DateTime.ParseExact(date, DateTimeExtension.BasicFormatter, CultureInfo.InvariantCulture);
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) return RedirectToAction("Common403", "Error");
                var result = await _reportService.GetGroupedUserShipmentsAsync(UserSession.UserId, shipmentDate);
                
                SetLayout("_PrintLayout");
                return View("ShipmentDetails", (result, new Dictionary<string, string[]>()));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get user shimpment items to print. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> GetShipmentFile(string filePath, [FromServices]IRootFileService fileService) {
            try {
                var file = await fileService.GetFileAsync(fileService.Settings.ClientReportsFilesPath, filePath);
                if (file == null) {
                    return BadRequest(OperationResult.Failure("Не удалось загузить данный файл"));
                }
                
                return Ok(file);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to download shipment file. Path: {path} {message}", filePath, ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> OrdersHistory() {
            try {
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) return RedirectToAction("Common403", "Error");
                SetBreadcrumbs(ControllerName, ActionName, "Личный кабинет");
                return View(await _reportService.GetUserHistoryItemsAsync(UserSession.UserId));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get user shimpment items. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> ProcessingOrder([FromServices] IRootFileService fileService) {
            try {
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0) return RedirectToAction("Common403", "Error");
                var result = await _reportService.GetUserComplexOrderAsync(UserSession.UserId);
                var directoriesNames = result.Shipments.Items.SelectMany(x => x.Items.Select(c => c.Name)).ToList();
                var directoriesFiles = fileService.GetFilesInDirectories(fileService.Settings.ClientReportsFilesPath, directoriesNames, UserSession.UserId);

                SetBreadcrumbs(ControllerName, ActionName, "Личный кабинет");
                return View((result, directoriesFiles));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get user complex items. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> MoneyReport() {
            try {
                var result = await _reportService.GetUserMoneyReportsAsync(UserSession.UserId);
                SetBreadcrumbs(ControllerName, ActionName, "Личный кабинет");
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get user money report. {message}", ex.Message);
                return RedirectToAction("Common500", "Error");
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.Common.Admin.Admin.Filters;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Partners.Business.Interfaces;
using Serilog;

namespace Module.Partners.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class PartnershipController : AppControllerBase {
        private readonly IPartnerService _partnerService;
        private readonly ICityService _cityService;
        private readonly ILogger _logger;
        private readonly ILookupsService _lookupsService;

        public PartnershipController(IPartnerService partnerService, ICityService cityService,
            ILogger logger, ILookupsService lookupsService) {
            _partnerService = partnerService;
            _cityService = cityService;
            _logger = logger;
            _lookupsService = lookupsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            return await Index(new PartnersFilter());
        }

        [HttpPost]
        public async Task<IActionResult> Index(PartnersFilter filter) {
            try {
                filter.CitiesSelect = await _cityService.GetCitiesLookupListAsync().AsSelectListAsync();
                filter.PartnersSelect = await _partnerService.GetPartnersLookUpAsync().AsSelectListAsync();
                filter.ManagersSelect = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();
                var partners = await _partnerService.GetAllPartnersAsync(filter);
                return View((partners, filter));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load users view. {message}", ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> SalesReport() {
            return await SalesReport(new ReportsFilter());
        }

        [HttpPost]
        public async Task<IActionResult> SalesReport(ReportsFilter filter) {
            try {
                filter.Articuls = new SelectList(await _lookupsService.GetArticulsListAsync());
                filter.Colors = await _lookupsService.GetColorsLookupListAsync().AsSelectListAsync();
                filter.Cities = await _cityService.GetCitiesLookupListAsync().AsSelectListAsync();
                filter.Users = await _partnerService.GetPartnersLookUpAsync().AsSelectListAsync();
                filter.Managers = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();
                filter.Catalogs = LookUpItem.FromEnum<CatalogType>().AsSelectList();

                var result = await _partnerService.GetSalesReportAsync(filter);
                return View((filter, result));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get load partner sales report. {message}", ex.Message);
                return BadRequest();
            }
        }  
        
        public async Task<IActionResult> PartnerSalesDecryptedReport(ReportsFilter filter) {
            try {
                filter.Articuls = new SelectList(await _lookupsService.GetArticulsListAsync());
                filter.Colors = await _lookupsService.GetColorsLookupListAsync().AsSelectListAsync();
                filter.Cities = await _cityService.GetCitiesLookupListAsync().AsSelectListAsync();
                filter.Users = await _partnerService.GetPartnersLookUpAsync().AsSelectListAsync();
                filter.Managers = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();
                filter.Catalogs = LookUpItem.FromEnum<CatalogType>().AsSelectList();

                var result = await _partnerService.GetDecryptedSalesReportAsync(filter);
                return View((filter, result));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get load partner sales decripted report. {message}", ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApprovePartnershipRequest(Guid id) {
            try {
                var result = await _partnerService.ApproveRequestAsync(id);
                SetNotification(result);
                return Ok();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to approve partnership request (id: {id}). {message}", id, ex.Message);
                return BadRequest(OperationResult.Failure("Не удалось удовлетворить заявку на партнерство"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> MakePartner(int userId) {
            try {
                await _partnerService.MakePartnerAsync(userId);
                return Ok(OperationResult.Success("Статус пользователя изменен на 'Партнер'"));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to make {userId} partner. {message}", userId, ex.Message);
                return BadRequest(OperationResult.Failure("Не удалось сделать клиента партнером"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePartnershipRequest(Guid id) {
            try {
                var result = await _partnerService.DeleteRequestAsync(id);
                SetNotification(result);
                return Ok(new object());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to approve partnership request (id: {id}). {message}", id, ex.Message);
                return BadRequest(OperationResult.Failure("Не удалось удовлетворить заявку на партнерство"));
            }
        }  
        
        [HttpPost]
        public async Task<IActionResult> AcceptPartnershipRequestToProcess(Guid id) {
            try {
                var result = await _partnerService.AcceptRequestToProcessAsync(id);
                SetNotification(result);
                return Ok(new object());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to approve partnership request (id: {id}). {message}", id, ex.Message);
                return BadRequest(OperationResult.Failure("Не удалось выполнить операцию!"));
            }
        }

        public async Task<IActionResult> UserActivePayments(int userId, [FromServices] IPartnerDocumentsService documentsService) {
            try {
                if (userId <= 0) {
                    SetNotification(OperationResult.Failure("Партнер не найден"));
                    return RedirectToAction(nameof(Index));
                }

                var result = await documentsService.GetNotPaidPaymentDocumentsAsync(userId);
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load user's active payments userId: {id}. {message}", userId, ex.Message);
                SetNotification(OperationResult.Failure("Произошла ошибка при получении списка счетов партнера"));
                return RedirectToAction(nameof(Index), new {userId});
            }
        }

        [HttpGet]
        public async Task<IActionResult> PaymentsHistory([FromServices] IPartnerDocumentsService documentsService) {
            return await PaymentsHistory(new PaymentsReportFilter {
                Date = DateRange.FromLastMonth()
            }, documentsService);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentsHistory(PaymentsReportFilter filter, [FromServices] IPartnerDocumentsService documentsService) {
            try {
                filter.CitiesSelect = await _cityService.GetCitiesLookupListAsync().AsSelectListAsync();
                filter.PartnersSelect = await _partnerService.GetPartnersLookUpAsync().AsSelectListAsync();
                filter.ManagersSelect = await _lookupsService.GetManagersLookupListAsync().AsSelectListAsync();
                filter.DocumentTypesSelect = documentsService.GetPayableDocumentsLookup().AsSelectList();
                var result = await documentsService.GetPaidPaymentDocumentsByManagerAsync(UserSession, filter);
                return View((result, filter));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load paid payments by manager: {managerId}. {message}", UserSession.ManagerId, ex.Message);
                SetNotification(OperationResult.Failure("Произошла ошибка при получении списка счетов партнера"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePayment(int userId, Guid documentId, [FromServices] IPartnerDocumentsService documentsService) {
            try {
                if (documentId.IsEmpty() || userId <= 0) {
                    SetNotification(OperationResult.Failure("Документ не найден"));
                    return RedirectToAction(nameof(UserActivePayments), new {userId});
                }
                
                await documentsService.UpdatePaymentDocumentStatusAsync(documentId);
                SetNotification(OperationResult.Success("Счет партнера оплачен"));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create payment document for user: {id}. {message}", userId, ex.Message);
                SetNotification(OperationResult.Failure("Произошла ошибка при выставлении счета партнеру"));
            }

            return RedirectToAction(nameof(UserActivePayments), new {userId});
        }
    }
}

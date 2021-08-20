using System;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.App.Admin.Admin.Models;
using Module.App.Business.Interfaces;
using Module.Client.Business.Interfaces;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Order.Business.Interfaces;
using Serilog;

namespace Module.App.Admin.Admin.Controllers {
    [Area("Admin")]
    [PermissionFilter]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class DashboardController : AppControllerBase {
        private readonly IRegistrationService _registrationService;
        private readonly IOrder1CService _orderService;
        private readonly ILogger _logger;
        private readonly IFeedbackService _feedbackService;

        public DashboardController(IRegistrationService registrationService, IOrder1CService orderService, ILogger logger, IFeedbackService feedbackService) {
            _registrationService = registrationService;
            _orderService = orderService;
            _logger = logger;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> GetTopMenuValues() {
            try {
                var newUsersCount = UserSession.IsRoot
                    ? await _registrationService.GetRequestsCountAsync()
                    : await _registrationService.GetRequestsCountAsync(UserSession.ManagerId);

                var ordersStats = 
                    UserSession.IsRoot
                    ? await _orderService.getUnprocessedOrdersStatsAsync()
                    : await _orderService.getUnprocessedOrdersStatsAsync(UserSession.ManagerId);

                var newFeedbacks = await _feedbackService.GetNewFeedbacksCount();

                return Ok(new TopMenuDashboardViewModel(newUsersCount, ordersStats.oCount, newFeedbacks));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get top menu dashboard. {message}", ex.Message);
                return BadRequest(OperationResult.Failure());
            }
        }
    }
}

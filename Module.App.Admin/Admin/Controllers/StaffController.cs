using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.App.Admin.Admin.Models;
using Module.App.Business.Interfaces;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.App.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class StaffController : AppControllerBase {
        private readonly IManagerService _managerService;
        private readonly IRoleAccessService _roleAccessService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public StaffController(IManagerService managerService, IRoleAccessService roleAccessService, IMapper mapper, ILogger logger) {
            _managerService = managerService;
            _roleAccessService = roleAccessService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            try {
                return View(await _managerService.GetManagersAsync());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get staff list {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(int id) {
            try {
                var details = await _managerService.GetManagerAsync(id);
                var model = _mapper.Map<ManagerDetailsViewModel>(details);
                var managers = await _managerService.GetManagersAsync();
                model.Managers = new SelectList(managers, "Id", "Name");
                model.Roles = new SelectList(await _roleAccessService.GetRolesAsync(), "Id", "Name");
                model.ManagerIdsForOrdersAccess = details.GetManagerIdsAccessesFor(ManagerAccessToType.Orders);
                model.ManagerIdsForRegistrationsAccess = details.GetManagerIdsAccessesFor(ManagerAccessToType.Users);
                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get manager to edit", ex.Message);
                SetNotification(OperationResult.Failure());
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManagerDetailsViewModel model) {
            try {
                if (!ModelState.IsValid) {
                    var managers = await _managerService.GetManagersAsync();
                    model.Managers = new SelectList(managers, "Id", "Name");
                    model.Roles = new SelectList(await _roleAccessService.GetRolesAsync(), "Id", "Name");
                    return View(model);
                }

                var managerDetails = _mapper.Map<ManagerDetailsDTO>(model);
                var accesses = model.ManagerIdsForOrdersAccess.Select(x => new ManagerAccessDTO() { ManagerId = model.Id, AccessTo = ManagerAccessToType.Orders, AccessToManagerId = x });
                accesses = accesses.Concat(model.ManagerIdsForRegistrationsAccess.Select(x => new ManagerAccessDTO() { ManagerId = model.Id, AccessTo = ManagerAccessToType.Users, AccessToManagerId = x }));
                managerDetails.Accesses = accesses;

                await _managerService.UpdateManagerAsync(managerDetails);
                SetNotification(OperationResult.Success());
                return RedirectToAction("Index");
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to edit manager", ex.Message);
                SetNotification(OperationResult.Failure());
                return RedirectToAction("Index");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.App.Admin.Admin.Models;
using Module.App.Business.Interfaces;
using Module.App.Business.Models;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.App.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class AccessController : AppControllerBase {
        private readonly IRoleAccessService _roleAccessService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccessController(IRoleAccessService roleAccessService, IMapper mapper, ILogger logger) {
            _roleAccessService = roleAccessService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            try {
                var items = await _roleAccessService.GetRolesAsync();
                return View(items);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get roles list. {message}", ex.Message);
                return BadRequest();
            }
        }

        public IActionResult CreateRole() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleViewModel model) {
            try {
                if (!ModelState.IsValid) {
                    return View(model);
                }
                await _roleAccessService.CreateRoleAsync(_mapper.Map<RoleDTO>(model));
                SetSuccessNotification();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create role. {message}", ex.Message);
                SetFailureNotification();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditRole(Guid id) {
            try {
                return View(_mapper.Map<RoleViewModel>(await _roleAccessService.GetRoleAsync(id)));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get role for edit. {message}", ex.Message);
                SetFailureNotification();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RoleViewModel model) {
            try {
                if (!ModelState.IsValid) {
                    return View(model);
                }
                await _roleAccessService.UpdateRoleAsync(_mapper.Map<RoleDTO>(model));
                SetSuccessNotification();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to edit role. {message}", ex.Message);
                SetFailureNotification();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditRoleAccess(Guid roleId) {
            try {
                var accesses = _mapper.Map<List<RoleAccessViewModel>>(await _roleAccessService.GetRoleAccessesAsync(roleId));
                
                var newAccesses = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.GetName().FullName.Contains("Module"))
                    .SelectMany(x => x.GetTypes())
                    .Concat(Assembly.GetExecutingAssembly().GetTypes())
                    .Where(type => type.Namespace != null && Attribute.IsDefined(type, typeof(AreaAttribute)) &&
                                   ((AreaAttribute) Attribute.GetCustomAttribute(type, typeof(AreaAttribute))).RouteValue == "Admin")
                    .Where(type => typeof(AppControllerBase).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods())
                    .Where(method =>
                        method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) &&
                        method.DeclaringType == method.ReflectedType)
                    .Select(method => new RoleAccessViewModel() {
                        Area = _getAreaNameForControllerType(method.DeclaringType),
                        Controller = method.DeclaringType.Name.Replace("Controller", ""),
                        Action = method.Name,
                        IsAccessGranted = false,
                        RoleId = roleId,
                        Description = ""
                    })
                    .Distinct()
                    .Where(x => !accesses.Any(a =>
                        a.Area.Equals(x.Area) && a.Controller.Equals(x.Controller) && a.Action.Equals(x.Action)))
                    .ToList();
                accesses.AddRange(newAccesses);
                return View(accesses);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get role accesses. {message}", ex.Message);
                SetFailureNotification();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRoleAccess([FromBody]List<RoleAccessViewModel> model) {
            try {
                if (!ModelState.IsValid) {
                    return View(model);
                }

                await _roleAccessService.UpdateRoleAccessesAsync(_mapper.Map<List<RoleAccessDTO>>(model));
                return Ok(OperationResult.SuccessAjaxRedirect(Url.Action(nameof(Index))));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get role accesses. {message}", ex.Message);
                SetFailureNotification();
                return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
            }
        }

        private string _getAreaNameForControllerType(Type controllerType) {
            var areaAttribute = controllerType.GetCustomAttribute(typeof(AreaAttribute)) as AreaAttribute;
            if (areaAttribute == null) return "";
            return areaAttribute.RouteValue;
        }
    }
}
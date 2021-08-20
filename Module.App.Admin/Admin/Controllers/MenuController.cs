using System;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class MenuController : AppControllerBase {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public MenuController(IMenuService menuService, IMapper mapper, ILogger logger) {
            _menuService = menuService;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> LoadData() {
            return Ok(await _menuService.GetMenuItemsAsync());
        }

        public async Task<IActionResult> Create() {
            try {
                var model = new MenuItemViewModel {
                    MenuItems = new SelectList(await _menuService.GetMenuItemsByTypeAsync(MenuType.Left), "Id", "Title")
                };
                return View(model);
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to get MenuItemViewModel for create view. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при создании нового пункта меню"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuItemViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var menuItem = _mapper.Map<MenuItemDTO>(model);
                    var result = await _menuService.InsertMenuItemAsync(menuItem);
                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to create MenuItem. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при создании нового пункта меню");
            }

            model.MenuItems = new SelectList(await _menuService.GetMenuItemsByTypeAsync(MenuType.Left), "Id", "Title");
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id) {
            try {
                if (id.IsEmpty())
                    return NotFound();

                var menuItem = await _menuService.GetMenuItemAsync(id);
                if (menuItem == null)
                    return NotFound();

                var model = _mapper.Map<MenuItemViewModel>(menuItem);
                model.MenuItems = new SelectList(await _menuService.GetMenuItemsByTypeAsync(MenuType.Left), "Id", "Title");
                return View(model);
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to get MenuItem for edit. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при создании нового пункта меню"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuItemViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var menuItem = _mapper.Map<MenuItemDTO>(model);
                    var result = await _menuService.UpdateMenuItemAsync(menuItem);
                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to edit MenuItem. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при изменении пункта меню");
            }

            model.MenuItems = new SelectList(await _menuService.GetMenuItemsByTypeAsync(MenuType.Left), "Id", "Title");
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id) {
            try {
                if (id == null) {
                    return NotFound();
                }

                var menuItem = await _menuService.GetMenuItemAsync(id.Value);
                if (menuItem == null) {
                    return NotFound();
                }

                var model = _mapper.Map<MenuItemViewModel>(menuItem);
                return View(model);
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to get MenuItem for delete. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при получении пункта меню"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id) {
            try {
                var result = await _menuService.DeleteMenuItemAsync(id);
                SetNotification(result);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                _logger.Error(ex, "Failed to delete MenuItem. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при удалении пункта меню"));
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
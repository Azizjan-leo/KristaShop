using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;
using Module.Media.Admin.Admin.Models;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.Admin.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class MBodyController : AppControllerBase
    {
        private readonly IDynamicPagesService _dynamicPagesService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly GlobalSettings _globalSettings;

        public MBodyController(IDynamicPagesService dynamicPagesService, IMapper mapper, IOptions<GlobalSettings> options, ILogger logger)
        {
            _dynamicPagesService = dynamicPagesService;
            _mapper = mapper;
            _logger = logger;
            _globalSettings = options.Value;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> LoadData() {
            var result = await _dynamicPagesService.GetPagesAsync();
            return Ok(_mapper.Map<List<DynamicPageViewModel>>(result));
        }

        public IActionResult Create() {
            try {
                var model = new DynamicPageViewModel {Layouts = _getLayoutsAsSelectList()};
                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get menu content for create. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при открытии формы создания контент страницы"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DynamicPageViewModel model) {
            try {
                if (ModelState.IsValid)
                {
                    var page = _mapper.Map<DynamicPageDTO>(model);
                    page.TitleIcon = await model.TitleIcon.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath, _globalSettings.MenuContentDirectory);
                    page.Image = await model.Image.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath, _globalSettings.MenuContentDirectory);
                    var result = await _dynamicPagesService.InsertPageAsync(page);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }

                model.Layouts = _getLayoutsAsSelectList();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create menu content. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при создании контент страницы");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id = default)
        {
            try {
                if (id == Guid.Empty)
                    return NotFound();

                var page = await _dynamicPagesService.GetPageByIdAsync(id);
                if (page == null)
                    return NotFound();

                var model = _mapper.Map<DynamicPageViewModel>(page);
                model.Layouts = _getLayoutsAsSelectList();
                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get menu content for edit. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при получении контент страницы для редактирования"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DynamicPageViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var page = _mapper.Map<DynamicPageDTO>(model);
                    page.TitleIcon = await model.TitleIcon.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath, _globalSettings.MenuContentDirectory);
                    page.Image = await model.Image.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath, _globalSettings.MenuContentDirectory);
                    var result = await _dynamicPagesService.UpdatePageAsync(page);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }
                model.Layouts = _getLayoutsAsSelectList();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to edit menu content. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при редактировании контент страницы");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id) {
            try {
                if (id == Guid.Empty)
                    return NotFound();

                var dto = await _dynamicPagesService.GetPageByIdAsync(id);
                var model = _mapper.Map<DynamicPageViewModel>(dto);
                if (model == null)
                    return NotFound();

                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get menu content for delete. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при получении контент страницы для удаления"));
                return RedirectToAction(nameof(Index));
            }
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id) {
            try {
                var result = await _dynamicPagesService.DeletePageAsync(id, _globalSettings.FilesDirectoryPath);
                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete menu content. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при удалении контент страницы"));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task UpdateRow(Guid id, int toPosition) {
            try {
                await _dynamicPagesService.UpdatePageOrderAsync(id, toPosition);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update menu content order. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при обновлении позиции контент страницы"));
            }
        }

        private SelectList _getLayoutsAsSelectList() {
            var layouts = DynamicPageLayout.GetLayouts();
            return new SelectList(layouts, nameof(DynamicPageLayout.Name), nameof(DynamicPageLayout.Title), layouts.FirstOrDefault(x => x.IsDefault)?.Name ?? string.Empty);
        }

        public async Task<IActionResult> ReloadCache([FromServices] IDynamicPagesManager dynamicPagesManager) {
            try {
                await dynamicPagesManager.ReloadAsync();
                SetNotification(OperationResult.Success("Кэш успешно обновлен"));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update dynamic pages cache. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Не удалось обновить кэш. Возникла ошибка при обновлении кэша"));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
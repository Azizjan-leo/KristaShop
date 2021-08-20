using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.Admin.Admin.Filters;
using Module.Common.Admin.Admin.Models;
using Module.Common.Business.Interfaces;
using Module.Common.Business.Models;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;
using Serilog;

namespace Module.Common.Admin.Admin.Controllers {
    [Area("Admin")]
    [PermissionFilter]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class SettingsController : AppControllerBase {
        private readonly ISettingsService _settingsService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SettingsController(ISettingsService settingsService, IMapper mapper, ILogger logger) {
            _settingsService = settingsService;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index() {
            ViewData["IsRoot"] = UserSession.IsRoot;
            return View();
        }

        public async Task<IActionResult> LoadData() {
            var result = await _settingsService.GetSettingsAsync(UserSession.IsRoot);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SettingEditViewModel model) {
            try {
                if (!UserSession.IsRoot)
                    return Forbid();

                if (!ModelState.IsValid) {
                    return BadRequest(ModelState.Values.ErrorsAsOperationResult());
                }

                var dto = _mapper.Map<SettingsDTO>(model);
                var result = await _settingsService.InsertAsync(dto);
                if (result.IsSuccess)
                    return Ok(result);
                else
                    return BadRequest(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create settings {@model}. {message}", model, ex.Message);
                ModelState.AddModelError("", "Не удалось создать запись");
                return BadRequest(ModelState.Values.ErrorsAsOperationResult());
            }
           
        }

        public async Task<IActionResult> Details(Guid id) {
            return Ok(_mapper.Map<SettingEditViewModel>(await _settingsService.GetByIdAsync(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SettingEditViewModel model) {
            try {
                if (model.Value == null) model.Value = "";

                if (!ModelState.IsValid) {
                    return BadRequest(ModelState.Values.ErrorsAsOperationResult());
                }

                var dto = _mapper.Map<SettingsDTO>(model);
                var result = await _settingsService.UpdateAsync(dto, UserSession.IsRoot);
                if (result.IsSuccess)
                    return Ok(result);
                else
                    return BadRequest(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to edit settings {@model}. {message}", model, ex.Message);

                ModelState.AddModelError("", "Не удалось изменить запись");
                return BadRequest(ModelState.Values.ErrorsAsOperationResult());
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) {
            try {
                if (!UserSession.IsRoot)
                    return Forbid();

                return Ok(await _settingsService.DeleteAsync(id));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete settings with id = {id}. {message}", id, ex.Message);

                ModelState.AddModelError("", "Не удалось удалить запись");
                return BadRequest(ModelState.Values.ErrorsAsOperationResult());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Module.App.Admin.Admin.Models;
using Module.App.Business.Interfaces;
using Module.Common.Admin.Admin.Filters;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.App.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class FeedbackController : AppControllerBase {
        private readonly IFeedbackService _feedbackService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly GlobalSettings _globalSettings;

        public FeedbackController(IFeedbackService feedbackService, IFileService fileService, IOptions<GlobalSettings> settings, IMapper mapper, ILogger logger) {
            _feedbackService = feedbackService;
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
            _globalSettings = settings.Value;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> LoadData() => Ok(await _feedbackService.GetFeedbackListAsync());

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(OperationResult.Failure(errors.ToList()));
            }

            var result = await _feedbackService.UpdateFeedback(id, Guid.Empty);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        public async Task<IActionResult> GetFiles(Guid id) {
            var result = _mapper.Map<List<FileViewModel>>(await _feedbackService.GetFilesByFeedbackIdAsync(id));

            ViewData["LinkAction"] = nameof(DownloadFile);
            return PartialView("_FilesPartial", result);
        }

        public async Task<IActionResult> DownloadFile(Guid id) {
            var file = await _feedbackService.GetFileAsync(id);
            if (file == null) {
                SetNotification(OperationResult.Failure(new List<string> { "Файл не найден." }));
                return RedirectToAction(nameof(Index));
            }

            var fileArray = await _fileService.GetFileAsync(_globalSettings.FilesDirectoryPath, file.VirtualPath);
            if (fileArray == null) {
                SetNotification(OperationResult.Failure(new List<string> { "Не удалось загрузить данный файл." }));
                return RedirectToAction(nameof(Index));
            }


            return File(fileArray, FileTypeExtensions.GetOctetStreamType(), file.Filename);
        }

        public async Task<IActionResult> Delete(Guid id) {
            try {
                var result = await _feedbackService.DeleteFeedbackAsync(id);
                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete feedback id: {id}. {message}", id, ex.Message);
                SetNotification(OperationResult.Failure("Произошла ошибка во время удаления"));
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAll(bool viewedOnly = true) {
            try {
                var result = await _feedbackService.DeleteFeedbackAsync(viewedOnly);
                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete viewed feedback id. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Произошла ошибка во время удаления"));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
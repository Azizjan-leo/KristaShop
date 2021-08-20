using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;
using Module.Media.Admin.Admin.Models;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.Admin.Admin.Controllers {
    [Area("Admin")]
    [PermissionFilter]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class VideoGalleryController : AppControllerBase {
        private readonly IMapper _mapper;
        private readonly IVideoGalleryService _galleryService;
        private readonly GlobalSettings _globalSettings;
        private readonly ILogger _logger;

        public VideoGalleryController(IMapper mapper, IVideoGalleryService galleryService,
            IOptions<GlobalSettings> settings, ILogger logger) {
            _mapper = mapper;
            _galleryService = galleryService;
            _globalSettings = settings.Value;
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> LoadData() {
            try {
                return Ok(_mapper.Map<List<VideoGalleryViewModel>>(
                    await _galleryService.GetGalleriesAsync(false, false)));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get video galleries list. {message}", ex.Message);
                SetNotification(OperationResult.Failure($"Возникла ошибка при получении списка галерей"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> SwitchVisibility(Guid id) {
            try {
                if (id == Guid.Empty) return NotFound();

                var result = await _galleryService.SwitchGalleryVisabilityAsync(id);
                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get video gallery item for switching visability. {message}", ex.Message);
                SetNotification(
                    OperationResult.Failure(
                        "Возникла ошибка при получении элемента видеогалереи для смены его видимости"));
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VideoGalleryViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var gallery = _mapper.Map<VideoGalleryDTO>(model);
                    gallery.Preview = await model.Preview.ConvertToFileDataProviderAsync(
                        _globalSettings.FilesDirectoryPath, _globalSettings.VideoGalleryPreviewsDirectory);
                    gallery.VideoPath = UrlHelper.YoutubeUrlToEmbed(gallery.VideoPath);
                    var result = await _galleryService.InsertGalleryAsync(gallery);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create video gallery. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, $"Возникла ошибка при создании галереи");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id = default) {
            try {
                if (id == Guid.Empty) return NotFound();

                var gallery = await _galleryService.GetGalleryAsync(id, false, false);
                if (gallery == null) return NotFound();

                var model = _mapper.Map<VideoGalleryViewModel>(gallery);
                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get video gallery for edit. {message}", ex.Message);
                SetNotification(OperationResult.Failure($"Возникла ошибка при получении галереи для изменения"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VideoGalleryViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var gallery = _mapper.Map<VideoGalleryDTO>(model);
                    gallery.Preview = await model.Preview.ConvertToFileDataProviderAsync(
                        _globalSettings.FilesDirectoryPath, _globalSettings.VideoGalleryPreviewsDirectory);
                    gallery.VideoPath = UrlHelper.YoutubeUrlToEmbed(gallery.VideoPath);
                    var result = await _galleryService.UpdateGalleryAsync(gallery);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to edit video gallery. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, $"Возникла ошибка при изменении галереи");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id = default) {
            try {
                if (id == Guid.Empty) return NotFound();

                var gallery = await _galleryService.GetGalleryAsync(id, false, false);
                if (gallery == null) return NotFound();

                var model = _mapper.Map<VideoGalleryViewModel>(gallery);
                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get video gallery for delete. {message}", ex.Message);
                SetNotification(OperationResult.Failure($"Возникла ошибка при получении галереи для удаления"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGallery(Guid id) {
            try {
                var result = await _galleryService.DeleteGalleryAsync(id, _globalSettings.FilesDirectoryPath);

                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete video gallery. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, $"Возникла ошибка при удалении галереи");
                BadRequest(ModelState);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRow(Guid id, int toPosition) {
            try {
                var gallery = await _galleryService.GetGalleryAsync(id, false, false);
                gallery.Order = toPosition;
                var result = await _galleryService.UpdateGalleryAsync(gallery);
                SetNotification(result);
                return Ok();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update video gallery order. {message}", ex.Message);
                SetNotification(OperationResult.Failure($"Возникла ошибка при обновлении позиции галлереи"));
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
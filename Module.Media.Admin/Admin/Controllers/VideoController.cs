using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Helpers;
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

namespace Module.Media.Admin.Admin.Controllers {
    [Area("Admin")]
    [PermissionFilter]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class VideoController : AppControllerBase {
        private readonly IMapper _mapper;
        private readonly IVideoGalleryService _galleryService;
        private readonly GlobalSettings _settings;
        private readonly ILogger _logger;

        public VideoController(IMapper mapper, IVideoGalleryService galleryService, IOptions<GlobalSettings> settings,
            ILogger logger) {
            _mapper = mapper;
            _galleryService = galleryService;
            _settings = settings.Value;
            _logger = logger;
        }

        public IActionResult Index(Guid galleryId = default) {
            ViewData["GalleryId"] = galleryId;
            return View();
        }

        public async Task<IActionResult> LoadData(Guid galleryId = default) {
            try {
                var result = galleryId == Guid.Empty ?
                    await _galleryService.GetVideosAsync(false) :
                    await _galleryService.GetVideosByGalleryAsync(galleryId, false);

                return Ok(_mapper.Map<List<VideoViewModel>>(result));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get videos list. {message}", ex.Message);

                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка видео"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRow(Guid videoId, Guid galleryId, int toPosition) {
            try {
                var result = await _galleryService.UpdateVideoOrder(galleryId, videoId, toPosition);
                //SetNotification(result);
                return Ok(new object());

                //return Ok(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update video order. {message}", ex.Message);

                SetNotification(OperationResult.Failure("Возникла ошибка при обновлении позиции видео"));
                return Problem(
                    OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index), new {galleryId = galleryId})));
            }
        }

        public async Task<IActionResult> Create(Guid galleryId = default) {
            var model = new VideoViewModel();
            if (galleryId != Guid.Empty) {
                model.GalleryIds.Add(galleryId);
                model.FromGalleryId = galleryId;
            }

            model.Galleries = new SelectList(await _galleryService.GetGalleriesAsync(false, false),
                nameof(VideoGalleryDTO.Id), nameof(VideoGalleryDTO.Title));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VideoViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var video = _mapper.Map<VideoDTO>(model);
                    video.Preview = await model.Preview.ConvertToFileDataProviderAsync(_settings.FilesDirectoryPath,
                        _settings.VideoPreviewsDirectory);
                    video.VideoPath = UrlHelper.YoutubeUrlToEmbed(video.VideoPath);
                    var result = await _galleryService.InsertVideoAsync(video);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index), new {galleryId = model.FromGalleryId});
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create video. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, $"Возникла ошибка при создании видео");
            }

            model.Galleries = new SelectList(await _galleryService.GetGalleriesAsync(false, false),
                nameof(VideoGalleryDTO.Id), nameof(VideoGalleryDTO.Title));
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id = default, Guid galleryId = default) {
            try {
                if (id == Guid.Empty) return NotFound();

                var video = await _galleryService.GetVideoAsync(id, false);
                if (video == null) return NotFound();

                var model = _mapper.Map<VideoViewModel>(video);
                model.Galleries = new SelectList(await _galleryService.GetGalleriesAsync(false, false),
                    nameof(VideoGalleryDTO.Id), nameof(VideoGalleryDTO.Title));
                if (galleryId != Guid.Empty) {
                    model.FromGalleryId = galleryId;
                }

                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get video for edit. {message}", ex.Message);
                SetNotification(OperationResult.Failure($"Возникла ошибка при получении видео для изменения"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VideoViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var video = _mapper.Map<VideoDTO>(model);
                    video.Preview = await model.Preview.ConvertToFileDataProviderAsync(_settings.FilesDirectoryPath,
                        _settings.VideoPreviewsDirectory);
                    video.VideoPath = UrlHelper.YoutubeUrlToEmbed(video.VideoPath);
                    var result = await _galleryService.UpdateVideoAsync(video);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index), new {galleryId = model.FromGalleryId});
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to edit video. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, $"Возникла ошибка при изменении видео");
            }

            return View(model);
        }

        [HttpGet]
        public async Task SwitchVisibility(Guid id) {
            try {
                if (id == Guid.Empty) return;

                var result = await _galleryService.SwitchVideoVisabilityAsync(id);
                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get video for switching visability. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при получении видео для смены его видимости"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id = default, Guid galleryId = default) {
            try {
                if (id == Guid.Empty) return NotFound();

                var video = await _galleryService.GetVideoAsync(id, false);
                if (video == null) return NotFound();

                var model = _mapper.Map<VideoViewModel>(video);
                if (galleryId != Guid.Empty) {
                    model.FromGalleryId = galleryId;
                }

                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get video for delete. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при получении видео для удаления"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVideo(Guid id, Guid galleryId) {
            try {
                var result = await _galleryService.DeleteVideoAsync(id, _settings.FilesDirectoryPath);

                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete video. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, $"Возникла ошибка при удалении видео");
                return BadRequest(ModelState);
            }

            return RedirectToAction(nameof(Index), new {galleryId});
        }
    }
}
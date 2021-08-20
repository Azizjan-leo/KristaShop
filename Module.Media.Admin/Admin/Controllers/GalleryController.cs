using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Data;
using Module.Media.Admin.Admin.Models;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Serilog;
using SixLabors.ImageSharp;

namespace Module.Media.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class GalleryController : AppControllerBase {
        private readonly IGalleryService _galleryService;
        private readonly IMapper _mapper;
        private readonly GlobalSettings _globalSettings;
        private readonly ILogger _logger;

        public GalleryController(IGalleryService galleryService, IMapper mapper, IOptions<GlobalSettings> options,
            ILogger logger) {
            _galleryService = galleryService;
            _mapper = mapper;
            _globalSettings = options.Value;
            _logger = logger;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> LoadData() => Ok(await _galleryService.GetGalleriesAsync());

        [HttpGet]
        public async Task<IActionResult> SwitchVisibility(Guid id) {
            try {
                if (id == Guid.Empty) return NotFound();

                var result = await _galleryService.SwitchGalleryVisabilityAsync(id);
                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get gallery item for switching visability. {message}", ex.Message);
                SetNotification(
                    OperationResult.Failure("Возникла ошибка при получении элемента галереи для смены её видимости"));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task UpdateRow(Guid id, int fromPosition, int toPosition) {
            var dto = await _galleryService.GetGalleryItemNoTrackAsync(id);
            dto.Order = toPosition;
            await _galleryService.UpdateGalleryAsync(dto);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(GalleryItemViewModel model) {
            if (ModelState.IsValid && model.Image != null) {
                var image = await Image.LoadAsync(model.Image.OpenReadStream());
                if (image.Width == image.Height) {
                    var dto = _mapper.Map<GalleryItemDTO>(model);
                    dto.ImagePath = await FileUpload.FilePathAsync(dto.Image, _globalSettings);

                    var result = await _galleryService.InsertGalleryAsync(dto);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }

                SetNotification(OperationResult.Failure(new List<string>
                    {"Ширина и высота фото должны быть одинаковыми."}));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id) {
            if (id == null) return NotFound();

            var dto = await _galleryService.GetGalleryItemAsync(id.Value);
            var model = _mapper.Map<GalleryItemViewModel>(dto);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GalleryItemViewModel model) {
            if (ModelState.IsValid) {
                var dto = _mapper.Map<GalleryItemDTO>(model);
                if (model.Image != null) {
                    var image = await Image.LoadAsync(model.Image.OpenReadStream());
                    if (image.Width == image.Height) {
                        dto.ImagePath = await FileUpload.FilePathAsync(dto.Image, _globalSettings);
                    } else {
                        SetNotification(OperationResult.Failure(new List<string>
                            {"Ширина и высота фото должны быть одинаковыми."}));
                        return View(model);
                    }
                }

                var result = await _galleryService.UpdateGalleryAsync(dto);
                SetNotification(result);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id) {
            if (id == null) return NotFound();

            var dto = await _galleryService.GetGalleryItemAsync(id.Value);
            var model = _mapper.Map<GalleryItemViewModel>(dto);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id) {
            var result = await _galleryService.DeleteGalleryAsync(id);
            SetNotification(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
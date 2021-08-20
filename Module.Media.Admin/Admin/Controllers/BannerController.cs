using System;
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
using Module.Common.WebUI.Data;
using Module.Common.WebUI.Extensions;
using Module.Media.Admin.Admin.Models;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Serilog;
using SixLabors.ImageSharp;

namespace Module.Media.Admin.Admin.Controllers {
    [Area("Admin")]
    [PermissionFilter]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class BannerController : AppControllerBase {
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly GlobalSettings _globalSettings;
        private readonly TextColors _textColors;

        public BannerController(IBannerService bannerService, IMapper mapper, ILogger logger,
            IOptions<GlobalSettings> options, IOptions<TextColors> textColorsOptions) {
            _bannerService = bannerService;
            _mapper = mapper;
            _logger = logger;
            _globalSettings = options.Value;
            _textColors = textColorsOptions.Value;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> LoadData() {
            try {
                return Ok(await _bannerService.GetBannersAsync());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get banners list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка баннеров"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRow(Guid id, int toPosition) {
            try {
                var dto = await _bannerService.GetBannerNoTrackAsync(id);
                dto.Order = toPosition;
                var result = await _bannerService.UpdateBannerAsync(dto);
                return Ok(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update banner order. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при обновлении позиции баннера"));
                return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(Index))));
            }
        }

        [HttpGet]
        public IActionResult Create() {
            try {
                var model = new BannerItemViewModel();
                model.Colors = _textColors.TitleColors
                    .Select(x =>
                        new SelectListItem(x.Key, x.Value, x.Value.Equals(Color.Black.ToHex())))
                    .ToList();
                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get create banner page. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при получении формы создания баннера"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(BannerItemViewModel model) {
            try {
                if (ModelState.IsValid && model.Image != null) {
                    var image = await Image.LoadAsync(model.Image.OpenReadStream());
                    if (image.Width >= 1920 && image.Height >= 850) {
                        var banner = _mapper.Map<BannerItemDTO>(model);
                        banner.TitleColor = Color.Parse(model.Color).ToHex();
                        banner.Image =
                            await model.Image.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath,
                                _globalSettings.GalleryDirectory);

                        var result = await _bannerService.InsertBannerAsync(banner);
                        SetNotification(result);
                        return RedirectToAction(nameof(Index));
                    }

                    SetNotification(OperationResult.Failure("Размеры фото должны быть 1920х850"));
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create banner. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при создании баннера");
            }

            model.Colors = _textColors.TitleColors
                .Select(x => new SelectListItem(x.Key, x.Value, x.Value.Equals(model.Color))).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) {
            try {
                if (id == Guid.Empty) {
                    return NotFound();
                }

                var banner = await _bannerService.GetBannerAsync(id);
                if (banner == null) {
                    return NotFound();
                }

                var result = _mapper.Map<BannerItemViewModel>(banner);
                result.Colors = _textColors.TitleColors
                    .Select(x => new SelectListItem(x.Key, x.Value, x.Value.Equals(banner.TitleColor))).ToList();
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get banner for edit. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при получении баннера для изменения"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> SwitchVisibility(Guid id) {
            try {
                if (id == Guid.Empty) return NotFound();

                var result = await _bannerService.SwitchBannerVisabilityAsync(id);
                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get banner for switching visability. {message}", ex.Message);
                SetNotification(
                    OperationResult.Failure("Возникла ошибка при получении баннера для смены его видимости"));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BannerItemViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var banner = _mapper.Map<BannerItemDTO>(model);
                    if (model.Image != null) {
                        var image = await Image.LoadAsync(model.Image.OpenReadStream());
                        if (image.Width >= 1920 && image.Height >= 850) {
                            banner.ImagePath = await FileUpload.FilePathAsync(model.Image, _globalSettings);
                        } else {
                            SetNotification(OperationResult.Failure("Размеры фото должны быть 1920х850"));
                            return View(model);
                        }
                    }

                    banner.TitleColor = Color.Parse(model.Color).ToHex();
                    banner.Image = await model.Image.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath,
                        _globalSettings.GalleryDirectory);

                    var result = await _bannerService.UpdateBannerAsync(banner);
                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to edit banner. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при изменении баннера");
            }

            model.Colors = _textColors.TitleColors
                .Select(x => new SelectListItem(x.Key, x.Value, x.Value.Equals(model.Color))).ToList();
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id) {
            try {
                if (id == Guid.Empty) {
                    return NotFound();
                }

                var banner = await _bannerService.GetBannerAsync(id);
                if (banner == null) {
                    return NotFound();
                }

                var result = _mapper.Map<BannerItemViewModel>(banner);
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get banner for delete. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Возникла ошибка при получении баннера для удаления"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(Guid id) {
            try {
                var result = await _bannerService.DeleteBanner(id, _globalSettings.FilesDirectoryPath);
                SetNotification(result);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete banner. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при удалении баннера");
                return BadRequest(ModelState);
            }
        }
    }
}
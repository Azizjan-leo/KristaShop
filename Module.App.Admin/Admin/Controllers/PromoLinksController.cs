using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Module.App.Admin.Admin.Models;
using Module.App.Business.Interfaces;
using Module.App.Business.Models;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;
using Serilog;

namespace Module.App.Admin.Admin.Controllers {
    [Area("Admin")]
    [PermissionFilter]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class PromoLinksController : AppControllerBase{
        private readonly IPromoLinkService _promoLinksService;
        private readonly GlobalSettings _settings;
        private readonly UrlSetting _urlSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PromoLinksController(IPromoLinkService promoLinksService, IOptions<GlobalSettings> settings,
            IOptions<UrlSetting> urlSettings, IMapper mapper, ILogger logger) {
            _promoLinksService = promoLinksService;
            _settings = settings.Value;
            _urlSettings = urlSettings.Value;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index() {
            ViewData["ShopPromoUrl"] = _urlSettings.KristaShopPromoUrl;
            return View();
        }

        public async Task<IActionResult> LoadPromoLinks() {
            try {
                return Ok(_mapper.Map<List<PromoLinkViewModel>>(await _promoLinksService.GetPromoLinksAsync(UserSession)));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load promo links list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка промо ссылок"));
            }
        }

        public async Task<IActionResult> Create() {
            var model = new PromoLinkViewModel();
            model.DeactivateTime = DateTimeOffset.Now.AddMinutes(20).ToOffset(DateTimeOffset.Now.Offset);
            model.OrderForms = new SelectList(OrderFormTypeExtension.GetOrderFormLookup(), "Key", "Value");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromoLinkViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var promoLink = _mapper.Map<PromoLinkDTO>(model);
                    promoLink.VideoPreview = await model.VideoPreview.ConvertToFileDataProviderAsync(_settings.FilesDirectoryPath, _settings.PromoFilesDirectory);
                    promoLink.VideoPath = UrlHelper.YoutubeUrlToEmbed(promoLink.VideoPath);
                    promoLink.ManagerId = UserSession.ManagerId;
                    var result = await _promoLinksService.InsertPromoLinkAsync(promoLink);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create promo link. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при создании промо ссылки");
            }

            model.OrderForms = new SelectList(OrderFormTypeExtension.GetOrderFormLookup(), "Key", "Value");
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id) {
            try {
                if (id == Guid.Empty)
                    return NotFound();

                var promoLink = await _promoLinksService.GetPromoLinkAsync(id);
                if (promoLink == null)
                    return NotFound();

                var model = _mapper.Map<PromoLinkViewModel>(promoLink);
                model.DeactivateTime = model.DeactivateTime.ToOffset(DateTimeOffset.Now.Offset);
                model.OrderForms = new SelectList(OrderFormTypeExtension.GetOrderFormLookup(), "Key", "Value");

                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get video for edit. {message}", ex.Message);
                SetNotification(OperationResult.Failure($"Возникла ошибка при получении видео для изменения"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PromoLinkViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var promoLink = _mapper.Map<PromoLinkDTO>(model);
                    promoLink.VideoPreview = await model.VideoPreview.ConvertToFileDataProviderAsync(_settings.FilesDirectoryPath, _settings.PromoFilesDirectory);
                    promoLink.VideoPath = UrlHelper.YoutubeUrlToEmbed(promoLink.VideoPath);
                    promoLink.ManagerId = UserSession.ManagerId;
                    var result = await _promoLinksService.UpdatePromoLinkAsync(promoLink);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update promo link. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Возникла ошибка при изменении промо ссылки");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id) {
            try {
                if (id.IsEmpty()) {
                    SetNotification(OperationResult.Failure("Промо ссылка не найдена"));
                    return RedirectToAction(nameof(Index));
                }

                var model = _mapper.Map<PromoLinkViewModel>(await _promoLinksService.GetPromoLinkAsync(id));
                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to find promo link for delete. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Промо ссылка не найдена"));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteConfirmed(Guid id) {
            try {
                if (id.IsEmpty()) {
                    SetNotification(OperationResult.Failure("Промо ссылка не найдена"));
                    return RedirectToAction(nameof(Index));
                }

                var result = await _promoLinksService.DeletePromoLinkAsync(id);
                SetNotification(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete promo link. {message}", ex.Message);
                SetNotification(OperationResult.Failure());
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

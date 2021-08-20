using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.FeatureManagement;
using Module.Catalogs.Admin.Admin.Models;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;
using Serilog;
using static KristaShop.Common.Models.GlobalConstant;

namespace Module.Catalogs.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class CatalogController : AppControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IMapper _mapper;
        private readonly GlobalSettings _globalSettings;
        private readonly IFeatureManager _featureManager;
        private readonly ILogger _logger;

        public CatalogController(ICatalogService catalogService,
            IMapper mapper, IOptions<GlobalSettings> settings, IFeatureManager featureManager, ILogger logger) {
            _catalogService = catalogService;
            _mapper = mapper;
            _globalSettings = settings.Value;
            _featureManager = featureManager;
            _logger = logger;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> LoadData() {
            var allCatalogs = _catalogService.GetAllCatalogs();

            var list = await _catalogService.GetCatalogsAsync();
            if(!await _featureManager.IsEnabledAsync(FeatureFlags.FeatureAdvancedFunctionality))
                list = list.Where(x => x.Id != CatalogType.RfInStockLines && x.Id != CatalogType.RfInStockParts).ToList();
            foreach (var item in list) 
                item.AssignCatalog1CName(allCatalogs);

            return Ok(list);
        }

        [HttpPost]
        public async Task UpdateRow(CatalogType id, int fromPosition, int toPosition) {
            await _catalogService.UpdateCatalogPosition(id, toPosition);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrSetCatalogExtraCharge(CatalogType catalogId, ExtraChargeType extraChargeType, double sum) {
            if(catalogId < CatalogType.InStockLines || sum < 0 ) {
                SetNotification(OperationResult.Failure("Введены невалидные данные"));
                return BadRequest();
            }
            try {
                await _catalogService.AddOrSetExtraChargeToCatalog(catalogId, extraChargeType, sum);
                var catalogExtraCharges = await _catalogService.GetCatalogExtraCharges(catalogId);
                return PartialView("_ExtraChargesPartial", _mapper.Map<List<CatalogExtraChargeViewModel>>(catalogExtraCharges));
            } catch (Exception ex) {
                _logger.Error($"Failed to add or set {extraChargeType} extra charge to {catalogId} catalog. {ex.Message}");
                SetNotification(OperationResult.Failure("Ошибка при добавлении/изменини наценки каталога"));
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCatalogExtraCharge(CatalogType catalogId, Guid extraChargeId) {
            try {
                await _catalogService.DeleteCatalogExtraCharge(extraChargeId);
                var catalogExtraCharges = await _catalogService.GetCatalogExtraCharges(catalogId);
                return PartialView("_ExtraChargesPartial", _mapper.Map<List<CatalogExtraChargeViewModel>>(catalogExtraCharges));
            } catch (Exception ex) {
                _logger.Error($"Failed to remove extra charge with id: {extraChargeId} of {catalogId} catalog. {ex.Message}");
                SetNotification(OperationResult.Failure("Ошибка при удалении наценки каталога"));
            }
            return BadRequest();
        }

        public IActionResult Create() {
            ViewBag.AllCatalogsFrom1C = new SelectList(_catalogService.GetAllCatalogs(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CatalogViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.FixParametersByCatalogType();

                var dto = _mapper.Map<CatalogDTO>(model);
                dto.Preview = await model.Preview.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath, _globalSettings.CatalogPreviewsDirectory);
                dto.VideoPath = UrlHelper.YoutubeUrlToEmbed(dto.VideoPath);
                var result = await _catalogService.InsertCatalog(dto);
                SetNotification(result);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllCatalogsFrom1C = new SelectList(_catalogService.GetAllCatalogs(), "Id", "Name");

            return View(model);
        }

        public async Task<IActionResult> Edit(CatalogType? id)
        {
            if (id == null)
                return NotFound();

            var dto = await _catalogService.GetCatalogDetailsWithExtraChargesAsync(id.Value);
            if (dto == null)
                return NotFound();

            var model = _mapper.Map<CatalogViewModel>(dto);
            if (model.CloseTime != null) {
                model.CloseTime = model.CloseTime.Value.ToOffset(DateTimeOffset.Now.Offset);
            }

            ViewBag.AllCatalogsFrom1C = new SelectList(_catalogService.GetAllCatalogs(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CatalogViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.FixParametersByCatalogType();

                var dto = _mapper.Map<CatalogDTO>(model);
                dto.Preview = await model.Preview.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath, _globalSettings.CatalogPreviewsDirectory);
                dto.VideoPath = UrlHelper.YoutubeUrlToEmbed(dto.VideoPath);
                var result = await _catalogService.UpdateCatalog(dto);
                SetNotification(result);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllCatalogsFrom1C = new SelectList(_catalogService.GetAllCatalogs(), "Id", "Name");

            return View(model);
        }

        public async Task<IActionResult> Delete(CatalogType? id)
        {
            if (id == null)
                return NotFound();

            var dto = await _catalogService.GetCatalogDetailsAsync(id.Value);
            var model = _mapper.Map<CatalogViewModel>(dto);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CatalogType id)
        {
            var result = await _catalogService.DeleteCatalog(id, _globalSettings.FilesDirectoryPath);
            SetNotification(result);
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> CatalogsVisibility(int userId, [FromServices] ICatalogAccessService catalogAccessService) {
            try {
                var catalogs = await catalogAccessService.GetAllUserCatalogs(userId);
                
                return Ok(new {
                    UserId = userId,
                    UserName = "",
                    CatalogsList = catalogs.Select(x => new {
                        Id = (int) x.Key,
                        Key = x.Key.ToString(),
                        Name = x.Key.GetDisplayName(),
                        Visible = x.Value
                    }).ToList()
                });
            } catch (Exception ex) {
                _logger.Error(ex, "Login failed for user {userId}", userId);
                SetNotification(OperationResult.Failure("Во время загрузки видимостей произошла ошибка"));
                return RedirectToAction("Index");
            }
            
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeVisibility(int userId, CatalogType catalog, bool visibility,
            [FromServices] ICatalogAccessService catalogAccessService) {            
            try {
                if (!catalog.IsValidCatalog()) {
                    return BadRequest();
                }
                
                await catalogAccessService.ChangeCatalogAccessAsync(userId, catalog, visibility);
                return Ok();
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to change catalog {catalog} acccess", catalog.AsString());
                return BadRequest();
            }
        }
    }
}
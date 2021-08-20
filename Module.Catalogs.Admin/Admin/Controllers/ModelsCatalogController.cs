using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Module.Catalogs.Admin.Admin.Models;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Data;
using Serilog;

namespace Module.Catalogs.Admin.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class ModelsCatalogController : AppControllerBase
    {
        private readonly ICatalogItemsService _catalogItemsService;
        private readonly ICatalogService _catalogService;
        private readonly ICatalogItemUpdateService _updateService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly GlobalSettings _globalSettings;

        public ModelsCatalogController(ICatalogItemsService catalogItemsService, ICatalogService catalogService,
            ICatalogItemUpdateService updateService, ICategoryService categoryService,
            IMapper mapper, IOptions<GlobalSettings> options, ILogger logger) {
            _catalogItemsService = catalogItemsService;
            _catalogService = catalogService;
            _updateService = updateService;
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
            _globalSettings = options.Value;
        }

        private IEnumerable<CatalogItemGroupNew> _catalogData = new List<CatalogItemGroupNew>();
        private async Task<IEnumerable<CatalogItemGroupNew>> _readCatalogDataAsync(CatalogType catalogId = CatalogType.All) {
            return await _catalogItemsService.GetFullModelsAllListAsync(
                    new ProductsFilterExtended {
                        CatalogId = catalogId, OrderDirection = CatalogOrderDir.Asc,
                        OrderType = CatalogOrderType.OrderByPosition, HideEmptySlots = false,
                        IncludeCategoriesMap = true, IncludeCatalogsMap = true, ShowHiddenModels = true,
                        ExcludeHiddenByColors = false
                    }, UserSession, new Page(0, 1000));
        }
        
        public async Task<IActionResult> Index(string articul) {
            try {
                var result = await _readCatalogDataAsync();

                ViewBag.Colors = new SelectList(_catalogItemsService.GetAllColors(result), "Name", "Name");
                ViewBag.SizeLines = new SelectList(_catalogItemsService.GetAllSizes(result));
                ViewBag.Catalogs = new SelectList(_catalogService.GetAllCatalogs(), "Name", "Name");
                ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Name", "Name");
                ViewBag.Visibility = new SelectList(new [] { new { Text = "Видимые", Value = "1" }, new { Text = "Не видимые", Value = "0" } }, "Value", "Text");
                ViewBag.Articul = articul;

                return View(_mapper.Map<IEnumerable<AdminCatalogItemBriefViewModel>>(result));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load all models. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexByCatalog(CatalogType? id, string articul) {
            if (id == null)
                return NotFound();

            var result = await _readCatalogDataAsync(id.Value);

            ViewBag.Colors = new SelectList(_catalogItemsService.GetAllColors(result), "Name", "Name");
            ViewBag.SizeLines = new SelectList(_catalogItemsService.GetAllSizes(result));
            ViewBag.Catalogs = new SelectList(_catalogService.GetAllCatalogs(), "Name", "Name");
            ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Name", "Name");
            ViewBag.Visibility = new SelectList(new[] {new {Text = "Видимые", Value = "1"}, new {Text = "Не видимые", Value = "0"}}, "Value", "Text");
            ViewBag.Articul = articul;

            return View((id.Value, _mapper.Map<IEnumerable<AdminCatalogItemBriefViewModel>>(result)));
        }

        public async Task<IActionResult> IndexDetailed(string articul) {
            var result = await _readCatalogDataAsync();

            ViewBag.Colors = new SelectList(_catalogItemsService.GetAllColors(result), "Name", "Name");
            ViewBag.SizeLines = new SelectList(_catalogItemsService.GetAllSizes(result));
            ViewBag.Catalogs = new SelectList(_catalogService.GetAllCatalogs(), "Name", "Name");
            ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Name", "Name");
            ViewBag.Visibility = new SelectList(new[] { new { Text = "Видимые", Value = "1" }, new { Text = "Не видимые", Value = "0" } }, "Value", "Text");
            ViewBag.Articul = articul;

            return View(_mapper.Map<IEnumerable<CatalogItem1CBriefViewModel>>(result));
        }

        public async Task<IActionResult> History(string articul) {
            try {
                var result = (await _catalogItemsService.GetFullModelsHistoryAsync()).ToList();
                ViewBag.Colors = new SelectList(_catalogItemsService.GetAllColors(result), "Name", "Name");
                ViewBag.SizeLines = new SelectList(_catalogItemsService.GetAllSizes(result));
                ViewBag.Catalogs = new SelectList(_catalogService.GetAllCatalogs(), "Name", "Name");
                ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Name", "Name");
                ViewBag.Visibility = new SelectList(new [] { new { Text = "Видимые", Value = "1" }, new { Text = "Не видимые", Value = "0" } }, "Value", "Text");
                ViewBag.Articul = articul;
                
                return View(_mapper.Map<IEnumerable<AdminCatalogItemBriefViewModel>>(result));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load history models list. {message}", ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexPopUp(string defCatalog) {
            if (string.IsNullOrEmpty(defCatalog)) {
                defCatalog = "none";
            } else {
                defCatalog = defCatalog.Trim().ToLower();
                if (defCatalog != "preorder" && defCatalog != "retail") {
                    defCatalog = "none";
                }
            }
            await _readCatalogDataAsync();

            ViewBag.Colors = new SelectList(_catalogItemsService.GetAllColors(_catalogData), "Name", "Name");
            ViewBag.SizeLines = new SelectList(_catalogItemsService.GetAllSizes(_catalogData));
            if (defCatalog == "none") {
                ViewBag.Catalogs = new SelectList(_catalogService.GetAllCatalogs().Where(x => x.Id != (int)CatalogType.Open), "Name", "Name");
            } else {
                ViewBag.Catalogs = new SelectList(_catalogService.GetAllCatalogs().Where(x => x.Id != (int)CatalogType.Open), "Name", "Name", (defCatalog == "preorder" ? CatalogType.Preorder.AsString() : CatalogType.InStockLines.AsString()));
            }

            return View();
        }

        public async Task<IActionResult> LoadDataPopUp() {
            try {
                var modelsList = await _catalogItemsService.GetProductsListForAddAsync();

                return Ok(modelsList);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get models list in catalog. {message}", ex.Message);

                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка моделей"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> MoveRow([FromBody] List<EditModelCatalogOrderViewModel> positions) {
            try {
                bool isMovingHappened = false;
                foreach (var item in positions) {
                    await _updateService.UpdateModelPositionInCatalogAsync(item.CatId, item.Articul, item.ToPosition);
                    isMovingHappened = true;
                }
                return isMovingHappened ? Ok(OperationResult.Success("Позиция модели в каталоге успешно изменена")) : Ok(OperationResult.Failure("Позиция в каталоге не изменена"));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update model order in catalog. {message}", ex.Message);

                SetNotification(OperationResult.Failure("Возникла ошибка при обновлении позиции модели в каталоге"));
                return Problem(OperationResult.FailureAjaxRedirect(Url.Action(nameof(IndexByCatalog), new { id = 1 })));
            }
        }

        public async Task<IActionResult> Edit(string id, int? catalogId, string articul) {
            try {
                if (id == null)
                    return NotFound();
                id = Uri.UnescapeDataString(id);

                catalogId ??= -1;

                var modelDto = await _catalogItemsService.FindModelForAdminAsync(id, catalogId.Value, UserSession);
                
                if (modelDto == null)
                    return NotFound();
                
                var model = _mapper.Map<Model1CViewModel>(modelDto);
                model.FullModelData = modelDto;
                model.CatalogsInvisibility = await _catalogItemsService.GetModelCatalogsInvisibilityAsync(id);
                model.CurrentCatalogId = catalogId.Value;

                ViewBag.Colors = new SelectList(modelDto.Colors, "Id", "Name"); // TO DO All colors ???
                ViewBag.CurrentCatalogName = (catalogId.Value.IsCatalogValueCorrect() ? catalogId.Value.ToProductCatalog1CId().AsString() : "Все каталоги");

                ViewBag.CatalogsInvisibility = new SelectList(_catalogService.GetAllCatalogs(), "Id", "Name");

                ViewBag.Articul = articul;
                
                return View(model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get model for edit {art}. {message}", articul, ex.Message);
                SetNotification(OperationResult.Failure("Не удалось получить модель"));
                return RedirectToAction("Index");
            }
        }
        
        public async Task<IActionResult> EditHistory(string id, int? catalogId, string articul) {
            try {
                if (id == null)
                    return NotFound();

                catalogId ??= -1;

                var modelDto = await _catalogItemsService.FindHistoryModelAsync(id);
                if (modelDto == null)
                    return NotFound();
                
                var model = _mapper.Map<Model1CViewModel>(modelDto);
                model.FullModelData = modelDto;
                model.CatalogsInvisibility = await _catalogItemsService.GetModelCatalogsInvisibilityAsync(id);
                model.CurrentCatalogId = catalogId.Value;

                ViewBag.Colors = new SelectList(modelDto.Colors, "Id", "Name"); // TO DO All colors ???
                ViewBag.CurrentCatalogName = (catalogId.Value.IsCatalogValueCorrect() ? catalogId.Value.ToProductCatalog1CId().AsString() : "Все каталоги");

                ViewBag.CatalogsInvisibility = new SelectList(_catalogService.GetAllCatalogs(), "Id", "Name");

                ViewBag.Articul = articul;

                return View("Edit", model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get model for edit {art}. {message}", articul, ex.Message);
                SetNotification(OperationResult.Failure("Не удалось получить модель"));
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Model1CViewModel model) {
            try {
                if (ModelState.IsValid) {
                    if (model.Image != null) {
                        model.ImagePath = await FileUpload.FilePathAsync(model.Image, _globalSettings);
                    }

                    var modelDto = _mapper.Map<UpdateCatalogItemDescriptorDTO>(model);
                    modelDto.VideoLink = UrlHelper.YoutubeUrlToEmbed(modelDto.VideoLink);
                    var uploadedPhotos = new List<string>();
                    if (model.Photos != null) {
                        foreach (var photo in model.Photos) {
                            var photoPath = await FileUpload.FilePathAsync(photo, _globalSettings);
                            uploadedPhotos.Add(photoPath);
                        }
                    }

                    modelDto.UploadedPhotos = uploadedPhotos;
                    await _updateService.UpdateModelDescriptionAsync(modelDto);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to save model description. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Ошибка при сохранеии описания модели в БД"));
            }
            
            var action = Request.GetTypedHeaders().Referer.LocalPath.Contains(nameof(EditHistory))
                ? nameof(EditHistory)
                : nameof(Edit);
            return RedirectToAction(action, new { id = model.Articul, catalogId = model.CurrentCatalogId });
        }

        public async Task<IActionResult> LoadPhotos(string id) {
            try {
                var list = await _catalogItemsService.GetModelPhotosAsync(id);

                return Ok(list);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get models list in catalog. {message}", ex.Message);

                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка фотографий модели"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhotoRow(int id, int toPosition) {
            try {
                await _updateService.UpdateModelPhotoPositionAsync(id, toPosition);

                return Ok(new object());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update model photo position. {message}", ex.Message);

                return Problem(OperationResult.FailureAjax("Возникла ошибка при обновлении позиции фото"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMainPhoto(string id, string path) {
            try {
                await _updateService.UpdateModelMainPhotoAsync(id, path);

                return Ok(OperationResult.Success());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update model main photo. {message}", ex.Message);

                return Problem(OperationResult.FailureAjax("Возникла ошибка при обновлении основного фото"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int photoId) {
            try {
                var filesToDelete = await _updateService.DeleteModelPhotoAsync(photoId);

                foreach (var filePath in filesToDelete) {
                    await FileUpload.FileRemove(filePath, _globalSettings);
                }

                return Ok(OperationResult.Success());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete model photo. {message}", ex.Message);

                return Problem(OperationResult.FailureAjax("Возникла ошибка удалении фото модели"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReorderPhotoModel(int id, string articul, int toPosition) {
            try {
                await _updateService.ReorderModelPhotosAsync(articul, id, toPosition);

                return Ok(OperationResult.Success());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to reorder model photos. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при переупорядочивании фото модели"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditPhoto(int photoId, int colorId) {
            try {
                await _updateService.SetPhotoColorAsync(photoId, colorId);

                return Ok(OperationResult.Success());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to add color to model photos. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при добавлении цвета к фото модели"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResizePhotoModel(int id, IFormFile file) {
            try {
                string path = await FileUpload.FilePathAsync(file, _globalSettings);

                var fileToDelete = await _updateService.UpdateModelPhotoAsync(id, path);
                await FileUpload.FileRemove(fileToDelete, _globalSettings);

                return Ok(OperationResult.Success());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update model photo path. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка обновлении фото модели"));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateModelVisibility(VisibilityCatalogItemEditModel model) {
            try {
                if (model.SizeValue.Equals(model.SizeLine)) {
                    model.SizeValue = "0";
                }
                
                await _catalogItemsService.UpdateCatalogItemVisibility(model.Articul, model.ModelId,
                    new SizeValue(model.SizeValue), model.ColorId, model.CatalogId, model.IsVisible);
                return Json(OperationResult.Success());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to update catalog item visibility. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при обновлении модели"));
            }
        }
    }
}
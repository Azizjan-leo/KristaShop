using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Module.Catalogs.Admin.Admin.Models;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Data;
using SixLabors.ImageSharp;

namespace Module.Catalogs.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class CategoryController : AppControllerBase {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryService _catService;
        private readonly GlobalSettings _globalSettings;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService catService, ICategoryService categoryService, IMapper mapper,
            IOptions<GlobalSettings> options) {
            _catService = catService;
            _categoryService = categoryService;
            _mapper = mapper;
            _globalSettings = options.Value;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> LoadData() {
            var allCategories = await _categoryService.GetAllCategoriesAsync();

            var list = await _catService.GetCategoriesAsync();
            foreach (var item in list) item.AssignCategory1CName(allCategories);

            return Ok(list);
        }

        [HttpPost]
        public async Task UpdateRow(Guid id, int fromPosition, int toPosition) {
            var dto = await _catService.GetCategoryAsync(id);
            dto.Order = toPosition;
            await _catService.UpdateCategoryAsync(dto);
        }

        public async Task<IActionResult> Create() {
            ViewBag.AllCetgoriesFrom1C = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model) {
            if (ModelState.IsValid && model.Image != null) {
                var image = await Image.LoadAsync(model.Image.OpenReadStream());
                if (image.Width == image.Height) {
                    var dto = _mapper.Map<CategoryDTO>(model);
                    dto.ImagePath = await FileUpload.FilePathAsync(dto.Image, _globalSettings);
                    var result = await _catService.InsertCategoryAsync(dto);

                    SetNotification(result);
                    return RedirectToAction(nameof(Index));
                }

                SetNotification(OperationResult.Failure(new List<string> {"Ширина и высота фото должны быть одинаковыми."}));
            } else if (model.Image == null) {
                SetNotification(OperationResult.Failure(new List<string> {"Необходимо прикрепить фото."}));
            }

            ViewBag.AllCetgoriesFrom1C = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name");

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id) {
            if (id == null)
                return NotFound();

            var dto = await _catService.GetCategoryAsync(id.Value);
            var model = _mapper.Map<CategoryViewModel>(dto);
            if (model == null)
                return NotFound();

            ViewBag.AllCetgoriesFrom1C = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model) {
            if (ModelState.IsValid) {
                var dto = _mapper.Map<CategoryDTO>(model);
                if (model.Image != null) {
                    var image = await Image.LoadAsync(model.Image.OpenReadStream());
                    if (image.Width == image.Height) {
                        dto.ImagePath = await FileUpload.FilePathAsync(dto.Image, _globalSettings);
                    } else {
                        SetNotification(OperationResult.Failure(new List<string>
                            {"Ширина и высота фото должны быть одинаковыми."}));

                        ViewBag.AllCetgoriesFrom1C =
                            new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name");

                        return View(model);
                    }
                }

                var result = await _catService.UpdateCategoryAsync(dto);
                SetNotification(result);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllCetgoriesFrom1C = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name");

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id) {
            if (id == null)
                return NotFound();

            var dto = await _catService.GetCategoryAsync(id.Value);
            var model = _mapper.Map<CategoryViewModel>(dto);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id) {
            var result = await _catService.DeleteCategory(id);
            SetNotification(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
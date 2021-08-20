using System;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;

namespace Module.Media.Admin.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    [PermissionFilter]
    public class FaqController : AppControllerBase
    {
        private IFaqService _faqService;
        private GlobalSettings _globalSettings;
        public FaqController(IFaqService faqService, IOptions<GlobalSettings> options)
        {
            _faqService = faqService;
            _globalSettings = options.Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FaqDTO model)
        {
            if (ModelState.IsValid)
            {
                await _faqService.CreateFaqAsync(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var faq = _faqService.GetFaqById(id);
            return View(faq);
        }

        public async Task<IActionResult> DeleteFaq(Guid id) 
        {
            await _faqService.DeleteFaqAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteFaqSection(Guid id) 
        {
            var faqId = await _faqService.DeleteFaqSectionAsync(id);
            return RedirectToAction("SectionList", new RouteValueDictionary(new { faqId = faqId }));
        }

        public async Task<IActionResult> DeleteFaqSectionContent(Guid id) 
        {
            var faqSectionId = await _faqService.DeleteFaqContentAsync(id);
            return RedirectToAction("SectionContentList", new RouteValueDictionary(new { sectionId = faqSectionId }));
        }

        public async Task<IActionResult> UpdateFaq(FaqDTO model) 
        {
            await _faqService.UpdateFaqAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateFaqSection(FaqSectionDto model) 
        {
            var faqId = await _faqService.UpdateFaqSectionAsync(model, _globalSettings.FaqDocumentsDirectory, _globalSettings.FilesDirectoryPath);
            return RedirectToAction("SectionList", new RouteValueDictionary(new { faqId = faqId }));
        }

        public async Task<IActionResult> UpdateFaqSectionContent(FaqSectionContentDto model) 
        {
            var faqSectionId = await _faqService.UpdateFaqSectionContentAsync(model, _globalSettings.FaqDocumentsDirectory, _globalSettings.FilesDirectoryPath);
            return RedirectToAction("SectionContentList", new RouteValueDictionary(new { sectionId = faqSectionId }));
        }

        public async Task<IActionResult> SectionList(Guid faqId) 
        {
            ViewBag.FaqId = faqId;
            var faqSections = await _faqService.GetFaqSectionsAsync(faqId);
            return View(faqSections);
        }

        public async Task<IActionResult> SectionContentList(Guid sectionId) 
        {
            ViewBag.SectionId = sectionId;
            var sectionContent = await _faqService.GetFaqSectionContentAsync(sectionId);
            return View(sectionContent);
        }

        [HttpGet]
        public async Task<IActionResult> EditSectionContent(Guid id) 
        {
            var sectionContent = await _faqService.GetFaqSectionContentByIdAsync(id);
            return View(sectionContent);
        }
        [HttpGet]
        public IActionResult EditSection(Guid sectionId) 
        {
            var sectionContent = _faqService.GetFaqSectionById(sectionId);
            ViewBag.FaqRootUrl = _globalSettings.FaqDocumentsDirectory;
            return View(sectionContent);
        }

        [HttpGet]
        public IActionResult CreateSection(Guid faqId) 
        {
            ViewBag.FaqId = faqId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection(FaqSectionDto model) 
        {
            if (ModelState.IsValid)
            {
                var faqId = await _faqService.CreateFaqSectionAsync(model, _globalSettings.FaqDocumentsDirectory, _globalSettings.FilesDirectoryPath);
                return RedirectToAction("SectionList", new RouteValueDictionary(new { faqId = faqId }));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSectionContent(Guid sectionId, int faqSectionId) 
        {
            ViewBag.SectionId = sectionId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSectionContent(FaqSectionContentDto model) 
        {
            if (ModelState.IsValid) 
            {
                var faqSectionId = await _faqService.CreateFaqSectionContentAsync(model, _globalSettings.FaqDocumentsDirectory, _globalSettings.FilesDirectoryPath);
                return RedirectToAction("SectionContentList", new RouteValueDictionary(new { sectionId = faqSectionId }));
            }

            return View(model);
        }

        public IActionResult LoadData() => Ok(_faqService.GetAllFaqs());
    }
}
using System;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Module.Common.WebUI.Infrastructure;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.Controllers
{
    [Permission(ForManagerOnly = true)]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class FaqController : Controller
    {
        private readonly IFaqService _faqService;
        private GlobalSettings _globalSettings;
        private readonly ILogger _logger;
        public FaqController(IFaqService faqService, IOptions<GlobalSettings> options, ILogger logger) 
        {
            _faqService = faqService;
            _globalSettings = options.Value;
            _logger = logger;
        }
        public async Task<IActionResult> Index(Guid faqId)
        {
            try
            {
                var faqs = _faqService.GetAllFaqs();
                var selectedSections = await _faqService.GetFaqSectionsAsync(faqId);
                var model = new FaqViewModel() { Faqs = faqs, SelectedFaqSections = selectedSections };
                model.SelectedSectionColorCode = await _faqService.GetFaqColorCode(faqId);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch faq info", ex.Message);
                return Json(new { success = false });
            }
        }

        public async Task<IActionResult> GetSections(Guid faqId)
        {
            try
            {
                var faqSections = await _faqService.GetFaqSectionsAsync(faqId);
                var faqSectionsColor = await _faqService.GetFaqColorCode(faqId);

                return Json(new { success=true, faqSections, faqSectionsColor });
            }
            catch (Exception ex) 
            {
                _logger.Error(ex, "Failed to fetch faq content", ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        public async Task<IActionResult> GetSectionContent(Guid faqSectionId) 
        {
            try
            {
                var faqSectionContent = await _faqService.GetFaqSectionContentAsync(faqSectionId);

                return Json(new { success = true, faqSectionContent });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch section content", ex.Message);
                return Json(new { success = false, error = ex.Message });
            }

        }
    }
}

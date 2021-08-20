using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class Gallery : ViewComponentBase {
        private readonly IGalleryService _galleryService;
        private readonly IDynamicPagesService _dynamicPagesService;
        private readonly ILogger _logger;
        private const string GalleryUrl = "/Gallery/Index";

        public Gallery(IGalleryService galleryService, IDynamicPagesService dynamicPagesService, ILogger logger) {
            _galleryService = galleryService;
            _dynamicPagesService = dynamicPagesService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isRandom = false) {
            try {
                var result = isRandom ? await _galleryService.GetTopNRandomItemsAsync(4) : await _galleryService.GetTopGalleryItemsAsync(4);
                var contents = await _dynamicPagesService.GetPageByUrlAsync(GalleryUrl);
                if (contents != null) {
                    ViewBag.GalleryDesc = contents.Body;
                }
                return View(result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get gallery items for ViewComponent. {message}", ex.Message);
            }

            return Content(string.Empty);
        }
    }
}
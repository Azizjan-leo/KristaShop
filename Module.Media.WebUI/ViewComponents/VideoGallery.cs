using System;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.ViewComponents {
    public class VideoGallery : ViewComponentBase {
        private readonly IVideoGalleryService _galleryService;
        private readonly ILogger _logger;

        public VideoGallery(IVideoGalleryService galleryService, ILogger logger) {
            _galleryService = galleryService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            try {
                var result = await _galleryService.GetFirstGalleryWithVideosAsync(4, !User.Identity.IsAuthenticated || User.IsGuest());
                if (result != null) {
                    return View(result);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get galleries list for video gallery ViewComponent. {message}", ex.Message);
            }

            return Content(string.Empty);
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;
using Serilog;

namespace Module.Media.WebUI.Controllers {
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    [AllowAnonymous]
    public class VideoGalleryController : AppControllerBase {
        private readonly IVideoGalleryService _galleryService;
        private readonly ILogger _logger;

        public VideoGalleryController(IVideoGalleryService galleryService, ILogger logger) {
            _galleryService = galleryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            try {
                var result = await _galleryService.GetGalleriesWithVideoAsync(4, !User.Identity.IsAuthenticated || User.IsGuest());
                if (result.Any()) {
                    return View(result);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get galleries list. {message}", ex.Message);
            }
            return BadRequest();
        }

        public async Task<IActionResult> Details(string gallery, int page = 0) {
            try {
                var galleryFound = await _galleryService.GetGalleryAsync(gallery, !User.Identity.IsAuthenticated || User.IsGuest());
                if (galleryFound != null) {
                    var result = await _galleryService.GetVideosByGalleryOnPageAsync(galleryFound.Id, page);
                    ViewData["Gallery"] = galleryFound;
                    return View(result);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get galleries list. {message}", ex.Message);
            }
            return BadRequest();
        }
    }
}

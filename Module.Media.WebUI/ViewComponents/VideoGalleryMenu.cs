using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Media.Business.Interfaces;

namespace Module.Media.WebUI.ViewComponents {
    public class VideoGalleryMenu : ViewComponentBase {
        private readonly IVideoGalleryService _videoGalleryService;

        public VideoGalleryMenu(IVideoGalleryService videoGalleryService) {
            _videoGalleryService = videoGalleryService;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var result = await _videoGalleryService.GetGalleriesAsync(!User.Identity.IsAuthenticated || User.IsGuest());
            return View(result);
        }
    }
}

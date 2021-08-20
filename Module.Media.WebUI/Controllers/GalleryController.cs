using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Infrastructure;
using Module.Media.Business.Interfaces;

namespace Module.Media.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class GalleryController : Controller {
        private readonly IGalleryService _galleryService;
        private readonly IDynamicPagesService _dynamicPagesService;
        private readonly string _url;

        public GalleryController(IGalleryService galleryService, IDynamicPagesService dynamicPagesService) {
            _galleryService = galleryService;
            _dynamicPagesService = dynamicPagesService;
            _url = "/Gallery/Index";
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1) {
            var content = await _dynamicPagesService.GetPageByUrlAsync(_url);
            ViewBag.Description = content?.Body;
            const int modelInPage = 12;
            var list = await _galleryService.GetGalleryItemsForPageAsync(page, modelInPage);
            return View(list);
        }
    }
}
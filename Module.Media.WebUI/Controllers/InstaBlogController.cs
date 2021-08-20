using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Infrastructure;
using Module.Media.Business.Interfaces;

namespace Module.Media.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class InstaBlogController : AppControllerBase {
        private readonly IBlogService _blogService;
        private readonly IDynamicPagesService _dynamicPagesService;
        private readonly string _url;

        public InstaBlogController(IBlogService blogService, IDynamicPagesService dynamicPagesService) {
            _blogService = blogService;
            _dynamicPagesService = dynamicPagesService;
            _url = "/Instagram/Index";
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1) {
            var content = await _dynamicPagesService.GetPageByUrlAsync(_url);
            ViewBag.Description = content?.Body;
            const int modelInPage = 12;
            var list = await _blogService.GetBlogsForPageAsync(page, modelInPage);
            return View(list);
        }
    }
}
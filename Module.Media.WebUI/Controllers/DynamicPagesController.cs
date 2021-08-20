using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Extensions;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Models;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;

namespace Module.Media.WebUI.Controllers {
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class DynamicPagesController : AppControllerBase {
        private readonly IDynamicPagesManager _manager;

        public DynamicPagesController(IDynamicPagesManager manager) {
            _manager = manager;
        }

        [Route("[controller]/{uri}")]
        public IActionResult Index(string uri) {
            if (string.IsNullOrEmpty(uri)) {
                uri = nameof(Index);
            }

            var (controller, action) = UrlHelper.DeconstructUri(uri);
            ViewData["DynamicController"] = controller;
            ViewData["DynamicAction"] = action;
            ViewData["MetaInfoPath"] = uri;

            if (!_manager.TryGetValuesByController(controller, !User.Identity.IsAuthenticated || User.IsGuest(), out var pages)) {
                return BadRequest();
            }

            if (pages.Any()) {
                var page = pages.First(x => x.URL.Equals(uri));
                ViewData["Title"] = string.IsNullOrEmpty(page.MetaTitle) ? page.Title : page.MetaTitle;
                SetMetaInfo(new MetaViewModel(page.MetaTitle, page.MetaDescription, page.MetaKeywords));
            }

            return View(nameof(Index), pages);
        }

        [Route("[controller]/{uri}")]
        public IActionResult Single(string uri) {
            if (string.IsNullOrEmpty(uri)) {
                uri = nameof(Index);
            }


            var (controller, action) = UrlHelper.DeconstructUri(uri);
            ViewData["DynamicController"] = controller;
            ViewData["DynamicAction"] = action;
            ViewData["MetaInfoPath"] = uri;

            if (!_manager.TryGetValue(uri, out var page) || ((!User.Identity.IsAuthenticated || User.IsGuest()) && !page.IsOpen)) {
                return BadRequest();
            }

            SetMetaInfo(new MetaViewModel(page.MetaTitle, page.MetaDescription, page.MetaKeywords));

            ViewData["Title"] = page.Title;
            return View(nameof(Index), new List<DynamicPageDTO> {page});
        }
    }
}
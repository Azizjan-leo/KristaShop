using System.Threading.Tasks;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Extensions;

namespace Module.Common.Admin.Admin.Controllers {
    [Area("Admin")]
    [Route("ckeditor")]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class CkeditorController : Controller {
        private readonly IFileService _fileService;

        private readonly GlobalSettings _globalSettings;
        public CkeditorController(IOptions<GlobalSettings> globalSettingsOptions, IFileService fileService) {
            _fileService = fileService;
            _globalSettings = globalSettingsOptions.Value;
        }

        [Route("upload_file")]
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile upload) {
            var file = await upload.ConvertToFileDataProviderAsync(_globalSettings.FilesDirectoryPath,
                _globalSettings.EditorFileStorageDirectory);

            var result = await _fileService.SaveFileAsync(file, true);
            var uploaded = string.IsNullOrEmpty(result) ? 0 : 1;
            return Ok(new { uploaded });
        }

        [Route("browse_file")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult BrowseFile() {
            ViewBag.Directory = _globalSettings.EditorFileStorageDirectory;
            ViewBag.FileInfos = _fileService.GetDirectoriesFiles(_globalSettings.FilesDirectoryPath, _globalSettings.EditorFileStorageDirectory);
            return View("BrowseFile");
        }
    }
}
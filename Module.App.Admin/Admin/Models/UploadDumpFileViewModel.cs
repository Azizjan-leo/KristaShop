using KristaShop.Common.Attributes.AllowedFileExtensionAttribute;
using KristaShop.Common.Attributes.FileRequired;
using Microsoft.AspNetCore.Http;

namespace Module.App.Admin.Admin.Models {
    public class UploadDumpFileViewModel {
        [FileRequired(ErrorMessage = "Необходимо указать файл")]
        [AllowedFileExtensions(".sql", ErrorMessage = "Неверное расширение файла")]
        public IFormFile File { get; set; }
    }
}

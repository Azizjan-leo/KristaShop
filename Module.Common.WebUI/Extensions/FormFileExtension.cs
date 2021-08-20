using System.IO;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Module.Common.WebUI.Extensions {
    public static class FormFileExtension {
        public static async Task<FileDataProvider> ConvertToFileDataProviderAsync(this IFormFile file) {
            return await _convertToFileDataProvider(file, string.Empty, string.Empty);
        }

        public static async Task<FileDataProvider> ConvertToFileDataProviderAsync(this IFormFile file, string filesDirectoryPath, string directory) {
            return await _convertToFileDataProvider(file, filesDirectoryPath, directory);
        }

        private static async Task<FileDataProvider> _convertToFileDataProvider(IFormFile file, string filesDirectoryPath, string directory) {
            if (file != null) {
                var resultFile = new FileDataProvider {
                    OriginalName = file.FileName,
                    FileStream = new MemoryStream(),
                    FilesDirectoryPath = filesDirectoryPath,
                    Directory = directory,
                };
                resultFile.OriginalName = resultFile.OriginalName.ToValidFileName();
                await file.CopyToAsync(resultFile.FileStream);
                return resultFile;
            }

            return null;
        }
    }
}

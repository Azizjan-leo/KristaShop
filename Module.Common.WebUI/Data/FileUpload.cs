using System;
using System.IO;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Module.Common.WebUI.Data
{
    public static class FileUpload
    {
        public static async Task<string> FilePathAsync(IFormFile file, GlobalSettings settings)
        {
            const string path = "/galleryphotos/";
            string imagePath = path + Guid.NewGuid() + Path.GetExtension(file.FileName);
            if (!Directory.Exists(settings.FilesDirectoryPath + path))
                Directory.CreateDirectory(settings.FilesDirectoryPath + path);

            await using (var fileStream = new FileStream(settings.FilesDirectoryPath + imagePath, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(fileStream);
            }
            return imagePath;
        }

        public static async Task FileRemove(string filePath, GlobalSettings settings) {
            string fullPath = settings.FilesDirectoryPath.TrimEnd('/') + "/" + filePath.TrimStart('/');
            if (File.Exists(fullPath)) {
                File.Delete(fullPath);
            }
        }
    }
}
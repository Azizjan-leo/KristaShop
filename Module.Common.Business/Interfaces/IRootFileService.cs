using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Module.Common.Business.Interfaces {
    public interface IRootFileService {
        GlobalSettings Settings { get; }
        Task<string> SaveFileAsync(IFormFile file, string directory);
        Task<string> SaveFileAsync(IFileDataProvider file, bool useOriginalName = false);
        bool RemoveFile(string filepath);
        Task<byte[]> GetFileAsync(string relativePath, string filepath);
        List<FileInfo> GetDirectoriesFiles(string directory);
        Dictionary<string, string[]> GetFilesInDirectories(string relativePath, List<string> directories, int userId);
        OperationResult GetLastError();
    }
}
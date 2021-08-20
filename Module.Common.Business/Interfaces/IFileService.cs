using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Module.Common.Business.Interfaces {
    public interface IFileService {
        Task<string> SaveFileAsync(IFormFile file, string filesDirectoryPath, string directory);
        Task<string> SaveFileAsync(IFileDataProvider file, bool useOriginalName = false);
        bool RemoveFile(string filesDirectoryPath, string filepath);
        Task<byte[]> GetFileAsync(string filesDirectoryPath, string filepath);
        List<FileInfo> GetDirectoriesFiles(string filesDirectoryPath, string directory);
        Dictionary<string, string[]> GetFilesInDirectories(string filesDirectoryPath, List<string> directories);
        OperationResult GetLastError();
    }
}

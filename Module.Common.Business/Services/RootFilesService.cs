using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Module.Common.Business.Interfaces;

namespace Module.Common.Business.Services {
    public class RootFilesService : IRootFileService {
        private readonly IFileService _fileService;
        private readonly IMemoryCache _cache;
        public GlobalSettings Settings { get; }

        public RootFilesService(IOptions<GlobalSettings> settings, IFileService fileService, IMemoryCache cache) {
            _fileService = fileService;
            _cache = cache;
            Settings = settings.Value;
        }

        public Task<string> SaveFileAsync(IFormFile file, string relativeDirectoryPath) {
            return _fileService.SaveFileAsync(file, Settings.FilesDirectoryPath, relativeDirectoryPath);
        }

        public Task<string> SaveFileAsync(IFileDataProvider file, bool useOriginalName = false) {
            return _fileService.SaveFileAsync(file, useOriginalName);
        }

        public bool RemoveFile(string filepath) {
            return _fileService.RemoveFile(Settings.FilesDirectoryPath, filepath);
        }

        public Task<byte[]> GetFileAsync(string relativePath, string filepath) {
            return _fileService.GetFileAsync(Path.Combine(Settings.FilesDirectoryPath, relativePath), filepath);
        }

        public List<FileInfo> GetDirectoriesFiles(string directory) {
            return _fileService.GetDirectoriesFiles(Settings.FilesDirectoryPath, directory);
        }

        public Dictionary<string, string[]> GetFilesInDirectories(string relativePath, List<string> directories, int userId) {
            if (!_cache.TryGetValue<Dictionary<string, string[]> >($"_DirectoriesCache_{userId}", out var cacheEntry)) {
                cacheEntry = _fileService.GetFilesInDirectories(Path.Combine(Settings.FilesDirectoryPath, relativePath), directories);
                _cache.Set($"_DirectoriesCache_{userId}", cacheEntry, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
            }

            return cacheEntry;
        }

        public OperationResult GetLastError() {
            return _fileService.GetLastError();
        }
    }
}
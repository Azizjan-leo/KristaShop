using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Http;
using Module.Common.Business.Interfaces;
using Serilog;

namespace Module.Common.Business.Services {
    public class FileService : IFileService {
        private readonly ILogger _logger;

        public FileService(ILogger logger) {
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string filesDirectoryPath, string directory) {
            try {
                if (file == null || string.IsNullOrEmpty(filesDirectoryPath) || string.IsNullOrEmpty(directory)) {
                    return string.Empty;
                }

                string imagePath = $"{directory}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                if (!Directory.Exists(filesDirectoryPath + directory))
                    Directory.CreateDirectory(filesDirectoryPath + directory);

                await using (var fileStream = new FileStream(filesDirectoryPath + imagePath, FileMode.OpenOrCreate)) {
                    await file.CopyToAsync(fileStream);
                }

                return imagePath;
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to save file {filesDirectory}{directory}. {message}", filesDirectoryPath,
                    directory, ex.Message);
                _lastError = $"Не удалось сохранить файл";
                return string.Empty;
            }
        }

        public async Task<string> SaveFileAsync(IFileDataProvider file, bool useOriginalName = false) {
            try {
                if (file == null || string.IsNullOrEmpty(file.FilesDirectoryPath) ||
                    string.IsNullOrEmpty(file.Directory)) {
                    return string.Empty;
                }

                var name = useOriginalName
                    ? file.OriginalName
                    : $"{Guid.NewGuid()}{Path.GetExtension(file.OriginalName)}";

                var filepath = $"{file.Directory}/{name}";
                if (!Directory.Exists($"{file.FilesDirectoryPath}{file.Directory}"))
                    Directory.CreateDirectory($"{file.FilesDirectoryPath}{file.Directory}");

                if (file.FileStream.CanSeek) {
                    file.FileStream.Seek(0, SeekOrigin.Begin);
                }

                await using (var fileStream =
                    new FileStream($"{file.FilesDirectoryPath}{filepath}", FileMode.OpenOrCreate)) {
                    await file.FileStream.CopyToAsync(fileStream);
                }

                return filepath;
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to save file {@file}. {message}", file, ex.Message);
                _lastError = $"Не удалось сохранить файл";
                return string.Empty;
            }
        }

        public bool RemoveFile(string filesDirectoryPath, string filepath) {
            var path = $"{filesDirectoryPath}{filepath}";
            try {
                if (File.Exists(path)) {
                    File.Delete(path);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete file {path}. {message}", path, ex.Message);
                _lastError = $"Не удалось удалить файл";
                return false;
            }

            return true;
        }

        public async Task<byte[]> GetFileAsync(string filesDirectoryPath, string filepath) {
            var path = $"{filesDirectoryPath}{filepath}";
            try {
                if (File.Exists(path)) {
                    await using (var memoryStream = new MemoryStream()) {
                        await using (var fileStream =
                            new FileStream($"{filesDirectoryPath}{filepath}", FileMode.Open)) {
                            await fileStream.CopyToAsync(memoryStream);
                        }

                        if (memoryStream.CanSeek) {
                            memoryStream.Seek(0, SeekOrigin.Begin);
                        }

                        return memoryStream.ToArray();
                    }
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get file {path}. {message}", path, ex.Message);
                _lastError = $"Не удалось получить файл";
                return null;
            }

            return null;
        }

        public List<FileInfo> GetDirectoriesFiles(string filesDirectoryPath, string directory) {
            var path = $"{filesDirectoryPath}{directory}";
            try {
                if (Directory.Exists(path)) {
                    var directoryInfo = new DirectoryInfo(path);
                    return directoryInfo.GetFiles().OrderBy(x => x.Name).ToList();
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get list of files in the directory {path}. {message}", path, ex.Message);
            }

            return new List<FileInfo>();
        }

        public Dictionary<string, string[]> GetFilesInDirectories(string filesDirectoryPath, List<string> directories) {
            var result = new Dictionary<string, string[]>();
            try {
                foreach (var directory in directories) {
                    var directoryInfo = new DirectoryInfo(Path.Combine(filesDirectoryPath, directory));
                    if (directoryInfo.Exists && !result.ContainsKey(directory)) {
                        result.Add(directory, directoryInfo.GetFiles().Select(x => x.Name).ToArray());
                    }
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to check if directories exist. filesDirectoryPath: {filesDirectoryPath} Directories: {@directories}. {message}",
                    filesDirectoryPath, directories, ex.Message);
            }

            return result;
        }

        private string _lastError = "";

        public OperationResult GetLastError() {
            return OperationResult.Failure(_lastError);
        }
    }
}
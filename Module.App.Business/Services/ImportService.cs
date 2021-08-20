using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Module.App.Business.Interfaces;
using Module.App.Business.Models;
using Module.App.Business.UnitOfWork;
using Module.Common.Business.Interfaces;
using Mono.Unix;

namespace Module.App.Business.Services {
    public class ImportService : IImportService {
        private readonly KristaShopDbContext _context;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly GlobalSettings _settings;
        private readonly IUnitOfWork _uow;

        private string _tablePrefixSearchFor1c = "1c_%";
        private string _mysqlFileGroup = "mysql";

        public ImportService(KristaShopDbContext context, IOptions<GlobalSettings> settings, IFileService fileService,
            IMapper mapper, IMemoryCache memoryCache, IUnitOfWork uow) {
            _context = context;
            _fileService = fileService;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _settings = settings.Value;
            _uow = uow;
        }

        public async Task CreateLackModelCatalogOrders() {
            try {
                await _uow.ModelCatalogOrder.CheckAllModels();
                await _uow.SaveChangesAsync();
            } catch (Exception) {
                throw;
            }
            return;
        }

        public async Task ImportDatabaseAsync(string script, int userId) {
            script = script.Replace("cp1251", "utf8");

            var keyValue = script.Substring(0, script.IndexOf(Environment.NewLine, StringComparison.Ordinal));
            var key = keyValue.Replace("-", "").Replace(" ", "").CalculateSha256Hash();
            var isBackupApplied = await _context.AppliedImports.AnyAsync(x => x.Key.Equals(key));
            if (isBackupApplied) {
                throw new Exception("Не удалось выполнить импорт. Данный файл уже был импортирован ранее.");
            }

            //var backupFilePath = await _backupTables();
            await _context.Database.ExecuteSqlRawAsync(script);

            //_context.AppliedImports.Add(new AppliedImport(key, keyValue, backupFilePath, userId));
            await _context.SaveChangesAsync();

            await _uow.ModelCatalogOrder.CheckAllModels();
            await _context.SaveChangesAsync();

            try {
                ((MemoryCache)_memoryCache).Compact(1.0);
            } catch (Exception ex) {
                throw new ExceptionBase("Failed to drop cache", ex);
            }
        }

        public async Task ApplyBackupAsync(string backupName) {
            await _applyBackupAsync(backupName);
        }

        public List<LookUpItem<string, string>> GetBackupsList() {
            var backupFiles = _getBackupFiles();

            return backupFiles
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new LookUpItem<string, string>(Path.GetFileNameWithoutExtension(x.Name), x.CreationTime.ToFormattedString()))
                .ToList();
        }

        public LookUpItem<string, string> GetLastAvailableBackup() {
            var backupFiles = _getBackupFiles().OrderByDescending(x => x.CreationTime).FirstOrDefault();

            return backupFiles == null ? null : new LookUpItem<string, string>(Path.GetFileNameWithoutExtension(backupFiles.Name), backupFiles.CreationTime.ToFormattedString());
        }

        public async Task DeleteOldBackupsAsync(int skipTopCount) {
            var backups = _getBackupFiles()
                .OrderByDescending(x=>x.CreationTime)
                .Skip(skipTopCount);

            foreach (var backup in backups) {
                var backupName = Path.GetFileNameWithoutExtension(backup.Name);
                File.Delete(backup.FullName);

                var dataFilesDirectory = await _getRootFilesDirectoryForBackupsAsync();
                var path = Path.Combine(dataFilesDirectory, backupName).InvertPathSeparator();
                if (Directory.Exists(path)) {
                    Directory.Delete(path, true);
                }
            }
        }

        #region backup

        public async Task<string> _backupTables() {
            var backupName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            var tables = await _getTablesToBackupAsync();
            var statements = await _getCreateStatements(tables);

            var mysqlFileFolder = await _getRootFilesDirectoryForBackupsAsync();
            var path = Path.Combine(mysqlFileFolder, backupName);

            await _makeTableDataBackupFiles(statements, path);
            return await _createJsonBackupFile(statements, backupName);
        }

        private async Task<string> _createJsonBackupFile(List<CreateTableStatement> statements, string backupName) {
            var backupModels = new List<BackupDTO>();
            for (int i = 0; i < statements.Count; i++) {
                var createStatement = "";
                createStatement += $"DROP TABLE IF EXISTS `{statements[i].Table}`;\r\n";
                createStatement += $"{statements[i].CreateTable};\r\n";

                var backupModel = new BackupDTO();
                backupModel.TableName = statements[i].Table;
                backupModel.Order = i;
                backupModel.CreateStatement = createStatement;
                backupModel.DataDumpFilePath = statements[i].Table;
                backupModels.Add(backupModel);
            }

            var json = JsonSerializer.Serialize(backupModels);
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(json));

            return await _saveJsonBackupFile(backupName, stream);
        }

        private async Task<string> _saveJsonBackupFile(string backupName, Stream stream) {
            var file = new FileDataProvider {
                FilesDirectoryPath = _settings.FilesDirectoryPath,
                Directory = _settings.DatabaseBackupsDirectory,
                FileStream = stream,
                OriginalName = $"{backupName}.json"
            };

            var backupFilePath = await _fileService.SaveFileAsync(file, true);
            if (string.IsNullOrEmpty(backupFilePath)) {
                throw new Exception("Failed to create json file with backup table create statements");
            }

            return backupFilePath;
        }

        private async Task _makeTableDataBackupFiles(List<CreateTableStatement> statements, string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                    var directory = new UnixDirectoryInfo(path) {
                        FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute |
                                                FileAccessPermissions.GroupReadWriteExecute,
                    };
                    directory.Refresh();
                }
            }

            var rootDirectory = await _getRootFilesDirectoryAsync();
            foreach (var statement in statements) {
                var tablePath = Path.Combine(rootDirectory, statement.Table).InvertPathSeparator();
                if (File.Exists(tablePath)) {
                    File.Delete(tablePath);
                }

                var query = $@"SELECT * INTO OUTFILE '{tablePath}'
                  FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '""'
                  LINES TERMINATED BY '\n'
                  FROM `{statement.Table}`;
                ";

                await _context.Database.ExecuteSqlRawAsync(query);
                File.Move(tablePath, Path.Combine(path, statement.Table).InvertPathSeparator());
            }
        }

        private async Task<List<CreateTableStatement>> _getCreateStatements(List<string> tables) {
            var createStatements = new List<CreateTableStatement>();
            foreach (var table in tables) {
                var statement = (await _context.Set<CreateTableStatement>().FromSqlRaw($"SHOW CREATE TABLE `{table}`")
                    .IgnoreQueryFilters()
                    .ToListAsync())
                    .FirstOrDefault();
                createStatements.Add(statement);
            }

            return createStatements;
        }

        private async Task<List<string>> _getTablesToBackupAsync() {
            var result = await _context.Set<Scalar>().FromSqlInterpolated($@"SELECT table_name as value FROM information_schema.tables
                        WHERE table_schema = {_context.Database.GetDbConnection().Database} and table_name like {_tablePrefixSearchFor1c};")
                .ToListAsync();

            return result.Select(x => x.Value).ToList();
        }

        #endregion

        #region rollback

        private async Task _applyBackupAsync(string backupName) {
            var backupFileInfo = _getBackupFile(backupName);

            var backupJson = await File.ReadAllTextAsync(backupFileInfo.FullName);
            var backupModels = JsonSerializer.Deserialize<List<BackupDTO>>(backupJson)
                .OrderBy(x => x.Order)
                .ToList();

            var createScript = string.Join("\r\n", backupModels.Select(x => x.CreateStatement));
            await _context.Database.ExecuteSqlRawAsync(createScript);

            var dumpsRootFolder = await _getRootFilesDirectoryForBackupsAsync();

            var dumpFolderName = Path.GetFileNameWithoutExtension(backupFileInfo.Name);
            var path = Path.Combine(dumpsRootFolder, dumpFolderName).InvertPathSeparator();
            var rootPath = await _getRootFilesDirectoryAsync();

            foreach (var backupModel in backupModels) {
                var tablePath = Path.Combine(path, backupModel.TableName).InvertPathSeparator();
                var tableRootPath = Path.Combine(rootPath, backupModel.TableName).InvertPathSeparator();
                File.Copy(tablePath, tableRootPath, true);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                    BashHelper.ExecuteCommand($"chgrp {_mysqlFileGroup} {tableRootPath}");
                }

                var query = $@"LOAD DATA INFILE '{tableRootPath}'
                            INTO TABLE `{backupModel.TableName}`
                            FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '""'
                            LINES TERMINATED BY '\n';";

                await _context.Database.ExecuteSqlRawAsync(query);
                File.Delete(tableRootPath);
            }
        }

        private FileInfo _getBackupFile(string backupName) {
            var directoryInfo = new DirectoryInfo(_settings.DatabaseBackupsPath);
            var backupFileInfo = directoryInfo.GetFiles()
                .FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.Name).Equals(backupName));

            if (backupFileInfo == null) {
                throw new FileNotFoundException("Не удалось найти backup файл");
            }

            return backupFileInfo;
        }

        #endregion

        private FileInfo[] _getBackupFiles() {
            if (!Directory.Exists(_settings.DatabaseBackupsPath)) {
                return Array.Empty<FileInfo>();
            }

            var directoryInfo = new DirectoryInfo(_settings.DatabaseBackupsPath);
            var backupFiles = directoryInfo.GetFiles();
            return backupFiles;
        }

        private async Task<string> _getRootFilesDirectoryAsync() {
            var mysqlFileFolder = (await _context.Set<Scalar>().FromSqlRaw("SELECT @@global.secure_file_priv as Value")
                    .ToListAsync())
                .First().Value;
            return mysqlFileFolder;
        }

        private async Task<string> _getRootFilesDirectoryForBackupsAsync() {
            var mysqlFileFolder = await _getRootFilesDirectoryAsync();
            return Path.Combine(mysqlFileFolder, "dumps").InvertPathSeparator();
        }

        public async Task<OperationResult> ValidateFolderAccessAsync() {
            var messages = new List<string>();
            try {
                var directoryPath = await _getRootFilesDirectoryForBackupsAsync();

                if (!Directory.Exists(directoryPath)) {
                    messages.Add($"{directoryPath} не существует");

                    Directory.CreateDirectory(directoryPath);
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                        var directory = new UnixDirectoryInfo(directoryPath) {
                            FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute |
                                                    FileAccessPermissions.GroupReadWriteExecute,
                        };
                        directory.Refresh();
                        messages.Add($"{directoryPath} успешно создана");
                    }
                }

                var innerDirectoryForBackupFiles = Path.Combine(directoryPath, "testBackupFolder").InvertPathSeparator();
                Directory.CreateDirectory(innerDirectoryForBackupFiles);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                    var directory = new UnixDirectoryInfo(innerDirectoryForBackupFiles) {
                        FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute |
                                                FileAccessPermissions.GroupReadWriteExecute,
                    };
                    directory.Refresh();
                    messages.Add($"{innerDirectoryForBackupFiles} успешно создана");

                    directory.Delete(true);
                    messages.Add($"{innerDirectoryForBackupFiles} успешно удалена");
                }
            }
            catch (Exception) {
                throw;
            }

            messages.Add("Проверка импорт сервиса прошла успешно");
            return OperationResult.Success(messages);
        }

        #region backup records

        public async Task<List<AppliedImportDTO>> GetAppliedImportsAsync() {
            var backups = await _context.AppliedImportItems.FromSqlRaw($@"SELECT
`backups`.`id`, `backups`.`key`, `backups`.`key_value`, `backups`.`apply_date`,
`backups`.`backup_file_path`, `backups`.`user_id`, IFNULL(`clients`.`login`, 'root') as login
FROM `applied_imports` AS `backups`
LEFT OUTER JOIN `1c_clients` AS `clients` ON `clients`.`id` = `backups`.`user_id`")
                .ProjectTo<AppliedImportDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return backups;
        }

        public async Task DeleteAppliedImportAsync(Guid id) {
            var appliedBackup = await _context.AppliedImports.FindAsync(id);
            _context.AppliedImports.Remove(appliedBackup);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
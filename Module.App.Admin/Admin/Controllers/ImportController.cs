using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.App.Admin.Admin.Models;
using Module.App.Business.Interfaces;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Base;
using Serilog;

namespace Module.App.Admin.Admin.Controllers {
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    public class ImportController : AppControllerBase {
        private readonly IImportService _importService;
        private readonly ILogger _logger;

        public ImportController(IImportService importService, ILogger logger) {
            _importService = importService;
            _logger = logger;
        }

        public IActionResult Backups() {
            return View();
        }

        public IActionResult LoadAvailableBackups() {
            try {
                return Ok(_importService.GetBackupsList());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get backups list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка бэкапов"));
            }
        }

        public IActionResult Imports() => View();

        public async Task<string> UpdateOrders() {
            try {
                await _importService.CreateLackModelCatalogOrders();
            } catch (Exception ex) {
                return ex.Message;
            }
            return "Ok!";
        }

        public async Task<IActionResult> LoadAppliedImports() {
            try {
                return Ok(await _importService.GetAppliedImportsAsync());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to get applied imports list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка импортов"));
            }
        }

        public async Task<IActionResult> ApplyBackup(string backupName) {
            try {
                if (string.IsNullOrEmpty(backupName)) {
                    SetNotification(OperationResult.Failure("Не указан файл восстановления"));
                    return RedirectToRefererOrAction(nameof(Execute));
                }

                await _importService.ApplyBackupAsync(backupName);
                SetNotification(OperationResult.Success("Восстановление выполнено успешно"));
                return RedirectToRefererOrAction(nameof(Execute));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to execute restore script. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Не удалось восстановить данные"));
                return RedirectToRefererOrAction(nameof(Execute));
            }
        }

        [HttpGet]
        public IActionResult Execute() {
            ViewData["LastBackup"] = _importService.GetLastAvailableBackup();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Execute(UploadDumpFileViewModel model) {
            try {
                ViewData["LastBackup"] = _importService.GetLastAvailableBackup();

                if (!ModelState.IsValid) {
                    return View(model);
                }
                
                var result = new StringBuilder();
                var encoding = Encoding.GetEncoding("windows-1251");
                using (var reader = new StreamReader(model.File.OpenReadStream(), encoding)) {
                    while (reader.Peek() >= 0)
                        result.AppendLine(await reader.ReadLineAsync());
                }

                await _importService.ImportDatabaseAsync(result.ToString(), UserSession.UserId);
                SetNotification(OperationResult.Success("Данные успешно импортированы"));
                return RedirectToAction(nameof(Execute));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to execute import script. {message}", ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [PermissionFilter(ForRootOnly = true)]
        public async Task<IActionResult> DeleteBackups(int skipTopCount) {
            try {
                await _importService.DeleteOldBackupsAsync(skipTopCount);
                SetNotification(OperationResult.Success("Бэкапы успешно удалены"));
                return RedirectToRefererOrAction(nameof(Backups));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete backups. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Не удалось удалить бэкапы"));
                return RedirectToRefererOrAction(nameof(Backups));
            }
        }

        [PermissionFilter(ForRootOnly = true)]
        public async Task<IActionResult> DeleteImport(Guid id) {
            try {
                await _importService.DeleteAppliedImportAsync(id);
                SetNotification(OperationResult.Success("Импорт успешно удален"));
                return RedirectToRefererOrAction(nameof(Backups));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete applied import. {message}", ex.Message);
                SetNotification(OperationResult.Failure("Не удалось удалить импорт"));
                return RedirectToRefererOrAction(nameof(Backups));
            }
        }

        public async Task<IActionResult> ValidateImport() {
            OperationResult result;
            try {
                result = await _importService.ValidateFolderAccessAsync();
            } catch (Exception ex) {
                _logger.Error(ex, "Import validation failed. {message}", ex.Message);
                result = OperationResult.Failure($"Возникла ошибка при валидации импорта. {ex.Message}");
            }

            return View(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.App.Business.Models;

namespace Module.App.Business.Interfaces {
    public interface IImportService {
        Task ImportDatabaseAsync(string script, int userId);
        Task ApplyBackupAsync(string backupName);
        List<LookUpItem<string, string>> GetBackupsList();
        LookUpItem<string, string> GetLastAvailableBackup();
        Task DeleteOldBackupsAsync(int skipTopCount);
        Task<List<AppliedImportDTO>> GetAppliedImportsAsync();
        Task DeleteAppliedImportAsync(Guid id);
        Task<OperationResult> ValidateFolderAccessAsync();
        Task CreateLackModelCatalogOrders();
    }
}

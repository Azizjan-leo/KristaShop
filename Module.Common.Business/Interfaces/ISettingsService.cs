using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Common.Business.Models;

namespace Module.Common.Business.Interfaces {
    public interface ISettingsService {
        Task<List<SettingsDTO>> GetSettingsAsync(bool isRoot);
        Task<SettingsDTO> GetByIdAsync(Guid id);
        Task<SettingsDTO> GetByKeyAsync(string key);
        Task<OperationResult> InsertAsync(SettingsDTO setting);
        Task<OperationResult> UpdateAsync(SettingsDTO setting, bool canChangeKey);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}
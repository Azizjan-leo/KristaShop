using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;

namespace Module.App.Business.Interfaces {
    public interface IManagerService {
        Task<List<ManagerDetailsDTO>> GetManagersAsync();
        Task<ManagerDetailsDTO> GetManagerAsync(int id);
        Task UpdateManagerAsync(ManagerDetailsDTO managerDto);
        Task<List<LookUpItem<int, string>>> GetManagersLookupListAsync();
    }
}
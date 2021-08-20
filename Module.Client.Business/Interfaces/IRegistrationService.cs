using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using Module.Client.Business.Models;

namespace Module.Client.Business.Interfaces {
    public interface IRegistrationService {
        Task<OperationResult> RegisterAsync(NewUserDTO newUser);
        Task<List<NewUserDTO>> GetRequestsAsync();
        Task<List<NewUserDTO>> GetGuestsRequestsAsync();
        Task<int> GetRequestsCountAsync();
        Task<int> GetRequestsCountAsync(int managerId);
        Task<UserClientDTO> GetRequestAsync(Guid id);
        Task<OperationResult> DeleteRequestAsync(Guid id);
        Task<OperationResult> ApproveRequestAsync(NewUserDTO newUserDTO, IEnumerable<CatalogType> visibleCatalogs, bool sendEmailToNewUser);
    }
}
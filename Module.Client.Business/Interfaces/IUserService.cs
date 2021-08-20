using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Module.Client.Business.Models;

namespace Module.Client.Business.Interfaces {
    public interface IUserService {
        Task<List<UserClientDTO>> GetUsersAsync(UserSession user, bool includeNew = false);
        Task<UserClientDTO> GetUserAsync(int id);
        Task<UserClientDTO> GetUserAsync(string login);
        Task<UserSession> GetUserDetailsAsync(UserSession session);
        Task<bool> IsActiveUserAsync(int userId);
        Task<bool> IsLoginExistsAsync(string login);
        Task<bool> ValidatePasswordAsync(int userId, string passwordHash);
        Task<OperationResult> ChangeUserPasswordAsync(int userId, string newPasswordHash, string newPasswordSrc);
    }
}

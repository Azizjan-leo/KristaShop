using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using Module.Client.Business.Models;

namespace Module.Client.Business.Interfaces {
    public interface ILinkService {
        Task<IResult<AuthorizationLinkDTO>> GetUserIdByRandCodeAsync(string code);
        Task<IResult<string>> InsertLinkAuthAsync(int userId, AuthorizationLinkType type = AuthorizationLinkType.MultipleAccess, bool fullPath = true);
        Task<IResult> RemoveLinksByUserIdAsync(int userId);
        Task<IResult> RemoveLinkByCodeAsync(string code);
        Task<bool> IsLinkExistAsync(string code);
    }
}
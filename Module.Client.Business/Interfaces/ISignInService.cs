using System;
using System.Security.Claims;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Module.Client.Business.Models;

namespace Module.Client.Business.Interfaces {
    public interface ISignInService {
        Task<OperationResult> SignIn(string login, string pass, bool isPersistent = false, bool isBackend = true, bool isFromApi = false);
        Task<OperationResult> SignIn(AuthorizationLinkDTO link);
        Task<ClaimsPrincipal> SignIn(GuestAccessInfo guestAccessInfo);
        Task SignOut(bool isBackend = true);
        Task SignOutGuest();
        string GetGuestLink(DateTime expiredDate, bool isPreoderVisible, bool isInstockByLinesVisible, bool isInstockByPartsVisible);
        GuestAccessInfo DecodeGuestAccessStr(string value);
        bool IsGuestAccessInfoCorrect(string value);
    }
}
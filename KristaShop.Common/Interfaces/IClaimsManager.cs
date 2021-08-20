using KristaShop.Common.Models.Session;
using Microsoft.AspNetCore.Http;

namespace KristaShop.Common.Interfaces {
    public interface IClaimsManager {
        public UserSession Session { get; }
        UserSession GetUserSession();
        string GetUserSessionClaim();
    }
}
using System.Security.Claims;
using System.Text.Json;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.Client.Business.Interfaces;

namespace Module.Client.Business.Services {
    public class ClaimsManager : IClaimsManager {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserSession _session;
        
        public UserSession Session => _session ??= GetUserSession();

        public ClaimsManager(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public UserSession GetUserSession() {
            if (_httpContextAccessor.HttpContext == null) return new UnauthorizedSession();
            return _getUserSession(_httpContextAccessor.HttpContext);
        }

        public string GetUserSessionClaim() {
            if (_httpContextAccessor.HttpContext == null) return string.Empty;
            return _getUserSessionClaim(_httpContextAccessor.HttpContext.User);
        }
        
        private UserSession _getUserSession(HttpContext httpContext) {
           var session = _getUserSession(httpContext.User);
           if (session == null) return new UnauthorizedSession();

           if (session.IsGuest) {
               return session;
           }

           var service = httpContext.RequestServices.GetService<IUserService>();
           return service!.GetUserDetailsAsync(session).Result;
        }

        private UserSession _getUserSession(ClaimsPrincipal user) {
            if (user.Identity == null || !user.Identity.IsAuthenticated) return null;

            if (user.HasClaim("Type", GlobalConstant.Session.FrontendScheme)) {
                var claim = user.FindFirstValue(GlobalConstant.Session.FrontendUserClaimName);
                if (!string.IsNullOrEmpty(claim)) {
                    return JsonSerializer.Deserialize<UserSession>(claim);
                }
            } else if (user.HasClaim("Type", GlobalConstant.Session.BackendScheme)) {
                var claim = user.FindFirstValue(GlobalConstant.Session.BackendUserClaimName);
                if (!string.IsNullOrEmpty(claim)) {
                    return JsonSerializer.Deserialize<UserSession>(claim);
                }
            } else if (user.HasClaim("Type", GlobalConstant.Session.FrontendGuestScheme)) {
                var claim = user.FindFirstValue(GlobalConstant.Session.FrontendGuestClaimName);
                if (!string.IsNullOrEmpty(claim)) {
                    return JsonSerializer.Deserialize<GuestSession>(claim);
                }
            }

            return null;
        }

        private string _getUserSessionClaim(ClaimsPrincipal user) {
            if (user.Identity == null || !user.Identity.IsAuthenticated) return null;

            if (user.HasClaim("Type", GlobalConstant.Session.FrontendScheme)) {
                return user.FindFirstValue(GlobalConstant.Session.FrontendUserClaimName);
            }

            if (user.HasClaim("Type", GlobalConstant.Session.BackendScheme)) {
                return user.FindFirstValue(GlobalConstant.Session.BackendUserClaimName);
            }

            if (user.HasClaim("Type", GlobalConstant.Session.FrontendGuestScheme)) {
                return user.FindFirstValue(GlobalConstant.Session.FrontendGuestClaimName);
            }

            return null;
        }
    }
}
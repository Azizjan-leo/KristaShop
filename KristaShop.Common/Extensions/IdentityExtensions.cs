using KristaShop.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using KristaShop.Common.Models.Session;

namespace KristaShop.Common.Extensions
{
    public static class IdentityExtensions {
        public static bool IsGuest(this IIdentity? identity)
        {
            if (identity == null) {
                return false;
            } else {
                if (identity.Name == "Guest") {
                    return true;
                } else {
                    return false;
                }
            }
        }

        public static bool IsGuest(this IPrincipal principal) {
            if (!(principal is ClaimsPrincipal claimsPrincipal)) return false;
            if (claimsPrincipal.HasClaim("Type", GlobalConstant.Session.FrontendGuestScheme)) {
                var claim = claimsPrincipal.FindFirstValue(GlobalConstant.Session.FrontendGuestClaimName);
                if (!string.IsNullOrEmpty(claim)) {
                    try {
                        var guestInfo = JsonSerializer.Deserialize<GuestSession>(claim);
                        if (guestInfo.Id != Guid.Empty) {
                            return true;
                        }
                    } catch (Exception ex) {

                    }
                }
            }

            return false;
        }

        public static bool IsGuest(this UserSession uSession) {
            return (uSession is GuestSession);
        }
    }
}

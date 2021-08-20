using Microsoft.AspNetCore.Http;
using Module.Common.WebUI.Extensions;
using Serilog.Core;
using Serilog.Events;

namespace KristaShop.WebUI.Infrastructure.Logging {
    public class UserCookieEnricher : ILogEventEnricher {
        private readonly IHttpContextAccessor _contextAccessor;
        
        public UserCookieEnricher(IHttpContextAccessor contextAccessor) {
            _contextAccessor = contextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("IsAuthenticated", 
                _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false));
            
            if(_contextAccessor.HttpContext is not null) {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserSession",
                    _contextAccessor.HttpContext?.GetUserSessionClaim()));
            }
        }
    }
}
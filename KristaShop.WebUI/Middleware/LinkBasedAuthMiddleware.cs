using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace KristaShop.WebUI.Middleware {
    public class LinkBasedAuthMiddleware {
        private readonly RequestDelegate _next;

        public LinkBasedAuthMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            var randh = context.Request.Query["randh"];
            var guestAccessInfo = context.Request.Query["guestAccess"];
            if (!string.IsNullOrEmpty(randh.ToString())) {
                var redirect = string.Empty;
                if (!context.Request.Query.TryGetValue("controller", out var controller)) {
                    controller = "Personal";
                }

                if (!context.Request.Query.TryGetValue("action", out var action)) {
                    if (context.Request.Query.TryGetValue("redirect", out var redirectUrl)) {
                        redirect = redirectUrl;
                    }
                } else {
                    redirect = $"/{controller}/{action}";
                    if (context.Request.Query.TryGetValue("id", out var docId)) {
                        redirect += $"/{docId}";
                    } else if (context.Request.Query.TryGetValue("params", out var queryParams)) {
                        redirect += $"?{queryParams.ToString()[1..^1]}";
                    }
                }

                context.Response.Redirect($"/Account/LoginByLink?code={randh}&returnUrl={Uri.EscapeDataString(redirect)}");
            } else if (!string.IsNullOrEmpty(guestAccessInfo.ToString())) {
                context.Request.Headers["Referer"] = "/";

                context.Response.Redirect($"/Account/TryLoginGuest?guestAccessCode={guestAccessInfo.ToString()}");
            } else {
                await _next.Invoke(context);
            }
        }
    }
}
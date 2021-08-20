using System;
using System.Threading.Tasks;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.App.Business.Interfaces;
using Module.Common.WebUI.Base;

namespace Module.App.WebUI.Infrastructure {
    public class PermissionManager : IPermissionManager {
        public async Task<bool> HasAccessAsync(HttpContext context, Guid roleId, RouteValue route) {
            var roleAccessService = context.RequestServices.GetRequiredService<IRoleAccessService>();
            return await roleAccessService.HasAccessAsync(roleId, route);
        }
    }
}
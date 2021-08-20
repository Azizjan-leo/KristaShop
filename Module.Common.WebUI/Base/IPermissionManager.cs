using System;
using System.Threading.Tasks;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Http;

namespace Module.Common.WebUI.Base {
    public interface IPermissionManager {
        Task<bool> HasAccessAsync(HttpContext context, Guid roleId, RouteValue route);
    }
}
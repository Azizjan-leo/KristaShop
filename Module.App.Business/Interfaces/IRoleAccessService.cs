using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models.Structs;
using Module.App.Business.Models;

namespace Module.App.Business.Interfaces {
    public interface IRoleAccessService {
        Task<RoleDTO> GetRoleAsync(Guid id);
        Task<List<RoleDTO>> GetRolesAsync();
        Task CreateRoleAsync(RoleDTO roleDto);
        Task UpdateRoleAsync(RoleDTO roleDto);
        Task<List<RoleAccessDTO>> GetRoleAccessesAsync(Guid roleId);
        Task UpdateRoleAccessesAsync(List<RoleAccessDTO> roleAccessDtos);
        Task<bool> HasAccessAsync(Guid roleId, RouteValue routeValue);
        Task<List<RoleAccessDTO>> HasAccessToRoutesAsync(Guid roleId, List<RouteValue> routeValues);
    }
}

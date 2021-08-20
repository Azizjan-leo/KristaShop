using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Module.App.Business.Interfaces;
using Module.App.Business.Models;
using Module.App.Business.UnitOfWork;

namespace Module.App.Business.Services {
    public class RoleAccessService : IRoleAccessService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoleAccessService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RoleDTO> GetRoleAsync(Guid id) {
            var role = await _uow.RoleAccesses.GetRoleAsync(id);
            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<List<RoleDTO>> GetRolesAsync() {
            return await _uow.Roles.All.ProjectTo<RoleDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<List<RoleAccessDTO>> GetRoleAccessesAsync(Guid roleId) {
            return await _uow.RoleAccesses.All
                .Where(x => x.RoleId == roleId)
                .ProjectTo<RoleAccessDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task CreateRoleAsync(RoleDTO roleDto) {
            if (await _uow.Roles.All.Where(x => x.Name.Equals(roleDto.Name) && x.Id != roleDto.Id).AnyAsync()) {
                throw new EntityNotFoundException($"Entity role access with name {roleDto.Name} already exist", $"Роль '{roleDto.Name}' уже существует");
            }

            var newRole = _mapper.Map<Role>(roleDto);
            await _uow.Roles.AddAsync(newRole, true);
            await _uow.SaveAsync();
        }

        public async Task UpdateRoleAsync(RoleDTO roleDto) {
            if (await _uow.Roles.All.Where(x => x.Name.Equals(roleDto.Name)).AnyAsync()) {
                throw new EntityNotFoundException($"Entity role access with name {roleDto.Name} already exist", $"Роль '{roleDto.Name}' уже существует");
            }

            var role = await _uow.Roles.GetByIdAsync(roleDto.Id);
            _mapper.Map(roleDto, role);
            _uow.Roles.Update(role);
            await _uow.SaveAsync();
        }

        public async Task UpdateRoleAccessesAsync(List<RoleAccessDTO> roleAccessDtos) {
            using (await _uow.BeginTransactionAsync()) {
                await _addNewRoleAccessesAsync(roleAccessDtos.Where(x => x.Id == Guid.Empty));
                await _updateRoleAccessesAsync(roleAccessDtos.Where(x => x.Id != Guid.Empty));
                await _uow.SaveAsync();
                _uow.CommitTransaction();
            }
        }

        private async Task _updateRoleAccessesAsync(IEnumerable<RoleAccessDTO> accessesToUpdate) {
            if (!accessesToUpdate.Any()) {
                return;
            }

            var toUpdate = accessesToUpdate.ToDictionary(k => k.Id, v => v);
            var toUpdateIds = toUpdate.Values.Select(x => x.Id).ToList();
            var roleAccesses = await _uow.RoleAccesses.All.Where(x => toUpdateIds.Contains(x.Id)).ToListAsync();
            foreach (var roleAccess in roleAccesses) {
                if (toUpdate.ContainsKey(roleAccess.Id)) {
                    roleAccess.IsAccessGranted = toUpdate[roleAccess.Id].IsAccessGranted;
                }
            }
            _uow.RoleAccesses.UpdateRange(roleAccesses);
        }

        private async Task _addNewRoleAccessesAsync(IEnumerable<RoleAccessDTO> accessesToAdd) {
            if (accessesToAdd.Any()) {
                var newAccesses = _mapper.Map<List<RoleAccess>>(accessesToAdd);
                newAccesses.ForEach(x => x.Id = Guid.NewGuid());
                await _uow.RoleAccesses.AddRangeAsync(newAccesses);
            }
        }

        public async Task<bool> HasAccessAsync(Guid roleId, RouteValue routeValue) {
            return await _uow.RoleAccesses.HasAccessAsync(roleId, routeValue);
        }

        public async Task<List<RoleAccessDTO>> HasAccessToRoutesAsync(Guid roleId, List<RouteValue> routeValues) {
            var source = await _uow.RoleAccesses.HasAccessToRoutesAsync(roleId, routeValues);
            return _mapper.Map <List<RoleAccessDTO>>(source);
        }
    }
}

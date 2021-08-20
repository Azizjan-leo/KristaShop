using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Module.App.Business.Interfaces;
using Module.App.Business.Models;
using Module.App.Business.UnitOfWork;

namespace Module.App.Business.Services {
    public class MenuService : IMenuService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MenuService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<MenuItemDTO>> GetMenuItemsAsync() {
            return await _uow.MenuItems.All
                .ProjectTo<MenuItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<MenuItemDTO>> GetMenuItemsByTypeAsync(MenuType menuType) {
            return await _uow.MenuItems.All
                .Where(x => x.MenuType == menuType)
                .ProjectTo<MenuItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<MenuItemDTO>> GetMenuItemsWithSameParent(MenuType menuType, string controller) {
            var parentItem = await _uow.MenuItems.All
                .Include(x => x.ParentItem.ChildItems)
                .Where(x => x.MenuType == menuType && x.ControllerName.Equals(controller))
                .Select(x => x.ParentItem)
                .FirstOrDefaultAsync();

            if (parentItem == null || parentItem.ChildItems == null || !parentItem.ChildItems.Any())
                return new List<MenuItemDTO>();

            return _mapper.Map<List<MenuItemDTO>>(parentItem.ChildItems);
        }

        public async Task<MenuItemDTO> GetMenuItemAsync(Guid id) {
            var menu = await _uow.MenuItems.GetByIdAsync(id);
            return _mapper.Map<MenuItemDTO>(menu);
        }

        public async Task<MenuItemDTO> GetMenuItemAsync(string controller, string action) {
            return _mapper.Map<MenuItemDTO>(await _uow.MenuItems.All
                .FirstOrDefaultAsync(x => x.ControllerName == controller && x.ActionName == action));
        }

        public async Task<OperationResult> InsertMenuItemAsync(MenuItemDTO menuItem) {
            var entity = _mapper.Map<MenuItem>(menuItem);
            await _uow.MenuItems.AddAsync(entity, true);

            return (await _uow.SaveAsync()) ? OperationResult.Success() : OperationResult.Failure();
        }

        public async Task<OperationResult> UpdateMenuItemAsync(MenuItemDTO menuItem) {
            var entity = _mapper.Map<MenuItem>(menuItem);
            _uow.MenuItems.Update(entity);
            return (await _uow.SaveAsync()) ? OperationResult.Success() : OperationResult.Failure();
        }

        public async Task<OperationResult> DeleteMenuItemAsync(Guid id) {
            await _uow.MenuItems.DeleteAsync(id);
            return (await _uow.SaveAsync()) ? OperationResult.Success() : OperationResult.Failure();
        }
    }
}
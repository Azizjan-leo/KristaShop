using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using Module.App.Business.Models;

namespace Module.App.Business.Interfaces {
    public interface IMenuService {
        Task<List<MenuItemDTO>> GetMenuItemsAsync();
        Task<List<MenuItemDTO>> GetMenuItemsByTypeAsync(MenuType menuType);
        Task<List<MenuItemDTO>> GetMenuItemsWithSameParent(MenuType menuType, string controller);
        Task<MenuItemDTO> GetMenuItemAsync(Guid id);
        Task<MenuItemDTO> GetMenuItemAsync(string controller, string action);
        Task<OperationResult> InsertMenuItemAsync(MenuItemDTO menuItem);
        Task<OperationResult> UpdateMenuItemAsync(MenuItemDTO menuItem);
        Task<OperationResult> DeleteMenuItemAsync(Guid id);
    }
}
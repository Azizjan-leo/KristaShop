using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface IUserRepository : IRepository<User, int> {
        new Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetAllByManagersAsync(IEnumerable<int> managerIds);
        Task<IEnumerable<User>> GetAllManagersAsync();
        Task<User?> GetManagerAsync(int userId);
        Task<User?> GetByIdWithDataAsync(int userId);
        Task<User?> GetByLoginAsync(string login);
        Task<bool> IsActiveAsync(int userId);
        Task<bool> IsAlreadyRegisteredAsync(string login, string phone = "");
        Task<IEnumerable<CatalogType>> GetUserAvailableCatalogsOrOpenCatalogAsync(int userId);
        Task<Dictionary<CatalogType, bool>> GetUserCatalogsAsync(int userId);
        Task<bool> HasAccessToCatalogAsync(int userId, CatalogType catalogId);
        Task SetCatalogVisibilityAsync(int userId, CatalogType catalogId, bool hasAccess);
    }
}
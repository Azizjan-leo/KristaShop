using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class UserRepository : Repository<User, int>, IUserRepository {
        public UserRepository(DbContext context) : base(context) { }

        public override async Task<IEnumerable<User>> GetAllAsync() {
            return await All
                .Include(x => x.Data)
                .Include(x => x.Manager)
                .Include(x => x.City)
                .Where(x => !x.IsManager)
                .OrderBy(x => x.FullName)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<User>> GetAllByManagersAsync(IEnumerable<int> managerIds) {
            return await All
                .Include(x => x.Data)
                .Include(x => x.Manager)
                .Include(x => x.City)
                .Where(x => !x.IsManager && managerIds.Contains(x.ManagerId.Value))
                .OrderBy(x => x.FullName)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<User>> GetAllManagersAsync() {
            return await All.Include(x => x.Data)
                .Include(x => x.Manager)
                .ThenInclude(x => x.Details)
                .Where(x => x.IsManager)
                .ToListAsync();
        }

        public async Task<User?> GetManagerAsync(int userId) {
            return await All.Include(x => x.Data)
                .Include(x => x.Manager)
                .ThenInclude(x => x.Details)
                .FirstOrDefaultAsync(x => x.IsManager && x.Id == userId);
        }

        public virtual async Task<User?> GetByIdWithDataAsync(int userId) {
            return await All.Include(x => x.Data).Where(x => x.Id == userId).FirstOrDefaultAsync();
        }
        
        public virtual async Task<User?> GetByLoginAsync(string login) {
            return await All.Include(x => x.Data).Where(x => x.Login == login).FirstOrDefaultAsync();
        }

        public virtual async Task<bool> IsActiveAsync(int userId) {
            return await All.AnyAsync(x => x.Id == userId);
        }

        public virtual async Task<bool> IsAlreadyRegisteredAsync(string login, string phone = "") {
            var query = All.Where(x => x.Login == login);
            if (!string.IsNullOrEmpty(phone)) {
                query = query.Where(x => x.Phone == phone);
            }

            return await query.AnyAsync();
        }

        public virtual async Task<IEnumerable<CatalogType>> GetUserAvailableCatalogsOrOpenCatalogAsync(int userId) {
            var user = await GetByIdAsync(userId);
            if (user != null) {
                return user.GetOnlyAvailableCatalogs();
            }

            return new List<CatalogType> {CatalogType.Open};
        }

        public virtual async Task<Dictionary<CatalogType, bool>> GetUserCatalogsAsync(int userId) {
            var user = await GetByIdAsync(userId);
            if (user == null) {
                throw new EntityNotFoundException($"User {userId} not found");
            }
            
            return user.GetAccessesToCatalogs();
        }

        public async Task<bool> HasAccessToCatalogAsync(int userId, CatalogType catalogId) {
            var user = await GetByIdAsync(userId);
            if (user == null) {
                throw new EntityNotFoundException($"User {userId} not found");
            }
            
            return user.HasAccessTo(catalogId);
        }

        public virtual async Task SetCatalogVisibilityAsync(int userId, CatalogType catalogId, bool hasAccess) {
            var user = await GetByIdAsync(userId);
            user.SetCatalogAccess(catalogId, hasAccess);
            Update(user);
        }
    }
}
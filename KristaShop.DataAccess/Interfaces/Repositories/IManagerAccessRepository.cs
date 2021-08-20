using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using System.Linq;

namespace KristaShop.DataAccess.Interfaces.Repositories {
    public interface IManagerAccessRepository<T, in TU> : IRepository<T, TU> where T : class {
        IQueryable<int> GetManagerIdsAccessesFor(int managerId, ManagerAccessToType accessTo);
    }
}

using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Interfaces.Repositories.Media {
    public interface IDynamicPageRepository<T, in TU> : IRepository<T, TU> where T : class {
        IQueryable<T> GetAllOrderedOpenAndVisibleInMenuOnly(bool openOnly, bool visibleInMenuOnly);
        Task<int> GetNewOrderValueAsync();
    }
}

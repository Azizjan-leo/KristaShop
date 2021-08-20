using System.Linq;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Interfaces.Repositories.Media {
    public interface IVideoGalleryRepository<T, in TU> : IRepository<T, TU> where T : class {
        IQueryable<T> GetAllOnlyVisibleAndOpenOrdered(bool onlyVisible = false, bool onlyOpen = false);
    }
}

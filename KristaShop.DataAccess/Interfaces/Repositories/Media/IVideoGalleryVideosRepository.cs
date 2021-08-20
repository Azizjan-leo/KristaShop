using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Interfaces.Repositories.Media {
    public interface IVideoGalleryVideosRepository<T, in TU> : IRepository<T, TU> where T : class {
        IQueryable<T> GetAllOrderedByGallery(Guid galleryId);
        IQueryable<T> GetAllOrderedOnlyVisibleVideos(bool onlyVisible = false);
        IQueryable<T> GetInGalleryOnlyVisibleOrdered(Guid galleryId, bool onlyVisible);
        Task<T> GetByGalleryAndVideoIdAsync(Guid galleryId, Guid videoId);
        Task<int> GetNewOrderValueAsync(Guid galleryId);
    }
}

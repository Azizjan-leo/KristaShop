using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using KristaShop.DataAccess.Interfaces.Repositories.Media;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.Media {
    public class VideoGalleryVideosRepository : Repository<VideoGalleryVideos, Guid>, IVideoGalleryVideosRepository<VideoGalleryVideos, Guid> {
        public VideoGalleryVideosRepository(DbContext context) : base(context) { }

        public override IOrderedQueryable<VideoGalleryVideos> AllOrdered => All.OrderBy(x => x.Order);

        public IQueryable<VideoGalleryVideos> GetAllOrderedByGallery(Guid galleryId) {
            return AllOrdered.Where(x => x.GalleryId == galleryId);
        }

        public IQueryable<VideoGalleryVideos> GetAllOrderedOnlyVisibleVideos(bool onlyVisible = false) {
            return onlyVisible ? AllOrdered.Where(x => x.Video.IsVisible) : AllOrdered;
        }

        public IQueryable<VideoGalleryVideos> GetInGalleryOnlyVisibleOrdered(Guid galleryId, bool onlyVisible) {
            return GetAllOrderedOnlyVisibleVideos(onlyVisible).Where(x => x.GalleryId == galleryId);
        }

        public async Task<VideoGalleryVideos> GetByGalleryAndVideoIdAsync(Guid galleryId, Guid videoId) {
            return await All.FirstOrDefaultAsync(x=>x.GalleryId == galleryId && x.VideoId == videoId);
        }

        public async Task<int> GetNewOrderValueAsync(Guid galleryId) {
            return await All.Where(x => x.GalleryId == galleryId).MaxAsync(x => (int?) x.Order) + 1 ?? 1;
        }
    }
}
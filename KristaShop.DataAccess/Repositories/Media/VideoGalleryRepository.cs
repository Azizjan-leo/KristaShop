using System;
using System.Linq;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using KristaShop.DataAccess.Interfaces.Repositories.Media;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.Media {
    public class VideoGalleryRepository : Repository<VideoGallery, Guid>, IVideoGalleryRepository<VideoGallery, Guid> {
        public VideoGalleryRepository(DbContext context) : base(context) { }

        public override IOrderedQueryable<VideoGallery> AllOrdered => All.OrderBy(x => x.Order);

        public IQueryable<VideoGallery> GetAllOnlyVisibleAndOpenOrdered(bool onlyVisible = false, bool onlyOpen = false) {
            var request = onlyVisible ? AllOrdered.Where(x => x.IsVisible) : AllOrdered;
            return onlyOpen ? request.Where(x => x.IsOpen) : request;
        }
    }
}

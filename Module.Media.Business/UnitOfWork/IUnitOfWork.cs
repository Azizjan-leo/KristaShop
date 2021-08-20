using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.Media;
using KristaShop.DataAccess.Interfaces.Repositories.Media;

namespace Module.Media.Business.UnitOfWork {
    public interface IUnitOfWork : IUnitOfWorkBase {
        IRepository<Video, Guid> Videos { get; }
        IVideoGalleryRepository<VideoGallery, Guid> VideoGalleries { get; }
        IVideoGalleryVideosRepository<VideoGalleryVideos, Guid> VideoGalleryVideos { get; }
        IDynamicPageRepository<DynamicPage, Guid> DynamicPage { get; }
        IFaqRepository<Faq, Guid> FaqRepository { get; }
        IFaqSectionRepository<FaqSection, Guid> FaqSectionRepository { get; }
        IFaqSectionContentRepository<FaqSectionContent, Guid> FaqSectionContentRepository { get; }
        IFaqSectionContentFileRepository<FaqSectionContentFile, Guid> FaqSectionContentFileRepository { get; }
        IRepository<BannerItem, Guid> Banners { get; }
        IRepository<BlogItem, Guid> BlogItems { get; }
        IRepository<GalleryItem, Guid> GalleryItems { get; }
    }
}
using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.Media;
using KristaShop.DataAccess.Interfaces.Repositories.Media;
using KristaShop.DataAccess.Repositories;
using KristaShop.DataAccess.Repositories.Media;
using Serilog;

namespace Module.Media.Business.UnitOfWork {
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork {
        private readonly Lazy<IRepository<Video, Guid>> _videoRepository;
        private readonly Lazy<IVideoGalleryRepository<VideoGallery, Guid>> _videoGalleryRepository;
        private readonly Lazy<IFaqRepository<Faq, Guid>> _faqRepository;
        private readonly Lazy<IFaqSectionRepository<FaqSection, Guid>> _faqSectionRepository;
        private readonly Lazy<IFaqSectionContentFileRepository<FaqSectionContentFile, Guid>> _faqSectionContentFileRepository;
        private readonly Lazy<IFaqSectionContentRepository<FaqSectionContent, Guid>> _faqSectionContentRepository;
        private readonly Lazy<IVideoGalleryVideosRepository<VideoGalleryVideos, Guid>> _videoGalleryVideosRepository;
        private readonly Lazy<IDynamicPageRepository<DynamicPage, Guid>> _dynamicPageRepository;
        private readonly Lazy<IRepository<BannerItem, Guid>> _bannerItemRepository;
        private readonly Lazy<IRepository<BlogItem, Guid>> _blogItemRepository;
        private readonly Lazy<IRepository<GalleryItem, Guid>> _galleryItemsRepository;

        public IRepository<Video, Guid> Videos => _videoRepository.Value;
        public IVideoGalleryRepository<VideoGallery, Guid> VideoGalleries => _videoGalleryRepository.Value;
        public IVideoGalleryVideosRepository<VideoGalleryVideos, Guid> VideoGalleryVideos => _videoGalleryVideosRepository.Value;
        public IDynamicPageRepository<DynamicPage, Guid> DynamicPage => _dynamicPageRepository.Value;
        public IFaqRepository<Faq, Guid> FaqRepository => _faqRepository.Value;
        public IFaqSectionRepository<FaqSection, Guid> FaqSectionRepository => _faqSectionRepository.Value;
        public IFaqSectionContentRepository<FaqSectionContent, Guid> FaqSectionContentRepository => _faqSectionContentRepository.Value;
        public IFaqSectionContentFileRepository<FaqSectionContentFile, Guid> FaqSectionContentFileRepository => _faqSectionContentFileRepository.Value;
        public IRepository<BannerItem, Guid> Banners => _bannerItemRepository.Value;
        public IRepository<BlogItem, Guid> BlogItems => _blogItemRepository.Value;
        public IRepository<GalleryItem, Guid> GalleryItems => _galleryItemsRepository.Value;
        
        public UnitOfWork(KristaShopDbContext context, ILogger logger) : base(context, logger) {
            _videoRepository = new Lazy<IRepository<Video, Guid>>(() => new Repository<Video, Guid>(Context));
            _videoGalleryRepository = new Lazy<IVideoGalleryRepository<VideoGallery, Guid>>(() => new VideoGalleryRepository(Context));
            _faqRepository = new Lazy<IFaqRepository<Faq, Guid>>(() => new FaqRepository(Context));
            _faqSectionRepository = new Lazy<IFaqSectionRepository<FaqSection, Guid>>(() => new FaqSectionRepository(Context));
            _faqSectionContentFileRepository = new Lazy<IFaqSectionContentFileRepository<FaqSectionContentFile, Guid>>(() => new FaqSectionContentFileRepository(Context));
            _faqSectionContentRepository = new Lazy<IFaqSectionContentRepository<FaqSectionContent, Guid>>(() => new FaqSectionContentRepository(Context));
            _videoGalleryVideosRepository = new Lazy<IVideoGalleryVideosRepository<VideoGalleryVideos, Guid>>(() => new VideoGalleryVideosRepository(Context));
            _dynamicPageRepository = new Lazy<IDynamicPageRepository<DynamicPage, Guid>>(() => new DynamicPageRepository(Context));
            _bannerItemRepository = new Lazy<IRepository<BannerItem, Guid>>(() => new Repository<BannerItem, Guid>(Context));
            _blogItemRepository = new Lazy<IRepository<BlogItem, Guid>>(() => new Repository<BlogItem, Guid>(Context));
            _galleryItemsRepository = new Lazy<IRepository<GalleryItem, Guid>>(() => new Repository<GalleryItem, Guid>(Context));
        }
    }
}
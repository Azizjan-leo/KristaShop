using Microsoft.Extensions.DependencyInjection;
using Module.Media.Business.Interfaces;
using Module.Media.Business.Services;
using Module.Media.Business.UnitOfWork;

namespace Module.Media.Business {
    public static class ServicesDIRegistration {
        public static void AddServices(this IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
                
            services.AddSingleton<IDynamicPagesManager, DynamicPagesManager>();
            services.AddScoped<IDynamicPagesService, DynamicPagesService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<IBannerService, BannerService>();
            
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IVideoGalleryService, VideoGalleryService>();
            services.AddScoped<IFaqService, FaqService>();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Services;
using Module.Catalogs.Business.UnitOfWork;

namespace Module.Catalogs.Business {
    public static class ServicesDIRegistration {
        public static void AddServices(this IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<ICatalogItemsService, CatalogItemsService>();
            services.AddScoped<ICatalogAccessService, CatalogAccessService>();
            services.AddScoped<ICatalogItemUpdateService, CatalogItemUpdateService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFavorite1CService, Favorite1CService>();
        }
    }
}
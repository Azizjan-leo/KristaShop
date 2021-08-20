using Microsoft.Extensions.DependencyInjection;
using Module.App.Business.Interfaces;
using Module.App.Business.Services;
using Module.App.Business.UnitOfWork;

namespace Module.App.Business {
    public static class ServicesDIRegistration {
        public static void AddServices(this IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            
            services.AddScoped<IPromoLinkService, PromoLinkService>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IRoleAccessService, RoleAccessService>();
        }
    }
}
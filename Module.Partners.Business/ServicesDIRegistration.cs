using Microsoft.Extensions.DependencyInjection;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.Services;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business {
    public static class ServicesDIRegistration {
        public static void AddServices(this IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IPartnerStorehouseService, PartnerStorehouseService>();
            services.AddScoped<IPartnerStorehouseReportService, PartnerStorehouseReportService>();
            services.AddScoped<IPartnerDocumentsService, PartnerDocumentsService>();
            services.AddScoped<ISellingRequestsService, SellingRequestsService>();
        }
    }
}
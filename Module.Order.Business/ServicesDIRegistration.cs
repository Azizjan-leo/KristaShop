using Microsoft.Extensions.DependencyInjection;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Services;
using Module.Order.Business.UnitOfWork;

namespace Module.Order.Business {
    public static class ServicesDIRegistration {
        public static void AddServices(this IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrder1CService, Order1CService>();
            services.AddScoped<IOrderReportService, OrderReportService>();
            services.AddScoped<IClientReportService, ClientReportService>();
        }
    }
}
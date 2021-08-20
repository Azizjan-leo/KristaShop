using KristaShop.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Module.Client.Business.Interfaces;
using Module.Client.Business.Services;
using Module.Client.Business.UnitOfWork;

namespace Module.Client.Business {
    public static class ServicesDIRegistration {
        public static void AddServices(this IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IClaimsManager, ClaimsManager>();
        }
    }
}
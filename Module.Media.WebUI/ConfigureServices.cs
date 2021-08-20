using Microsoft.Extensions.DependencyInjection;
using Module.Media.Business;
using Module.Media.Business.Interfaces;

namespace Module.Media.WebUI {
    public static class ConfigureServices {
        public static void ConfigureModuleServices(IServiceCollection services) {
            services.AddServices();
        }

        public static void OnInitializeApplication(IServiceScope serviceScope) {
            var dynamicPagesManager = serviceScope.ServiceProvider.GetRequiredService<IDynamicPagesManager>();
            dynamicPagesManager.InitializeAsync(serviceScope);
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Module.Common.Business;
using Module.Common.Business.Interfaces;

namespace Module.Common.WebUI {
    public static class ConfigureServices {
        public static void ConfigureModuleServices(IServiceCollection services) {
            services.AddServices();
        }

        public static void OnInitializeApplication(IServiceScope serviceScope) {
            var settingsManager = serviceScope.ServiceProvider.GetRequiredService<ISettingsManager>();
            settingsManager.InitializeAsync(serviceScope);
        }
    }
}
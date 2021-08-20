using Microsoft.Extensions.DependencyInjection;
using Module.Catalogs.Business;

namespace Module.Catalogs.WebUI {
    public static class ConfigureServices {
        public static void ConfigureModuleServices(IServiceCollection services) {
            services.AddServices();
        }
    }
}
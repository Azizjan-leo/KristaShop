using Microsoft.Extensions.DependencyInjection;
using Module.Partners.Business;

namespace Module.Partners.WebUI {
    public static class ConfigureServices {
        public static void ConfigureModuleServices(IServiceCollection services) {
            services.AddServices();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Module.Client.Business;

namespace Module.Client.WebUI {
    public static class ConfigureServices {
        public static void ConfigureModuleServices(IServiceCollection services) {
            services.AddServices();
        }
    }
}
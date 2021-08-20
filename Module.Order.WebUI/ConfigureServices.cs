using Microsoft.Extensions.DependencyInjection;
using Module.Order.Business;

namespace Module.Order.WebUI {
    public static class ConfigureServices {
        public static void ConfigureModuleServices(IServiceCollection services) {
            services.AddServices();
        }
    }
}
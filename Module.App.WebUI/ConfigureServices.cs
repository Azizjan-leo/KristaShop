using Microsoft.Extensions.DependencyInjection;
using Module.App.Business;
using Module.App.WebUI.Infrastructure;
using Module.Common.WebUI.Base;

namespace Module.App.WebUI {
    public static class ConfigureServices {
        public static void ConfigureModuleServices(IServiceCollection services) {
            services.AddServices();

            services.AddSingleton<IPermissionManager, PermissionManager>();
        }
    }
}
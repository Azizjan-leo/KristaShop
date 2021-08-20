using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KristaShop.WebUI.Infrastructure {
    public static class InitializeApplication {
        public static void InitializeApp(this IApplicationBuilder application, IServiceScopeFactory serviceScopeFactory) {
            Module.Common.WebUI.ConfigureServices.OnInitializeApplication(serviceScopeFactory.CreateScope());
            Module.Media.WebUI.ConfigureServices.OnInitializeApplication(serviceScopeFactory.CreateScope());
        }
    }
}

using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace KristaShop.WebUI {
    public static class ModulesDeclaration {
        private static readonly string[] ModulesAssembliesNames;

        static ModulesDeclaration() {
            ModulesAssembliesNames = new[] {
                "Module.Common.WebUI",
                "Module.Common.Admin",
                "Module.App.WebUI",
                "Module.App.Admin",
                "Module.Media.WebUI",
                "Module.Media.Admin",
                "Module.Partners.WebUI",
                "Module.Partners.Admin",
                "Module.Catalogs.WebUI",
                "Module.Catalogs.Admin",
                "Module.Cart.WebUI",
                "Module.Cart.Admin",
                "Module.Order.WebUI",
                "Module.Order.Admin",
                "Module.Client.WebUI",
                "Module.Client.Admin",
            };
        }

        public static void ConfigureServices(IServiceCollection services) {
            Module.Common.WebUI.ConfigureServices.ConfigureModuleServices(services);
            Module.App.WebUI.ConfigureServices.ConfigureModuleServices(services);
            Module.Media.WebUI.ConfigureServices.ConfigureModuleServices(services);
            Module.Partners.WebUI.ConfigureServices.ConfigureModuleServices(services);
            Module.Catalogs.WebUI.ConfigureServices.ConfigureModuleServices(services);
            Module.Order.WebUI.ConfigureServices.ConfigureModuleServices(services);
            Module.Client.WebUI.ConfigureServices.ConfigureModuleServices(services);
        }
        
        public static  Assembly[] GetAssemblies() {
            return ModulesAssembliesNames.Select(Assembly.Load).ToArray();
        }
    }
}
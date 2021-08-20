using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace KristaShop.WebUI.Infrastructure {
    public static class ModulesConfiguration {
        public static IMvcBuilder AddApplicationModules(this IMvcBuilder builder, params Assembly[] assemblies) {
            foreach (var assembly in assemblies) {
                builder.AddApplicationPart(assembly);
            }
            
            return builder;
        }

        public static IServiceCollection ConfigureModulesRuntimeCompilation
            (this IServiceCollection services, string contentRootPath, params Assembly[] assemblies) {
            services.Configure<MvcRazorRuntimeCompilationOptions>(options => {
                options.FileProviders.Add(new CompositeFileProvider(assemblies.Select(assembly => new EmbeddedFileProvider(assembly))));
            });
            
            services.Configure<MvcRazorRuntimeCompilationOptions>(options => {
                var paths = assemblies.Select(assembly => Path.GetFullPath(Path.Combine(contentRootPath,
                    "..", assembly.GetName().Name ?? throw new InvalidOperationException("Assembly name is null"))));
                options.FileProviders.Add(new CompositeFileProvider(paths.Select(path => new PhysicalFileProvider(path))));
            });

            return services;
        }
    }
}
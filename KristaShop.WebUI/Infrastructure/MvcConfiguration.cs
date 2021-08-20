using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Processors;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using KristaShop.Common.Models;
using KristaShop.WebUI.Infrastructure.ImageSharpWebP;
using KristaShop.WebUI.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Web.Providers;
using SmartBreadcrumbs.Extensions;
using System.Runtime.InteropServices;
using KristaShop.Common.Attributes;
using KristaShop.WebUI.Infrastructure.Logging;
using Microsoft.Extensions.Hosting;
using Module.App.WebUI.Controllers;
using Module.Cart.WebUI.Jobs;
using Module.Common.Admin.Admin.Filters;
using Module.Common.WebUI.Jobs.Core;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace KristaShop.WebUI.Infrastructure
{
    public static class MvcConfiguration
    {
        public static IServiceCollection AddMvcConfiguration(this IServiceCollection services, IWebHostEnvironment environment,
            IConfiguration configuration) {
            AddImageSharp(services, configuration);

            services.AddMemoryCache();

            services.ConfigureNonBreakingSameSiteCookies();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(GlobalConstant.Session.BackendScheme, options => {
                    options.Cookie.Name = "Back.App";
                    options.LoginPath = "/Admin/Account/Login/";
                    options.AccessDeniedPath = "/Admin/Identity/Login/";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                })
                .AddCookie(GlobalConstant.Session.FrontendScheme, options => {
                    options.Cookie.Name = "Front.App";
                    options.LoginPath = "/Home/Index/showLoginForm";
                    options.AccessDeniedPath = "/Home/Index/showLoginForm";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                })
                .AddCookie(GlobalConstant.Session.FrontendGuestScheme, options => {
                     options.Cookie.Name = "FrontGuest.App";
                     options.LoginPath = "/Home/Index/showLoginForm";
                     options.AccessDeniedPath = "/Home/Index/showLoginForm";
                     options.ExpireTimeSpan = TimeSpan.FromHours(2);
                     options.SlidingExpiration = true;
                     options.Cookie.IsEssential = true;
                     options.Cookie.HttpOnly = true;
                     options.Cookie.SameSite = SameSiteMode.Strict;
                 });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();

            services.AddScoped<PermissionFilter>();
            services.AddTransient<UserCookieEnricher>();
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
            services.AddHttpContextAccessor();
            
            
            services.AddControllersWithViews()
                .AddApplicationModules(ModulesDeclaration.GetAssemblies())
                .AddRazorRuntimeCompilation();

            if (environment.IsDevelopment()) {
                services.ConfigureModulesRuntimeCompilation(environment.ContentRootPath, ModulesDeclaration.GetAssemblies());
            }
            
            ModulesDeclaration.ConfigureServices(services);
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddBreadcrumbs(Assembly.GetAssembly(typeof(HomeController)), options => {
                options.TagName = "nav";
                options.OlClasses = "breadcrumb";
                options.LiClasses = "breadcrumb-item";
                options.ActiveLiClasses = "breadcrumb-item active";
                options.InferFromAction = true;
                options.DefaultAction = "Index";
                options.FallbackTitleToMethodName = true;
                options.AriaLabel = "breadcrumbs";
                options.ResourceType = null;
                options.LiTemplate = "<li><a href=\"{1}\" class=\"link-base link-grey\">{0}</a></li>";
                options.SeparatorElement = "<li class=\"separator text-main-grey\"><span class=\"text-main-grey mx-1\">/</span></li>";
            });

            return services;
        }

        private static void AddImageSharp(IServiceCollection services, IConfiguration configuration) {
            var cacheFolder =
                configuration[$"{nameof(GlobalSettings)}:{nameof(GlobalSettings.FilesDirectoryPath)}"] +
                configuration[$"{nameof(GlobalSettings)}:{nameof(GlobalSettings.GalleryCache)}"];
            
            services.AddImageSharp(
                    options => {
                        options.Configuration = Configuration.Default;
                        options.MemoryStreamManager = new RecyclableMemoryStreamManager();
                        options.BrowserMaxAge = TimeSpan.FromDays(30);
                        options.CacheMaxAge = TimeSpan.FromDays(30);
                        options.CachedNameLength = 8;
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                            options.Configuration = Configuration.Default;
                        } else {
                            options.Configuration = new Configuration(
                               new PngConfigurationModule(),
                               new JpegConfigurationModule(),
                               new GifConfigurationModule(),
                               new BmpConfigurationModule(),
                               new TgaConfigurationModule(),
                               new WebPConfigurationModule());
                        }
                        options.OnParseCommandsAsync += context => {
                            if (context.Commands.Count == 0) {
                                return Task.CompletedTask;
                            }
                            
                            PreventToInterceptFormatRssQueries(context);
                            return Task.CompletedTask;
                        };
                    })
                .SetRequestParser<QueryCollectionRequestParser>()
                .Configure<PhysicalFileSystemCacheOptions>(options => { options.CacheFolder = cacheFolder; })
                .SetCache(provider => new PhysicalFileSystemCache(
                    provider.GetRequiredService<IOptions<PhysicalFileSystemCacheOptions>>(),
                    provider.GetRequiredService<IWebHostEnvironment>(),
                    provider.GetRequiredService<IOptions<ImageSharpMiddlewareOptions>>(),
                    provider.GetRequiredService<FormatUtilities>()))
                .SetCacheHash<CacheHash>()
                .RemoveProvider<PhysicalFileSystemProvider>()
                .AddProvider<CustomPhysicalFileSystemProvider>()
                .AddProcessor<ResizeWebProcessor>()
                .AddProcessor<FormatWebProcessor>()
                .AddProcessor<BackgroundColorWebProcessor>();

            static void PreventToInterceptFormatRssQueries(ImageCommandContext context) {
                var format = context.Commands.GetValueOrDefault(FormatWebProcessor.Format);
                if (!string.IsNullOrEmpty(format) && format.Equals("rss")) {
                    context.Commands.Remove(FormatWebProcessor.Format);
                }
            }
        }

        public static void AddBackgroundJobs(this IServiceCollection services) {
            services.AddHostedService<QuartzHostedService>();
            
            // Quartz services
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            
            //Custom jobs registration
            services.AddSingleton<CleanOldCartsJob>();
            services.AddSingleton(new JobSchedule(typeof(CleanOldCartsJob), "0 0 5 ? * * *")); // Run every day at 5:00
        }

        public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration) {
            try {
                var dataProtectionKeysPath = configuration["DataProtection:KeysDirectory"];
                var certificatePath = configuration["DataProtection:CertificatePath"];
                var passwordEnvVariable = configuration["DataProtection:EnvironmentVariableWithCertificatePassword"];

                var password = Environment.GetEnvironmentVariable(passwordEnvVariable);

                if (File.Exists(certificatePath)) {
                    services.AddDataProtection()
                        .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath))
                        .ProtectKeysWithCertificate(new X509Certificate2(certificatePath, password))
                        .SetDefaultKeyLifetime(TimeSpan.FromDays(30))
                        .SetApplicationName("KristaShop.WebUI");
                } else {
                    Console.WriteLine("Data protection certificate not found. Failed to add DataProtection");
                }
            } catch (Exception ex) {
                Console.WriteLine($"Failed to add DataProtection. {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
using KristaShop.Common.Models;
using KristaShop.WebUI.Infrastructure;
using KristaShop.WebUI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp.Web.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.FeatureManagement;
using Module.Common.Business;
using Module.Media.WebUI.Infrastructure;

namespace KristaShop.WebUI {
    public class Startup {
        private const string DefaultCulture = "ru-RU";
        private static CultureInfo _defaultCultureInfo = new(DefaultCulture);
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public static ILoggerFactory DebugConsole { get; private set; } = null;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.LoginPath = new PathString("/authorization/login");
                    options.AccessDeniedPath = new PathString("/authorization/denied");
                });
            if (Environment.IsDevelopment()) {
                DebugConsole = LoggerFactory.Create(builder => { builder.AddDebug(); });
            }

            services.Configure<RazorViewEngineOptions>(x => x.ViewLocationExpanders.Add(new ModulesViewLocationExpander()));
            services.AddFeatureManagement(Configuration.GetSection("FeatureFlags"));
            services.AddDbContext(Configuration, DebugConsole);
            services.Configure<UrlSetting>(Configuration.GetSection("UrlSetting"));
            services.Configure<GlobalSettings>(Configuration.GetSection("GlobalSettings"));
            services.Configure<TextColors>(Configuration.GetSection("TextColors"));
            services.Configure<EmailsSetting>(Configuration.GetSection("EmailsSetting"));
            services.AddScoped<DynamicPagesRouteValueTransformer>();
            services.AddMvcConfiguration(Environment, Configuration);
            services.AddBackgroundJobs();
            if (!Environment.IsDevelopment()) {
                services.AddDataProtection(Configuration);
            }
        }

        public void Configure(IApplicationBuilder app, IServiceScopeFactory serviceScopeFactory) {
            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            if (Environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/error/common500");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseImageSharp(); // should be before UseStaticFiles
            
            app.UseStaticFiles(new StaticFileOptions {
                OnPrepareResponse = ctx => {
                    const int durationInSeconds = 60 * 60 * 24 * 7;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
                }
            });

            app.UseCookiePolicy();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseRouting();
            app.UseRequestLocalization(new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture(_defaultCultureInfo),
                SupportedCultures = new List<CultureInfo> {_defaultCultureInfo},
                SupportedUICultures = new List<CultureInfo> {_defaultCultureInfo}
            });
            app.UseMiddleware<LinkBasedAuthMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "Areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
                endpoints.MapDynamicControllerRoute<DynamicPagesRouteValueTransformer>(
                    "{controller=Home}/{action=Index}");
            });
            app.InitializeApp(serviceScopeFactory);
        }
    }
}
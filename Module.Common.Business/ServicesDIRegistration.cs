using System;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Module.Common.Business.Interfaces;
using Module.Common.Business.Services;
using Module.Common.Business.UnitOfWork;

namespace Module.Common.Business {
    public static class ServicesDIRegistration {
        public static void AddServices(this IServiceCollection services) {
            services.AddScoped<ICommonUnitOfWork, CommonUnitOfWork>();

            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IRootFileService, RootFilesService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddSingleton<ISettingsManager, SettingsManager<AppSettings>>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ILookupsService, LookupsService>();
            services.AddScoped<ICollectionService, CollectionService>();
        }
        
        public static IServiceCollection AddDbContext(this IServiceCollection services,
            IConfiguration configuration, ILoggerFactory logger) {

            /*Данная конфигурация используется для MYSQL*/
            services.AddDbContextPool<KristaShopDbContext>(options => {
                if (logger != null) {
                    options.UseLoggerFactory(logger);
                }

                options.UseMySql(configuration.GetConnectionString("KristaShopMysql"),
                    new MySqlServerVersion(new Version(8, 0, 18)));
                options.ReplaceService<IMigrationsModelDiffer, MigrationsModelDifferWithoutForeignKey>();
            });

            return services;
        }
    }
}
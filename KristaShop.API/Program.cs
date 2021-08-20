using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using Serilog;
using Serilog.Events;

namespace KristaShop.API {
    public class Program {
        public static int Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
               .WriteTo.Console()
               .CreateLogger();

            try {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            } catch (Exception ex) {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => {
                    config.Sources.Clear();

                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", true, true);
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

                    if (hostingContext.HostingEnvironment.IsDevelopment()) {
                        config.AddUserSecrets<Program>();
                    }

                    config.AddEnvironmentVariables();

                    if (args != null) {
                        config.AddCommandLine(args);
                    }
                })
                .UseSerilog((hostingContext, loggerConfiguration) => { CreateSerilogLoggerConfiguration(loggerConfiguration, hostingContext); })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.ConfigureKestrel(serverOptions => {
                        serverOptions.Limits.MaxConcurrentConnections = 1000;
                        serverOptions.Limits.MaxConcurrentUpgradedConnections = 1000;
                        serverOptions.Limits.MaxRequestBodySize = 31457280;
                    })
                    .UseStartup<Startup>();
                });

        private static void CreateSerilogLoggerConfiguration(LoggerConfiguration loggerConfiguration, HostBuilderContext hostingContext) {
            loggerConfiguration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Default", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("SixLabors", LogEventLevel.Fatal)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", "KristaShop.API")
                .WriteTo.Logger(x => x.WriteTo.Console()
                    .Filter.ByIncludingOnly(c => c.Level == LogEventLevel.Information || c.Level == LogEventLevel.Debug))
                .WriteTo.Logger(x => x.WriteTo.MySQL(hostingContext.Configuration.GetConnectionString("KristaShopMysql"), "logs_errors")
                    .Filter.ByIncludingOnly(c => c.Level == LogEventLevel.Warning || c.Level == LogEventLevel.Error || c.Level == LogEventLevel.Fatal))
                .WriteTo.Logger(x => x.WriteTo.MySQL(hostingContext.Configuration.GetConnectionString("KristaShopMysql"), "logs_info")
                    .Filter.ByIncludingOnly(c => c.Level == LogEventLevel.Information || c.Level == LogEventLevel.Debug));

            // Write logs to file
            var filepath = hostingContext.Configuration["Logger:Filepath"];
            if (!string.IsNullOrEmpty(filepath)) {
                loggerConfiguration
                    .WriteTo.Logger(x => x.WriteTo.File(path: filepath, rollingInterval: RollingInterval.Month, rollOnFileSizeLimit: true)
                        .Filter.ByIncludingOnly(c => c.Level == LogEventLevel.Warning || c.Level == LogEventLevel.Error || c.Level == LogEventLevel.Fatal));
            }

            // Write logs to telegram
            var botToken = hostingContext.Configuration["Logger:Telegram:BotToken"];
            var chatId = hostingContext.Configuration["Logger:Telegram:ChatId"];
            if (!string.IsNullOrEmpty(botToken) && !string.IsNullOrEmpty(chatId)) {
                loggerConfiguration
                    .WriteTo.Logger(x => x.WriteTo.Telegram(botToken, chatId)
                        .Filter.ByIncludingOnly(c => c.Level == LogEventLevel.Warning || c.Level == LogEventLevel.Error || c.Level == LogEventLevel.Fatal));
            }
        }
    }
}

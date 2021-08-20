using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using KristaShop.API.Сonfiguration;
using Module.Common.Business;
using Module.Common.Business.Interfaces;
using Module.Common.Business.Services;

namespace KristaShop.API {
    public class Startup {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public static ILoggerFactory DebugConsole { get; private set; } = null;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            if (Environment.IsDevelopment()) {
                DebugConsole = LoggerFactory.Create(builder => { builder.AddDebug(); });
            }
            services.AddControllers();
            services.AddDbContext(Configuration, DebugConsole);
            services.AddServices();
            Module.Client.Business.ServicesDIRegistration.AddServices(services);
            Module.Partners.Business.ServicesDIRegistration.AddServices(services);
            services.Configure<UrlSetting>(Configuration.GetSection("UrlSetting"));
            services.Configure<GlobalSettings>(Configuration.GetSection("GlobalSettings"));
            services.Configure<TextColors>(Configuration.GetSection("TextColors"));
            services.Configure<EmailsSetting>(Configuration.GetSection("EmailsSetting"));
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "KristaShop.API", Version = "v1"});
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "KristaShop.API.xml"));
                
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer token_string')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
           
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            // within this section we are configuring the authentication and setting the default scheme
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                
            .AddJwtBearer(jwt => {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            //////////////////////////
            var cacheFolder =
       Configuration[$"{nameof(GlobalSettings)}:{nameof(GlobalSettings.FilesDirectoryPath)}"] +
       Configuration[$"{nameof(GlobalSettings)}:{nameof(GlobalSettings.GalleryCache)}"];

            

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMemoryCache();

          

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options => {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });
            
            
            //services.AddScoped<IEmailService, EmailService>();
            //services.AddScoped<IFileService, FileService>();
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
            services.AddHttpContextAccessor();
            services.AddMvc();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory serviceScopeFactory) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsProduction()) {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
                });

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "KristaShop.API v1");
                    c.RoutePrefix = "api/swagger";
                });
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });             
            
        }
    }
}

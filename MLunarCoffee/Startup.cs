using System;
using System.Globalization;
using System.IO;
using System.Text;
using AspNetCoreRateLimit;
using LazZiya.ExpressLocalization;
using LibRCL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.ScheduledTask;
using MLunarCoffee.Comon.SignalR;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.LocalizationResources;
using MLunarCoffee.Models;
using MLunarCoffee.Service.Quartz;
using WebMarkupMin.AspNetCore5;
using static System.Net.Mime.MediaTypeNames;

namespace MLunarCoffee
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env ,IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;

        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false).AddNewtonsoftJson();
            //var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddResponseCaching();
            #region//Daily Job
            services.AddHostedService<QuartzHostedService>();
            services.AddSingleton<IJobFactory ,SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory ,StdSchedulerFactory>();
            services.AddSingleton<JobReminder>();
            services.AddSingleton(new DailyJob(type: typeof(JobReminder)));
            #endregion
            services.AddTransient<ITokenService ,Comon.Services.TokenService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true ,
                    ValidateAudience = true ,
                    ValidateLifetime = true ,
                    ValidateIssuerSigningKey = true ,
                    ValidIssuer = Configuration["Jwt:Issuer"] ,
                    ValidAudience = Configuration["Jwt:Issuer"] ,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            #region // Middleware Authen JWT
            //services.AddAuthentication (x =>
            // {
            //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // })
            //.AddJwtBearer (x =>
            // {
            //     x.RequireHttpsMetadata = false;
            //     x.SaveToken = true;
            //     x.TokenValidationParameters = new TokenValidationParameters {
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey = new SymmetricSecurityKey (key),
            //         ValidateIssuer = false,
            //         ValidateAudience = false
            //     };

            // });
            #endregion
            //services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            #region // LimitRate
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration ,RateLimitConfiguration>();
            services.AddSingleton<IIpPolicyStore ,DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore ,DistributedCacheRateLimitCounterStore>();
            #endregion
            if (bool.Parse(Configuration["IsProductEnviroment"]))
            {
                services.AddResponseCompression(options =>
               {
                   options.EnableForHttps = true;
                   options.MimeTypes = new[] { "text/plain",
                        "text/css",
                        "application/javascript",
                        "text/html",
                        "application/xml",
                        "text/xml",
                        "application/json",
                        "text/json"};
               });
                services.AddWebMarkupMin(options =>
                {
                    options.AllowMinificationInDevelopmentEnvironment = true;
                    options.AllowCompressionInDevelopmentEnvironment = true;
                    options.DisableMinification = false;
                    options.DisableCompression = false;
                    options.DisablePoweredByHttpHeaders = true;
                }).AddHtmlMinification(options =>
                {
                    options.MinificationSettings.RemoveHttpProtocolFromAttributes = false;
                    options.MinificationSettings.RemoveHttpsProtocolFromAttributes = false;
                    options.MinificationSettings.WhitespaceMinificationMode = WebMarkupMin.Core.WhitespaceMinificationMode.Safe;
                    options.MinificationSettings.RemoveRedundantAttributes = false;
                    options.MinificationSettings.RemoveTagsWithoutContent = false;
                    options.MinificationSettings.PreserveCase = true;
                    options.MinificationSettings.RemoveEmptyAttributes = false;
                    options.MinificationSettings.RemoveOptionalEndTags = false;
                    options.MinificationSettings.MinifyEmbeddedCssCode = true;
                    options.MinificationSettings.MinifyInlineCssCode = true;
                    options.MinificationSettings.MinifyEmbeddedJsonData = true;
                    options.MinificationSettings.EmptyTagRenderMode = WebMarkupMin.Core.HtmlEmptyTagRenderMode.Slash;

                }).AddHttpCompression();
            }
            services.AddDistributedMemoryCache();
            services.AddOutputCaching();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy" ,
                   builder => builder
                      .SetIsOriginAllowedToAllowWildcardSubdomains()
                      //.WithOrigins("https://*.vttechsolution.com")
                      .WithOrigins("https://cdnvttimg.vttechsolution.com")
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .AllowAnyHeader()
                      .Build()
                   );
            }
            );
            services.AddEditor();
            services.AddRazorPages().AddRazorPagesOptions(options => { }).AddNewtonsoftJson().AddMvcOptions(options =>
            {
                options.Filters.Add(new AsyncPageFilter(Configuration));
            });
            services.AddScoped<AuthorizeIPAddressAttribute>(container =>
            {
                return new AuthorizeIPAddressAttribute();
            });
            services.AddHostedService<TimedHostedService>();
            services.AddSignalR();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            var cultures = new[]
            {
                new CultureInfo("vi"),
                new CultureInfo("en"),
            };
            var culturesfor = new[]
           {

                new CultureInfo("en")
            };
            services.AddRazorPages()
                .AddExpressLocalization<ExpressLocalizationResource ,ViewLocalizationResource>(
                ops =>
                {
                    ops.ResourcesPath = "LocalizationResources";
                    ops.RequestLocalizationOptions = o =>
                    {
                        o.SupportedCultures = culturesfor;
                        o.SupportedUICultures = cultures;
                        o.DefaultRequestCulture = new RequestCulture("en");
                    };
                });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });

        }
        public void Configure(IApplicationBuilder app ,IWebHostEnvironment env)
        {
            if (bool.Parse(Configuration["IsProductEnviroment"]))
            {
                app.UseResponseCompression();
                app.UseWebMarkupMin();
            }

            app.UseIpRateLimiting();
            app.UseSession();
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseExceptionHandler(exceptionHandlerApp =>
                {
                    exceptionHandlerApp.Run(async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        // using static System.Net.Mime.MediaTypeNames;
                        context.Response.ContentType = Text.Plain;

                        await context.Response.WriteAsync("An exception was thrown.");

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                        {
                            await context.Response.WriteAsync(" The file was not found.");
                        }

                        if (exceptionHandlerPathFeature?.Path == "/")
                        {
                            await context.Response.WriteAsync(" Page: Home.");
                        }
                    });
                });
                app.UseHsts();
            }
            //app.UseWebOptimizer ();
            app.UseResponseCaching();
            app.UseOutputCaching();
            app.UseCookiePolicy();
            app.UseDefaultFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseOnlineUsers();
            app.UseRouting();
            app.UseCors("MyCorsPolicy");
            //app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default1" ,
                   pattern: "{culture}/{controller}/{action}/{id?}" ,
                   defaults: new { culture = "vi" ,controller = "Home" ,action = "Index" }
                 );
                endpoints.MapControllerRoute(
                   name: "default" ,
                   pattern: "{controller}/{action}/{id?}" ,
                   defaults: new { controller = "Home" ,action = "Index" }
                 );
                endpoints.MapControllerRoute(
                  name: "api" ,
                  pattern: "api/{controller}/{action}/{id?}" ,
                  defaults: new { controller = "Home" ,action = "Index" }
                );
                endpoints.MapRazorPages();
                endpoints.MapHub<NotiHub>("/notiHub");
            });

            Initialize_Application();
        }
        private async void Initialize_Application()
        {
            var KEYCODE = Configuration.GetValue<string>("DATA:KEYCODE");
            if (KEYCODE != null)
            {
                await Global.System_Start(KEYCODE.ToString() ,Configuration);
            }
        }
    }
}

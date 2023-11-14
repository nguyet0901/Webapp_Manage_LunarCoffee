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
//using MLunarCoffee.Comon.ScheduledTask;
//using MLunarCoffee.Comon.SignalR;
//using MLunarCoffee.Comon.TokenJWT;
//using MLunarCoffee.LocalizationResources;
//using MLunarCoffee.Models;
//using MLunarCoffee.Service.Quartz;
using WebMarkupMin.AspNetCore5;
using static System.Net.Mime.MediaTypeNames;
using MLunarCoffee.Models;
using static MLunarCoffee.Comon.GlobalUser;

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
            services.AddResponseCaching();
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
            services.AddDistributedMemoryCache();
            services.AddOutputCaching();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddEditor();
            services.AddRazorPages().AddRazorPagesOptions(options => { }).AddNewtonsoftJson().AddMvcOptions(options =>
            {
                options.Filters.Add(new AsyncPageFilter(Configuration));
            });
            services.AddSignalR();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });

        }
        public void Configure(IApplicationBuilder app ,IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            app.UseResponseCaching();
            app.UseOutputCaching();
            app.UseCookiePolicy();
            app.UseDefaultFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
  
            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
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
                //endpoints.MapHub<NotiHub>("/notiHub");
            });

            InitApplication();
        }
        private async void InitApplication()
        {
            await Global.System_Start(Configuration);
        }
    }
}

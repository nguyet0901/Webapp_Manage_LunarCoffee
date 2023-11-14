using CoffeePage.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibRCL;
using CoffeePage.Comon;

namespace CoffeePage
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
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
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller}/{action}/{id?}",
                   defaults: new { controller = "Home", action = "Index" }
                 );
                endpoints.MapControllerRoute(
                  name: "api",
                  pattern: "api/{controller}/{action}/{id?}",
                  defaults: new { controller = "Home", action = "Index" }
                );
                endpoints.MapRazorPages();
                //endpoints.MapHub<NotiHub>("/notiHub");
            });

            //InitApplication();
        }
        //private async void InitApplication()
        //{
        //    //await Global.InitSystem(Configuration);
        //}
    }
}

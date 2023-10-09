using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using System.Configuration;
using System.Drawing;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var app = builder.Build();
IConfiguration configuration = app.Configuration;
IWebHostEnvironment environment = app.Environment;

// Add services to the container.
builder.Services.AddRazorPages();

//services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false).AddNewtonsoftJson();
//var key = Encoding.ASCII.GetBytes(Settings.Secret);
services.AddResponseCaching();
#region//Daily Job
//services.AddHostedService<QuartzHostedService>();
//services.AddSingleton<IJobFactory, SingletonJobFactory>();
//services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
//services.AddSingleton<JobReminder>();
//services.AddSingleton(new DailyJob(type: typeof(JobReminder)));
#endregion
//services.AddTransient<ITokenService, Comon.Services.TokenService>();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = Configuration["Jwt:Issuer"],
        //ValidAudience = Configuration["Jwt:Issuer"],
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
    };
});





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

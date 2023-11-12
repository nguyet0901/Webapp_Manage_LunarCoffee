using CoffeePage;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {

                webBuilder.UseStartup<Startup>();
            });
}

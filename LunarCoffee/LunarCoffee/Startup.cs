using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LunarCoffee.Startup))]
namespace LunarCoffee
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace LibRCL
{

    public static class LibRCLServiceCollectionExtensions
    {
        public static void AddEditor(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(LibRCLConfigureOptions));
        }
    }
}

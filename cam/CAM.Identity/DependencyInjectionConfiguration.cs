using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CAM.Identity
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            return services;
        }

        public static IServiceCollection AddIdentityServiceMock(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IIdentityService, IdentityServiceMock>();
            return services;
        }
    }
}

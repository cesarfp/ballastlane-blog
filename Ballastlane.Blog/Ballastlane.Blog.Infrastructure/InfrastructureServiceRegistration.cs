using Ballastlane.Blog.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ballastlane.Blog.Infrastructure
{
    /// <summary>
    /// This main class used to configure the infrastructure services and invoked by <see cref="Program"/>.
    /// </summary>
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);
            services.AddSwaggerDocumentation();
            services.AddUserContextService();

            return services;
        }
    }
}

using Ballastlane.Blog.Application.Contracts.Infraestructure;
using Ballastlane.Blog.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ballastlane.Blog.Infrastructure.Extensions
{
    /// <summary>
    /// This class is used to configure the UserContextService and invoked by <see cref="InfrastructureServiceRegistration"/> class.
    /// </summary>
    internal static class UserContextServiceExtensions
    {
        public static IServiceCollection AddUserContextService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContextService, UserContextService>();

            return services;
        }
    }
}

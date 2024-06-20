using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ballastlane.Blog.Infrastructure.Extensions
{
    public static class UserContextServiceExtensions
    {
        public static IServiceCollection AddUserContextService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContextService, UserContextService>();

            return services;
        }
    }
}

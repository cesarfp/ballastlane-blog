using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ballastlane.Blog.Application.Extensions
{
    public static class ApplicationConfigurationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}

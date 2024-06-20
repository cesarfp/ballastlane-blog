using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Infrastructure.Extensions;
using Ballastlane.Blog.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ballastlane.Blog.Infrastructure
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);
            services.AddSwaggerDocumentation();
            services.AddUserContextService();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddTransient<IPostRepository, PostRepository>(provider =>
                new PostRepository(connectionString!));

            services.AddTransient<IUserRepository, UserRepository>(provider =>
                new UserRepository(connectionString!));

            return services;
        }
    }
}

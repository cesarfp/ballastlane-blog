using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ballastlane.Blog.Infrastructure.Extensions
{
    /// <summary>
    /// This main class used to configure the infrastructure services and invoked by <see cref="Program"/>.
    /// </summary>
    public static class InfrastructureConfigurationExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);
            services.AddSwaggerDocumentation();
            services.AddUserContextService();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IPostRepository, PostRepository>(provider =>
                new PostRepository(connectionString!));

            services.AddScoped<IUserRepository, UserRepository>(provider =>
                new UserRepository(connectionString!));

            return services;
        }
    }
}

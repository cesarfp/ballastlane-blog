using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Infraestructure.Extensions;
using Ballastlane.Blog.Infraestructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ballastlane.Blog.Infraestructure
{
    public static class InfraestructureServiceExtensions
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

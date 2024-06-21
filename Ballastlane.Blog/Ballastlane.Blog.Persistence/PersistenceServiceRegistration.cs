using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ballastlane.Blog.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IPostRepository, PostRepository>(provider =>
                    new PostRepository(connectionString!));

            services.AddScoped<IUserRepository, UserRepository>(provider =>
                new UserRepository(connectionString!));

            return services;
        }
    }
}

using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Infraestructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.Blog.Infraestructure
{
    public static class InfraestructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IPostRepository, PostRepository>(provider =>
                new PostRepository(connectionString));

            return services;
        }
    }
}

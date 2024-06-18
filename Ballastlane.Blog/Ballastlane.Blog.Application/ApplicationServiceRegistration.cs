using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Services;

namespace Ballastlane.Blog.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Application layer services, e.g.:
            services.AddTransient<IPostService, PostService>();

            return services;
        }
    }
}

using Ballastlane.Blog.Application.Contracts.Infraestructure;
using Ballastlane.Blog.Infrastructure.Options;
using Ballastlane.Blog.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ballastlane.Blog.Infrastructure.Extensions
{
    /// <summary>
    /// This class is used to configure the JWT authentication to the application and invoked by the <see cref="InfrastructureServiceRegistration"/> class.
    /// </summary>
    internal static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

            services.AddSingleton<IJwtGeneratorService, JwtGeneratorService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var jwtConfig = serviceProvider.GetRequiredService<IOptionsSnapshot<JwtConfig>>().Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}

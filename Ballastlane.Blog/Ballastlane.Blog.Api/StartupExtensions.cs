using Ballastlane.Blog.Application;
using Ballastlane.Blog.Infrastructure;
using Ballastlane.Blog.Persistence;

namespace Ballastlane.Blog.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices( this WebApplicationBuilder builder)
        {

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline( this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ballast Lane Blog API v1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}

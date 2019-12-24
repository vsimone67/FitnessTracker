using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Common.Swagger
{
    public static class SwaggerExtention
    {
        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
        {
            SwaggerOptions options;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<SwaggerOptions>(configuration.GetSection("swagger"));
                options = configuration.GetOptions<SwaggerOptions>("swagger");
            }

            if (options.Enabled)
            {
                services.AddSwaggerDocument(config =>
                {
                    config.PostProcess = document =>
                    {
                        document.Info.Version = options.Version;
                        document.Info.Title = options.Title;
                    };
                });
            }

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
        {
            var options = builder.ApplicationServices.GetService<IConfiguration>()
                                 .GetOptions<SwaggerOptions>("swagger");

            if (options.Enabled)
            {
                builder.UseOpenApi();
                builder.UseSwaggerUi3();
            }

            return builder;
        }
    }
}
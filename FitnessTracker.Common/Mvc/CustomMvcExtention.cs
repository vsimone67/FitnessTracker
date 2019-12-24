using FitnessTracker.Common.Filters;
using FitnessTracker.Common.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FitnessTracker.Common.Mvc
{
    public static class CustomMvcExtention
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddControllers();

            MetricsOptions metricOptions;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<MetricsOptions>(configuration.GetSection("metrics"));
                metricOptions = configuration.GetOptions<MetricsOptions>("metrics");
            }

            if (!metricOptions.Enabled)
            {
                services.AddMvc(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
                                                  .AddNewtonsoftJson(opt => opt.SerializerSettings.ContractResolver = new DefaultContractResolver())
                                                  .AddControllersAsServices();  //Injecting Controllers themselves thru DI
            }
            else
            {
                services.AddMvc(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
                                                  .AddNewtonsoftJson(opt => opt.SerializerSettings.ContractResolver = new DefaultContractResolver())
                                                  .AddControllersAsServices()  //Injecting Controllers themselves thru DI
                                                  .AddMetrics();
            }

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors();
            return services;
        }

        public static IApplicationBuilder UseCustomMvc(this IApplicationBuilder builder)
        {
            builder.UseRouting();
            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return builder;
        }

        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder builder)
        {
            builder.UseCors(cors => cors
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());

            return builder;
        }
    }
}
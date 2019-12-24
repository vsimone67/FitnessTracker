using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Common.MassTransit
{
    public static class MassTransitExtention
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services)
        {
            services.AddSingleton<IServiceBus>(sp =>
            {
                ServiceBusSettings serviceBusSettings;
                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var configuration = serviceProvider.GetService<IConfiguration>();
                    services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
                    serviceBusSettings = configuration.GetOptions<ServiceBusSettings>("ServiceBusSettings");
                }
                var serviceBus = new ServiceBusRabbitMq(serviceBusSettings);
                return serviceBus;
            });
            return services;
        }

        public static IApplicationBuilder UseMassTransit(this IApplicationBuilder builder)
        {
            var serviceBus = builder.ApplicationServices.GetService<IServiceBus>();

            serviceBus.Connect();

            return builder;
        }
    }
}
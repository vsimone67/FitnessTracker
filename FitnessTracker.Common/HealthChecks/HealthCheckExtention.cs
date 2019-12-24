using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Common.HealthChecks
{
    public static class HealthCheckExtention
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, string connectionString = "")
        {
            if (!string.IsNullOrEmpty(connectionString))
                services.AddHealthChecks().AddSqlServer(connectionString);
            else
                services.AddHealthChecks();
            return services;
        }

        public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder builder)
        {
            builder.UseHealthChecks("/health");

            return builder;
        }
    }
}
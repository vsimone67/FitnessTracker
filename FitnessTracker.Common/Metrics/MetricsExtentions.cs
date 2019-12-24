using App.Metrics;
using App.Metrics.Formatters.InfluxDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FitnessTracker.Common.Metrics
{
    public static class MetricExtensions
    {
        public static IServiceCollection AddAppMetrics(this IServiceCollection services)
        {
            var metricOptions = GetMetricOptions(services);

            if (metricOptions.Enabled) // true is enable metrics
            {
                var metrics = AppMetrics.CreateDefaultBuilder()
                .Configuration.Configure(options =>
                {
                    options.GlobalTags.Add("svrenv", metricOptions.ServerEnvironment);
                    options.GlobalTags.Add("applicationsuite", metricOptions.ApplicationSuite);
                })
                .Report.ToInfluxDb(options =>
                {
                    options.InfluxDb.BaseUri = new Uri(metricOptions.InfluxDbUrl);
                    options.InfluxDb.Database = metricOptions.InfluxDbName;
                    options.InfluxDb.CreateDataBaseIfNotExists = true;
                    options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                    options.HttpPolicy.Timeout = TimeSpan.FromSeconds(metricOptions.HttpPolicyTimeOut);
                    options.FlushInterval = TimeSpan.FromSeconds(metricOptions.FlushInterval);
                })
                .Build();

                services.AddMetrics(metrics);
                services.AddMetricsTrackingMiddleware();
                services.AddMetricsReportingHostedService();
                services.AddSingleton<IHostedService, MetricsReporter>();
            }
            return services;
        }

        public static IApplicationBuilder UseAppMetrics(this IApplicationBuilder app)
        {
            var metricOptions = app.ApplicationServices.GetService<IConfiguration>()
               .GetOptions<MetricsOptions>("metrics");

            if (metricOptions.Enabled) // true is enable metrics
            {
                app.UseMetricsAllMiddleware();
            }
            return app;
        }

        public static IServiceCollection UseAppMetricsMvc(this IServiceCollection services)
        {
            var metricOptions = GetMetricOptions(services);

            if (metricOptions.Enabled) // true is enable metrics
            {
                services.AddMvc().AddMetrics();
            }

            return services;
        }

        private static MetricsOptions GetMetricOptions(IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<MetricsOptions>(configuration.GetSection("metrics"));
                return configuration.GetOptions<MetricsOptions>("metrics");
            }
        }
    }
}
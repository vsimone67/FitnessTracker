using FitnessTracker.Common.ExtentionMethods;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace FitnessTracker.Common.Logging
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddSeriLog(this IServiceCollection services)
        {
            var env = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                                                             .AddEnvironmentVariables()
                                                             .Build(); // get variables from environment to pass to config (if exist)

            string basePath = env.GetValue<string>("appdirectory").NullToEmpty();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(basePath + "appsettings.json", optional: false)
                .Build();

            Log.Logger = new LoggerConfiguration()
                      .Enrich.FromLogContext()
                      .ReadFrom.Configuration(configuration)
                      .CreateLogger();

            AppDomain.CurrentDomain.DomainUnload += (o, e) => Log.CloseAndFlush();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });

            return services;
        }
    }
}
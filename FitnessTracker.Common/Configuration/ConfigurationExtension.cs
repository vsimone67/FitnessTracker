using FitnessTracker.Common.ExtentionMethods;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FitnessTracker.Common.Configuration
{
    public static class ConfigurationExtension
    {
        public static IHostBuilder AddAppConfiguration(this IHostBuilder builder, string basePath = "")
        {
            builder.ConfigureAppConfiguration((builderContext, config) =>
            {
                var env = builderContext.HostingEnvironment;

                if (!string.IsNullOrEmpty(basePath))
                    config.SetBasePath(basePath);  // set path to docker volume for common files or static sites

                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                config.AddEnvironmentVariables();
            });

            return builder;
        }

        public static IHostBuilder AddAppConfigurationFromEnvironment(this IHostBuilder builder)
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                                                               .AddEnvironmentVariables()
                                                               .Build(); // get variables from environment to pass to config (if exist)

            string basePath = config.GetValue<string>("appdirectory").NullToEmpty();

            return AddAppConfiguration(builder, basePath);
        }
    }
}
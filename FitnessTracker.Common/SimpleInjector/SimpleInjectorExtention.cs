using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace FitnessTracker.Common.SimpleInjector
{
    public static class SimpleInjectorExtention
    {
        public static IServiceCollection AddDiContainer(this IServiceCollection services, Container container)
        {
            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope.
                options.AddAspNetCore();
                options.AddLogging();
            });

            return services;
        }

        public static IApplicationBuilder UseDiContainer(this IApplicationBuilder builder, Container container)
        {
            // UseSimpleInjector() enables framework services to be injected into
            // application components, resolved by Simple Injector.
            builder.UseSimpleInjector(container, options =>
            {
                // Optionally, allow application components to depend on the
                // non-generic Microsoft.Extensions.Logging.ILogger abstraction.
                //options.UseLogging();
            });

            container.Verify();

            return builder;
        }
    }
}
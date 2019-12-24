using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Common.Mediator
{
    public static class MediatorExtention
    {
        public static IServiceCollection AddCommandQueryHandlers(this IServiceCollection services)
        {
            var fitnessTrackerAssemblies = LibraryManager.GetReferencingAssemblies("FitnessTracker");

            services.AddMediatR(fitnessTrackerAssemblies);  // we are using MetiatR as our mediator for the command/query handlers
            return services;
        }
    }
}
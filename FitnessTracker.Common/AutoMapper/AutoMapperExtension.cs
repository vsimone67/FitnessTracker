using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Common.AutoMapper
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(LibraryManager.GetReferencingAssemblies("FitnessTracker"));
            return services;
        }
    }
}
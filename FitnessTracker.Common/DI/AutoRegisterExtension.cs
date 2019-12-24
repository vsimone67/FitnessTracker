using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FitnessTracker.Common.DI
{
    public static class AutoRegisterExtension
    {
        public static IServiceCollection AddAutoRegisterDependencies(this IServiceCollection services)
        {
            var fitnessTrackerAssemblyDefinedTypes = LibraryManager.GetReferencingAssemblies("FitnessTracker").ToList().SelectMany(an => an.DefinedTypes);

            // Get types with AutoRegisterAttribute in assemblies
            var typesWithAutoRegisterAttribute =
                from t in fitnessTrackerAssemblyDefinedTypes
                let attributes = t.GetCustomAttributes(typeof(AutoRegisterAttribute), true)
                where attributes?.Length > 0
                select new { Type = t, Attributes = attributes.Cast<AutoRegisterAttribute>() };

            // Loop through types to register with IoC
            foreach (var typeToRegister in typesWithAutoRegisterAttribute)
            {
                foreach (var attribute in typeToRegister.Attributes)
                {
                    services.AddScoped(attribute.RegisterAsType, typeToRegister.Type.AsType());
                }
            }

            return services;
        }
    }
}

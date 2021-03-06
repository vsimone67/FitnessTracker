﻿using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FitnessTracker.Common
{
    public static class LibraryManager
    {
        public static Assembly[] GetReferencingAssemblies(string assemblyName)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateLibrary(library, assemblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies.ToArray();
        }

        public static Assembly GetAssembly(string assemblyName)
        {
            var assemblies = GetReferencingAssemblies(assemblyName);
            var assembley = assemblies.FirstOrDefault(exp => exp.FullName.StartsWith(assemblyName));

            return assembley;
        }

        private static bool IsCandidateLibrary(RuntimeLibrary library, string assemblyName)
        {
            return library.Name == assemblyName
                || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName));
        }
    }
}
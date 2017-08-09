using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace App.Core.Utils
{
    public static class AutofacAssemblyStore
    {
        private static readonly ConcurrentDictionary<string, Assembly> Assemblies;

        static AutofacAssemblyStore()
        {
            Assemblies = new ConcurrentDictionary<string, Assembly>();
        }

        public static Assembly LoadAssembly(Assembly assembly)
        {
            return Assemblies.GetOrAdd(ExtractAssemblyShortName(assembly.FullName), shortName => assembly);
        }

        public static Assembly LoadAssembly(string assemblyName)
        {
            return Assemblies.GetOrAdd(ExtractAssemblyShortName(assemblyName), shortName => LoadWorker(shortName, assemblyName));
        }

        public static Assembly[] GetAssemblies()
        {
            return Assemblies.Values.ToArray();
        }

        public static string ExtractAssemblyShortName(string fullName)
        {
            var index = fullName.IndexOf(',');
            return index < 0 ? fullName : fullName.Substring(0, index);
        }

        public static void ClearAssemblies()
        {
            Assemblies.Clear();
        }

        private static Assembly LoadWorker(string shortName, string fullName)
        {
            Assembly result;

            // Try loading with full name first (if there is a full name)
            if (fullName != shortName)
            {
                result = TryAssemblyLoad(fullName);
                if (result != null) return result;
            }

            // Try loading with short name
            result = TryAssemblyLoad(shortName);
            if (result != null) return result;

            // Try again so that we get the exception this time
            return Assembly.Load(fullName);
        }

        private static Assembly TryAssemblyLoad(string name)
        {
            try
            {
                return Assembly.Load(name);
            }
            catch
            {
                return null;
            }
        }
    }
}
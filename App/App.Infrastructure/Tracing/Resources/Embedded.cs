using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace App.Infrastructure.Tracing.Resources
{
    public class EmbeddedResources
    {
        private static EmbeddedResources _callingResources;

        private static EmbeddedResources _entryResources;

        private static EmbeddedResources _executingResources;

        private readonly Assembly _assembly;

        private readonly string[] _resources;

        public static EmbeddedResources CallingResources => _callingResources ?? (_callingResources = new EmbeddedResources(Assembly.GetCallingAssembly()));

        public static EmbeddedResources EntryResources => _entryResources ?? (_entryResources = new EmbeddedResources(Assembly.GetEntryAssembly()));

        public static EmbeddedResources ExecutingResources => _executingResources ??
                                                              (_executingResources = new EmbeddedResources(Assembly.GetExecutingAssembly()));

        public static EmbeddedResources ThisResources => _executingResources ??
                                                         (_executingResources = new EmbeddedResources(Assembly.GetAssembly(typeof(EmbeddedResources))));

        public EmbeddedResources(Assembly assembly)
        {
            _assembly = assembly;
            _resources = assembly.GetManifestResourceNames();
        }

        public Stream GetStream(string resName)
        {
            var possibleCandidates = _resources.Where(s => s.Contains(resName)).ToArray();

            switch (possibleCandidates.Length)
            {
                case 0:
                    return null;
                case 1:
                    return _assembly.GetManifestResourceStream(possibleCandidates[0]);
            }

            throw new ArgumentException(@"Ambiguous name, cannot identify resource", nameof(resName));
        }
    }
}
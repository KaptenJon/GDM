using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace GDMCore
{
    /// <summary>
    /// Custom AssemblyLoadContext for loading plugins from a subdirectory.
    /// Shared types (e.g. GDMInterfaces) are resolved from the default context
    /// so that interface type identity is preserved across the host and plugins.
    /// Plugin-specific dependencies are resolved from the plugin directory.
    /// </summary>
    internal class PluginLoadContext : AssemblyLoadContext
    {
        private readonly string _pluginDirectory;

        public PluginLoadContext(string pluginDirectory) : base(isCollectible: false)
        {
            _pluginDirectory = pluginDirectory;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Try the default context first so shared types (GDMInterfaces, etc.)
            // use the same assembly instance as the host application.
            try
            {
                return Default.LoadFromAssemblyName(assemblyName);
            }
            catch
            {
                // Not found in default context – fall through to local resolution.
            }

            // Resolve from the plugin directory.
            string candidate = Path.Combine(_pluginDirectory, assemblyName.Name + ".dll");
            if (File.Exists(candidate))
            {
                return LoadFromAssemblyPath(candidate);
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using GDMInterfaces;

namespace GDMCore
{
    /// <summary>
    /// Instanciates the the plug-ins used by the program at construction. Holds three lists of plug-ins - Tool,, Input and Output. Both default internal plugins and external plugins are handled.
    /// </summary>

    public class PluginManager
    {
        private List<IInput> _inputPlugins = new List<IInput>();
        private List<IOutput> _outputPlugins = new List<IOutput>();
        private List<ITool> _toolPlugins = new List<ITool>();
        private static Type[] _pluginSettingsType;
        // Path to external plugins
        private string _path = "Plugin";
        private List<IPlugin> _specialPlugins = new List<IPlugin>();
        public static AppDomain PluginDomain { get; set; }
        private PluginLoadContext _pluginLoadContext;
        /// <summary>
        /// Instanciates all plugins
        /// </summary>
        /// <param name="path">The path to the plugin library</param>
        public PluginManager(string path)
        {
            _path = path;
            Internal();
            External();


             var pluglist = new List<Type>();

            foreach (IPlugin plug in _inputPlugins)
                pluglist.Add(plug.GetSettingsType());
            foreach (IPlugin plug in _outputPlugins)
                pluglist.Add(plug.GetSettingsType());
            foreach (IPlugin plug in _toolPlugins)
                pluglist.Add(plug.GetSettingsType());
            pluglist.Add(typeof(BatchPluginSettings));
            _pluginSettingsType = pluglist.ToArray();
            _specialPlugins.Add(new BatchPlugin(_pluginSettingsType));
        }

        
        public static Type[] PluginSettingsType => _pluginSettingsType;

        /// <summary>
        /// Instanciates internal (default) plugins
        /// </summary>
        private void Internal()
        {
            string strPath = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            //LoadPluginsFromDll(strPath + "\\GDMPlugins.dll");
            //LoadPluginsFromDll(strPath + "\\SortColumns.dll");
        }

        /// <summary>
        /// Instanciates external plugins
        /// </summary>
        private void External()
        {
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

            _pluginLoadContext = new PluginLoadContext(System.IO.Path.GetFullPath(_path));

            // Get all dll files in path
            foreach (string dll in Directory.GetFiles(_path, "*.dll"))
            {
                // Skip DLLs that are already loaded in the host (default) context.
                // These are shared assemblies (GDMInterfaces, GDMCore, MathNet, etc.)
                // and must NOT be loaded again into the plugin context, otherwise
                // type identity breaks (e.g. IInput from host != IInput from plugin copy).
                // They will be resolved on-demand via PluginLoadContext.Load() when
                // the actual plugin assembly references them.
                try
                {
                    var name = System.Reflection.AssemblyName.GetAssemblyName(dll);
                    System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyName(name);
                    continue; // Already in host — skip
                }
                catch
                {
                    // Not in host context — this is a plugin or plugin-only dependency
                }

                LoadPluginsFromDll(dll);
            }
        }

        private void LoadPluginsFromDll(string plugin)
        {
            try
            {
                var assembly = _pluginLoadContext.LoadFromAssemblyPath(System.IO.Path.GetFullPath(plugin));

                // Only scan assemblies that actually reference GDMInterfaces — they are the real plugins.
                // This avoids ReflectionTypeLoadException from dependency DLLs
                // (e.g. Microsoft.Data.SqlClient) that happen to be in the Plugin folder.
                bool referencesInterfaces = false;
                foreach (var refName in assembly.GetReferencedAssemblies())
                {
                    if (refName.Name == "GDMInterfaces")
                    {
                        referencesInterfaces = true;
                        break;
                    }
                }
                if (!referencesInterfaces)
                    return;

                // Get all classes in dll file
                try
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        // Instanciate class and put it in corresponding list 
                        foreach (Type iface in type.GetInterfaces())
                        {
                            if (iface.Equals(typeof(IInput)))
                            {
                                IInput instance = (IInput) Activator.CreateInstance(type);
                                _inputPlugins.Add(instance);
                            }
                            if (iface.Equals(typeof(IOutput)))
                            {
                                IOutput instance = (IOutput) Activator.CreateInstance(type);
                                _outputPlugins.Add(instance);
                            }
                            if (iface.Equals(typeof(ITool)))
                            {
                                ITool instance = (ITool) Activator.CreateInstance(type);
                                _toolPlugins.Add(instance);
                            }
                            _inputPlugins = InputPlugins.OrderBy(t => t.Name).ToList();
                            _outputPlugins = OutputPlugins.OrderBy(t => t.Name).ToList();
                            _toolPlugins = ToolPlugins.OrderBy(t => t.ToolCategory).ThenBy(t => t.Name).ToList();

                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// "LibraryPath" refers to the path of external plugins
        /// </summary>
        public string LibraryPath => _path;

        public List<IInput> InputPlugins => _inputPlugins;

        public List<IOutput> OutputPlugins => _outputPlugins;

        public List<ITool> ToolPlugins => _toolPlugins;

        public List<IPlugin> SpecialPlugins => _specialPlugins;

        public IEnumerable<IPlugin> AllPlugins => _inputPlugins.Cast<IPlugin>().Union(_outputPlugins).Union(_toolPlugins).Union(SpecialPlugins);

        public IPlugin GetPlugin(string name)
        {
            return AllPlugins.FirstOrDefault(t => t.Name == name);
        }
    }
}

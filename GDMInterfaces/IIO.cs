namespace GDMInterfaces
{
    /// <summary>
    /// Base interface for the Input and Output type of plug-ins.
    /// </summary>
    /// <remarks>A plugin will not be recognized by simply implementing this interface. Either IInput or IOutput must be used.</remarks>
    public interface IIo : IPlugin
    {
        /// <summary>
        /// Returns a string describing the Input or Output type. For example. In a configuration with multiple operations with the same plug-in and all exposeses a file path via the GetDynamicSettings method. Then the returned string from this method is used as a title and the user can thereby separate them by more then just the plug-in name
        /// </summary>
        string GetJobDescription(PluginSettings s);
        /// <summary>
        /// Returns an object that is a subset of the supplied settings object. A wrapper that only exposes properties that is not static to a configuration. For example. file paths to source files, database passwords etc.
        /// </summary>
        /// <returns>Returns an instance of a class that contain public properties that in turn set properties in the given instance of the plug-ins type of PluginSettings.</returns>
        object GetDynamicSettings(PluginSettings s);
    }
}

using System;
using System.Drawing;

namespace GDMInterfaces {
    /// <summary>
    /// Base interface of all plugins. Contains the basic operations supported by all plugin types and the identification parts like name, version, description etc.
    /// </summary>
    /// <remarks>A plugin will only be recognized if it is implementing ITool, IOutput or IInput. A plugin will not be recognized by simple implementing this interface.</remarks>
    public interface IPlugin {
        /// <summary>
        /// A short text message describing the functionality of the plug-in. Used as tooltip when hoovering a plug-in in the menu.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Current version of the plug-in.
        /// </summary>
        string Version
        {
            get;
        }

        /// <summary>
        /// The name of the plug-in.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Used as icon in the plug-in menu and the configuration view.
        /// </summary>
        Image Icon
        {
            get;
        }


        /// <summary>
        /// Creates a new instance of the settings class that is used by this plug-in.
        /// </summary>
        /// <remarks>All settings classes must be a sub class of PluginSettings.</remarks>
        PluginSettings GetSettings(IModel model);

        /// <summary>
        /// Updates the settings object to reflect changes made to the model.
        /// </summary>
        void UpdateSettings(PluginSettings pluginSettings, IModel model);

        /// <summary>
        /// Returns the Type of the settings class that is used by this plug-in.
        /// </summary>
        Type GetSettingsType();

        /// <summary>
        /// Executes the functionality of the plug-in on the data.
        /// </summary>
        /// <param name="model">Gives access to the model API</param>
        /// <param name="pluginSettings">
        /// An instance of the current plug-ins settings class. 
        /// This parameter can be cast to the correct type by the implementor.
        /// The object contains the settings made for the current execution.
        /// </param>
        /// <param name="log">Gives access to the application log API.</param>
        /// <param name="status">Gives access to the status and status bar API.</param>
        void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status);
    }
}

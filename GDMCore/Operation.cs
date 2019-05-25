using System;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMCore
{
    /// <summary>
    /// The operation class defines an operation made to the data. It consist of the name of the plug-in used to perform the operation and a PluginSettings object that contain the settings.
    /// </summary>
    public class Operation
    {
        private string _plugin;
        private PluginSettings _settings;

        public Operation() { }

        internal Operation(string plugin, PluginSettings settings)
        {
            _plugin = plugin;
            _settings = settings;
        }

        [XmlAttribute("plugin")]
        public string Plugin
        {
            get { return _plugin; }
            set { _plugin = value; }
        }

        [XmlElement]
        public PluginSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }
        [XmlElement]
        public TimeSpan LastTime { get; set; }
    }
}

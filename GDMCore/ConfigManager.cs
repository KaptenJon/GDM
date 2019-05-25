using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMCore
{
    /// <summary>
    /// Holds the current configuration and the previously made configurations. The stored XML-configurations are stored in a folder and is deserialized upon instantiation.
    /// </summary>
    /// <remarks></remarks>
    public class ConfigManager
    {
        private string _path;
        private List<Config> _configurations;
        private Config _currentConfig;
        private Type[] _settingtypes;

        public ConfigManager(string path, Type[] settingtypes)
        {
            _settingtypes = settingtypes;
            _configurations = new List<Config>();
            _currentConfig = new Config();
            _path = path;
            
                LoadConfigFiles();
            
        }

        public List<Config> Configurations
        {
            get { return _configurations; }
            set { _configurations = value; }
        }
        
        public Config CurrentConfig
        {
            get { return _currentConfig; }
            set { _currentConfig = value; }
        }

        /// <summary>
        /// Saves the current config to file.
        /// </summary>
        /// <param name="name">The name of the config</param>
        public void SaveCurrent(string name)
        {
            string file = _path + "\\" + name;
            if (!name.ToLower().Contains(".xml"))
                file += ".xml";
            _currentConfig.Name = name;
            Save(_currentConfig,file);
            _currentConfig.Name = null;
            Config saved = Load(file);
            if(saved != null)
                _configurations.Add(saved);
        }

        public bool IsValidNewName(string name)
        {
            if (name == null) return false;
            if (name.Length == 0) return false;

            foreach (Config c in _configurations)
            {
                if (c.Name == name) return false;
            }

            return (!name.Contains("\\") || !name.Contains("/"));
        }

        public void DeleteConfig(Config config)
        {
            try
            {
                _configurations.Remove(config);
              
                File.Delete(config.FileName);
            }
            catch { return; }
        }

        /// <summary>
        /// Loads the Config files from the filesystem by deserialize the Xml.
        /// </summary>
        public void LoadConfigFiles()
        {
            try
            {
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }

                foreach (string file in Directory.GetFiles(_path))
                {
                    Config config = Load(file);
                    if(config != null)
                        Configurations.Add(config);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns an instance of Config from an XML-serialized object.
        /// </summary>
        /// <remarks>Throws an exception if the file cant be deserialized</remarks>
        private Config Load(string xmlFile)
        {
            using (XmlTextReader reader = new XmlTextReader(xmlFile))
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Config),_settingtypes);
                    Config obj = (Config)ser.Deserialize(reader);
                    if(obj.Operations.Any(t=> Controller.PluginManager.GetPlugin(t.Plugin)==null))
                        throw new Exception();
                    obj.FileName = xmlFile;
                    return obj;
                }
                catch (Exception e)
                {
                    if(PluginSettings.IsInUIMode)
                        if (MessageBox.Show(e.Message + "\n" + xmlFile + "\n" +
                                            "Try To Delete it?", "Error loading Config", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                            try
                            {
                                File.Delete(xmlFile);
                            }
                            catch
                            {
                            }
                        else
                            throw;
                    return null;

                }
            }
        }

        /// <summary>
        /// Serializes a config instance to XML.
        /// </summary>
        public void Save(Config config, string xmlFile)
        {
            using (XmlTextWriter writer = new XmlTextWriter(xmlFile, Encoding.UTF8))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Config),_settingtypes);
                writer.Formatting = Formatting.Indented;
                ser.Serialize(writer, config);
            }
        }

     

        public void Export(Config config,string file)
        {
            try
            {
                
                Save(config, file);
            }
            catch(Exception)
            {
                throw new Exception("Could not export the file");
            }
        }

        public void Import(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    Config import = Load(file);
                    if(import == null)
                        return;
                    _configurations.Add(import);
                    Save(import,_path + "\\" + import.Name + ".xml");
                }
                catch (Exception)
                {
                    throw new Exception("Could not import the file");
                }
            }
        }
    }
}

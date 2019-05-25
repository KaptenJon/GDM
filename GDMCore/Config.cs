using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMCore
{
    /// <summary>
    /// The Config class defines one set of operations to 
    /// perform on the data and the tags placed on the data. It consists of a descriptive name of the Config and a list of Operations. Xml- serialization/deserialization of the Config object is handled by the ConfigManager.
    /// </summary>
    /// <remarks></remarks>
    public class Config
    {
        private string _name;
        private string _filename;
        private List<Operation> _operations;
        private List<ConfigTag> _tags;

        internal Config()
        {
            _operations = new List<Operation>();
            _tags = new List<ConfigTag>();
        }
        
        public List<Operation> Operations
        {
            get { return _operations; }
            set { _operations = value; }
        }

        public List<ConfigTag> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        [XmlIgnore]
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Time => Operations.Sum(t => t.LastTime.TotalSeconds).ToString(CultureInfo.CurrentCulture); 

        internal void AddOperation(IPlugin plugin, PluginSettings settings, TimeSpan time)
        {
            Operation operation = new Operation();
            operation.Plugin = plugin.Name;
            operation.Settings = settings;
            operation.LastTime = time;

            _operations.Add(operation);
        }

        public void RemoveOperation(Operation operation)
        {
           _operations.Remove(operation);
          
        }

        internal void RefreshTags(List<Tag> taglist)
        {
            _tags.Clear();
            foreach (Tag t in taglist)
            {
                List<string> path = new List<string>();
                RefreshTag(t, path);
            }
        }

        private void RefreshTag(Tag t, List<string> path)
        {
            path.Add(t.TagName);
            if (t.Entity != null)
            {
                ConfigTag configtag = new ConfigTag();
                configtag.Entity = t.Entity;
                configtag.Path = path;
                _tags.Add(configtag);
            }
            if (t.Subtags != null)
            {
                foreach (Tag subtag in t.Subtags)
                {
                    List<string> newpath = new List<string>(path);
                    RefreshTag(subtag,newpath);
                }
            }
        }

        internal void LoadSavedConfigTags(List<Tag> tags)
        {
            foreach (Tag t in tags) ClearTag(t);
            foreach (ConfigTag ct in _tags)
            {
                int index = 0;
                List<Tag> taglist = tags;
                while (index < ct.Path.Count)
                {
                    bool found = false;
                    Tag last = null;
                    foreach (Tag t in taglist)
                    {
                        if (t.TagName == ct.Path[index])
                        {
                            index++;
                            found = true;
                            last = t;
                            taglist = t.Subtags;
                            break;
                        }
                    }
                    if (!found) break;
                    if (index == ct.Path.Count && found)
                    {
                        last.Entity = ct.Entity;
                    }
                }
            }
        }

        private void ClearTag(Tag t)
        {
            t.Entity = null;
            if (t.Subtags != null)
            {
                foreach (Tag tag in t.Subtags)
                {
                    ClearTag(tag);
                }
            }
        }

        [XmlType("Tag")]
        public class ConfigTag
        {
            public List<string> Path;
            [XmlAttribute("entity")]
            public string Entity;
        }
    }
}

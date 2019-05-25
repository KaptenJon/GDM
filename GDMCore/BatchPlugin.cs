using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GDMCore;


namespace GDMInterfaces
{
    public class BatchPlugin : IPlugin
    {
        private static Type[] _settingtypes = PluginManager.PluginSettingsType;
        private static BatchPluginSettings _currentBatchPlugin;

        public string Description => "Use to start du a set of plugins on multiple files";

        public string Version => "1.0";

        public string Name => "Batch Plugin";

        public Image Icon => null;

        public BatchPlugin()
        {
            _settingtypes = PluginManager.PluginSettingsType;
        }

        public BatchPlugin(Type[] types)
        {
            _settingtypes = types;
        }

      

        public PluginSettings GetSettings(IModel model)
        {
            return new BatchPluginSettings(); ;
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
        }

        public Type GetSettingsType()
        {
            return typeof(BatchPluginSettings);
        }

        public static void ExecuteAllButFirst(Model model, Log log, Status status)
        {
            var tables = model.GetAllTableNames();
            if(CurrentBatchPlugin.ConstantValue.Count<2)
                return;
            foreach (var newvalue in BatchPlugin.CurrentBatchPlugin.ConstantValue.Skip(1))
            {
                foreach (var setting in BatchPlugin.CurrentBatchPlugin.ChildPlugins)
                {
                    var parameterToExchange = ListBoxEditor.CurrentConstant;
                    var t = PluginSettings.CopyAndChangeParameter(setting.Settings, parameterToExchange, newvalue, PluginManager.PluginSettingsType);
                    Controller.PluginManager.GetPlugin(setting.Plugin).Apply(model, t, log, status);
                    
                }
                SetNewTableNames(model, tables, BatchPlugin.CurrentBatchPlugin.ConstantValue.IndexOf(newvalue) );
            }
        }

        public static void ApplyPluginToVizulizeBatch(IPlugin plugin, PluginSettings settings, ILog log, IStatus status, Model model)
        {
            var tables = model.GetAllTableNames();
            var newsetting = PluginSettings.CopyAndChangeParameter(settings,
                ListBoxEditor.CurrentConstant, BatchPlugin.CurrentBatchPlugin.ConstantValue.First(),
                PluginManager.PluginSettingsType);
            plugin.Apply(model, newsetting, log, status);
            BatchPlugin.CurrentBatchPlugin.ChildPlugins.Add(new Operation(plugin.Name, settings));
            SetNewTableNames(model, tables, 0);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var batchPluginSetting = pluginSettings as BatchPluginSettings;
            if(batchPluginSetting == null)
                return;
            //BatchPlugin.CurrentBatchPlugin = batchPluginSetting;

            var tables = model.GetAllTableNames();
            
            foreach (var newvalue in batchPluginSetting.ConstantValue)
            {
                foreach (var setting in batchPluginSetting.ChildPlugins)
                {
                   var parameterToExchange = batchPluginSetting.ConstantName;
                    var t = PluginSettings.CopyAndChangeParameter(setting.Settings, parameterToExchange, newvalue, _settingtypes);
                    Controller.PluginManager.GetPlugin(setting.Plugin).Apply(model, t, log, status);
                    
                }
                SetNewTableNames(model, tables, batchPluginSetting.ConstantValue.IndexOf(newvalue));
            }
        }

        private static void SetNewTableNames(IModel model, ISet<string> tables, int i)
        {
            foreach (DataTable table in model.DataSet.Tables)
            {
                if (!tables.Contains(table.TableName))
                {
                    table.TableName = "§" + i + "§" + table.TableName;
                    tables.Add(table.TableName);
                }
            }
            
            
        }

        private static int FindCurrentI(ISet<string> tables)
        {
            var highestnr = tables.LastOrDefault(t => t.StartsWith("§"));
            int i = 0;
            if (highestnr != null)
            {
                var startnr = highestnr.Substring(2);
                if (startnr.Contains("§"))
                {
                    var endnr = startnr.IndexOf("§");
                    i = int.Parse(startnr.Substring(0, endnr));
                    i++;
                }
            }
            return i;
        }


        public static BatchPluginSettings CurrentBatchPlugin
        {
            get { return _currentBatchPlugin; }
            set
            {
                _currentBatchPlugin = value;
                if (value == null) ListBoxEditor.CurrentConstant = null;
            }
        }
    }


    public class BatchPluginSettings : PluginSettings
    {
        private static int nextint = 0;
        private List<string> _constantValue = new List<string>();
        private string _constantName;
        private List<Operation> _childPlugins = new List<Operation>();


        [Browsable(true)]
        [ReadOnly(true)]
        public string ConstantName
        {
            set { _constantName = value; }
            get { return _constantName; }
        }

        public List<string> ConstantValue
        {
            get { return _constantValue; }
            set
            {
                _constantValue = value;
                
            }
        }

        [XmlArray("ChildPlugins")]
        [XmlArrayItem("Operation")]
        [Browsable(false)]
        public List<Operation> ChildPlugins
        {
            get { return _childPlugins; }
            set { _childPlugins = value; }
        }

        public override async Task<bool> IsValid()
        {
            return await Factory.StartNew(()=>BatchPlugin.CurrentBatchPlugin == null && ConstantName != null && ConstantValue != null && ConstantValue.Any());
        }

        public void InitConstant()
        {
            ListBoxEditor.CurrentConstant = "§§" + (nextint++) + "§§"; ;
            _constantName = ListBoxEditor.CurrentConstant;
        }
    }
}

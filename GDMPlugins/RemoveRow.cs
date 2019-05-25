using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveRow : ITool
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Remove";

        public string Description => "Removes a row";

        public string Version => "1.0";

        public string Name => "Remove Row";

        public Image Icon => Icons.RemoveRow;

        public PluginSettings GetSettings(IModel model)
        {
            RemoveRowSettings settings = new RemoveRowSettings();
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings settings, IModel model)
        {
            ((RemoveRowSettings)settings).TableName = model.SelectedTable?.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(RemoveRowSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RemoveRowSettings rSettings = (RemoveRowSettings)pluginSettings;
            DataTable table = model.GetTable(rSettings.TableName);
            status.InitStatus("Removing row...", 1);
            table.Rows.RemoveAt(rSettings.RowNumber);
            status.Increment();
        }
    }
}
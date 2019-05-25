using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveTable : ITool
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Remove";

        public string Description => "Removes a table";

        public string Version => "1.0";

        public string Name => "Remove Table";

        public Image Icon => Icons.RemoveTable;

        public PluginSettings GetSettings(IModel model)
        {
            RemoveTableSettings settings = new RemoveTableSettings();
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings settings, IModel model)
        {
            if(model.SelectedTable == null)
                return;
            ((RemoveTableSettings)settings).TableName = model.SelectedTable.TableName;
            ((RemoveTableSettings) settings).Model = model;
        }

        public Type GetSettingsType()
        {
            return typeof(RemoveTableSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RemoveTableSettings settings = (RemoveTableSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            model.DropTable(settings.TableName);
        }
    }
}
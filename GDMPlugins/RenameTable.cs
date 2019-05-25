using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RenameTable : ITool
    {

        public RenameTable()
        {
        }

        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Rename";

        public string Description => "Rename a table.";

        public string Version => "1.0";

        public string Name => "Rename Table";

        public Image Icon => Icons.RenameTable;

        public PluginSettings GetSettings(IModel model)
        {
            RenameTableSettings settings = new RenameTableSettings();
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings settings, IModel model)
        {
            ((RenameTableSettings)settings).TableName = model.SelectedTable?.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(RenameTableSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RenameTableSettings settings = (RenameTableSettings)pluginSettings;
            status.InitStatus("Renaming column...", 1);
            DataTable table = model.GetTable(settings.TableName);
            table.TableName = settings.NewTableName;
            status.Increment();
        }
    }
}
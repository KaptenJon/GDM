using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RenameColumn : ITool
    {
        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Rename";

        public string Description => "Plugin for renaming a datatable's column";

        public string Version => "1.0";

        public string Name => "Rename Column";

        public Image Icon => Icons.RenameColumn;

        public PluginSettings GetSettings(IModel model)
        {
            return new RenameColumnSettings();
        }

        public Type GetSettingsType()
        {
            return typeof(RenameColumnSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RenameColumnSettings settings = (RenameColumnSettings)pluginSettings;
            DataTable datatable = model.GetTable(settings.TableName);
            datatable.Columns[settings.ColumnName].ColumnName = settings.NewName;
            log.Add(LogType.Success, "Column renamed successfully");
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            RenameColumnSettings settings = (RenameColumnSettings)pluginSettings;
            settings.ColumnName = model.SelectedColumnName;
            settings.TableName = model.SelectedTable.TableName;
        }
    }
}

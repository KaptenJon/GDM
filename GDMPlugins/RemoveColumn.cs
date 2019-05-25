using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveColumn : ITool
    {

        public RemoveColumn() { }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;



        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Remove";

        public string Description => "Removes a column";

        public string Version => "1.0";

        public string Name => "Remove Column";

        public Image Icon => Icons.RemoveColumn;

        public PluginSettings GetSettings(IModel model)
        {
            RemoveColumnSettings settings = new RemoveColumnSettings();
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            RemoveColumnSettings settings = (RemoveColumnSettings)pluginSettings;
            if (model.SelectedTable != null) settings.TableName = model.SelectedTable.TableName;
            if (model.SelectedColumnName != null) settings.ColumnName = model.SelectedColumnName;
        }

        public Type GetSettingsType()
        {
            return typeof(RemoveColumnSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RemoveColumnSettings settings = (RemoveColumnSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            status.InitStatus("Removing column...", 1);

            if (!table.Columns.Contains(settings.ColumnName))
            {
                log.Add(LogType.Error, "Column Name to remove did not exsist. Table:" + settings.TableName + " Column:" + settings.ColumnName);
                return;
            }

            if (table.Columns.Count == 1)   // Remove table if only 1 column left 
            {
                model.DropTable(settings.TableName);
                log.Add(LogType.Warning, "The whole table removed instead of just the last column");
            }
            else
            {
                table.Columns.Remove(settings.ColumnName);
            }

            status.Increment();
        }
    }
}
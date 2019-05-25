using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    class DateInterval : ITool
    {
        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            if (t == typeof(DateTime))
                return true;
            else return false;
        }

        public string ToolCategory => "Filter";

        public string Description => "Plugin for ...";

        public string Version => "1.0";

        public string Name => "Date Interval";

        public Image Icon => Icons.DateInterval;

        public PluginSettings GetSettings(IModel model)
        {
            return new DateIntervalSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            DateIntervalSettings settings = (DateIntervalSettings)pluginSettings;
            settings.ColumnName = model.SelectedColumnName;
            settings.TableName = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(DateIntervalSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            DateIntervalSettings settings = (DateIntervalSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            DataTable result = new DataTable(table.TableName);
            foreach(DataColumn column in table.Columns)result.Columns.Add(column.ColumnName, column.DataType);

            switch (settings.Mode)
            {
                case DateIntervalSettings.DateFilterMode.LowerBound:
                    foreach (DataRow row in table.Rows)
                    {
                        if ((DateTime)row[settings.ColumnName] >= settings.MinimumDateTime)
                            result.ImportRow(row);
                    }
                    break;
                case DateIntervalSettings.DateFilterMode.UpperBound:
                    foreach (DataRow row in table.Rows)
                    {
                        if ((DateTime)row[settings.ColumnName] <= settings.MaximumDateTime)
                            result.ImportRow(row);
                    }
                    break;
                case DateIntervalSettings.DateFilterMode.Interval:
                    foreach (DataRow row in table.Rows)
                    {
                        if ((DateTime)row[settings.ColumnName] >= settings.MinimumDateTime &&
                            (DateTime)row[settings.ColumnName] <= settings.MaximumDateTime)
                        {
                            result.ImportRow(row);
                        }
                    }
                    break;
            }

            if(result.Rows.Count>0)
            {
                model.DropTable(table.TableName);
                model.AddTable(result);
            }
            else
            {
                log.Add(LogType.Warning, "No rows satisfied given condition");
                return;
            }

            log.Add(LogType.Success, table.Rows.Count - result.Rows.Count + " rows were removed");
        }
    }
}

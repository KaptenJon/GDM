using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class NumericInterval : ITool
    {
        public string Description => "Filter data by giving a minimum and/or maximum value";

        public string Version => "1.0";

        public string Name => "Numerical Interval";

        public Image Icon => Icons.NumericInterval;

        public PluginSettings GetSettings(IModel model)
        {
            return new NumericIntervalSettings();
        }

        public Type GetSettingsType()
        {
            return typeof(NumericIntervalSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            NumericIntervalSettings settings = (NumericIntervalSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            DataTable result = new DataTable(table.TableName);
            foreach (DataColumn column in table.Columns) result.Columns.Add(column.ColumnName, column.DataType);

            switch (settings.Mode)
            {
                case NumericIntervalSettings.NumericFilterMode.LowerBound:
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[settings.ColumnName] as double? >= settings.Minimum)
                            result.ImportRow(row);
                    }
                    break;
                case NumericIntervalSettings.NumericFilterMode.Interval:
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[settings.ColumnName] as double? >= settings.Minimum &&
                            row[settings.ColumnName] as double? <= settings.Maximum)
                        {
                            result.ImportRow(row);
                        }
                    }
                    break;
                case NumericIntervalSettings.NumericFilterMode.UpperBound:
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[settings.ColumnName] as double? <= settings.Maximum)
                            result.ImportRow(row);
                    }
                    break;
            }

            if (result.Rows.Count > 0)
            {
                model.DropTable(table.TableName);
                model.AddTable(result);
            }
            else
            {
                log.Add(LogType.Warning, "No rows satisfied given conditions");
                return;
            }

            log.Add(LogType.Success, table.Rows.Count - result.Rows.Count + " rows were filtered away");
        }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool ShowSettingsObject
        {
            get { throw new NotImplementedException(); }
        }

        public bool AcceptsDataType(Type t)
        {
            return (t == typeof(int) || t == typeof(double));
        }

        public string ToolCategory => "Filter";

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            NumericIntervalSettings settings = (NumericIntervalSettings)pluginSettings;
            settings.ColumnName = model.SelectedColumnName;
            if (model.SelectedTable != null)
            {
                settings.TableName = model.SelectedTable.TableName;
            }
        }
    }
}

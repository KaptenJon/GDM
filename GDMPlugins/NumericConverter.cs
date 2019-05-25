using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class NumericConverter : ITool
    {
        public string Description => "Convert a column to integer or double";

        public string Version => "1.0";

        public string Name => "Numeric Converter";

        public Image Icon => Icons.NumericConverter;

        public PluginSettings GetSettings(IModel model)
        {
            return new NumericConverterSettings();
        }

        public Type GetSettingsType()
        {
            return typeof(NumericConverterSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            NumericConverterSettings settings = (NumericConverterSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            List<DataRow> toRemove = new List<DataRow>(); Type dataType;

            if (settings.Type == NumericConverterSettings.DataType.Integer) dataType = Type.GetType("System.Int32");
            else dataType = Type.GetType("System." + settings.Type);

            // Add temporary column to existing table
            int j = 0; while (table.Columns.Contains("Col" + j)) { j++; }
            table.Columns.Add("Col" + j, dataType);
            status.InitStatus("Converting column...", table.Rows.Count);

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    // Force delete/abort
                    if (row[settings.ColumnName] == DBNull.Value)
                        throw new Exception();
                    else if (row[settings.ColumnName].ToString() == null)
                        throw new Exception();
                    else if (row[settings.ColumnName].ToString() == "")
                        throw new Exception();

                    switch (settings.Type)
                    {
                        case NumericConverterSettings.DataType.Double:
                            // Cannot convert if "." is used as comma character
                            double doubleData = Convert.ToDouble(row[settings.ColumnName].ToString().Replace(".", ","));
                            row["Col" + j] = doubleData;
                            break;
                        case NumericConverterSettings.DataType.Integer:
                            int intData = Convert.ToInt32(row[settings.ColumnName].ToString().Replace(".", ","));
                            row["Col" + j] = intData;
                            break;
                        case NumericConverterSettings.DataType.String:
                            string stringData = row[settings.ColumnName].ToString().Replace(".", ",");
                            row["Col" + j] = stringData;
                            break;
                    }
                }
                catch   // Error during conversion:
                {
                    switch (settings.Action)
                    {
                        case NumericConverterSettings.NumericErrorAction.Delete:
                            toRemove.Add(row);
                            log.Add(LogType.Warning, "Row " + status.CurrentStatus + " was removed because it could not be converted to " + dataType.ToString());
                            break;
                        case NumericConverterSettings.NumericErrorAction.Abort: // Undo what was done
                            table.Columns.Remove(table.Columns["Col" + j]);
                            log.Add(LogType.Warning, "Operation aborted because row " + status.CurrentStatus + "could not be converted to " + dataType.ToString());
                            return;
                    }
                }

                status.Increment();
            }

            if (settings.Action == NumericConverterSettings.NumericErrorAction.Delete)
                foreach (DataRow row in toRemove) table.Rows.Remove(row);

            table.Columns.Remove(settings.ColumnName);
            table.Columns["Col" + j].ColumnName = settings.ColumnName;
        }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool ShowSettingsObject
        {
            get { throw new NotImplementedException(); }
        }

        public bool AcceptsDataType(Type t)
        {
            return (t == typeof(string) || t == typeof(int) || t == typeof(double));
        }

        public string ToolCategory => "Converters";

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            NumericConverterSettings settings = (NumericConverterSettings)pluginSettings;
            if (model.SelectedColumnName != null) settings.ColumnName = model.SelectedColumnName;
            if (model.SelectedTable != null) settings.TableName = model.SelectedTable.TableName;
        }
    }
}
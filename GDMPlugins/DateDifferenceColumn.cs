using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class DateDifferenceColumn : ITool
    {
        private bool _col = true;

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return t == typeof(DateTime) || t == typeof(TimeSpan);
        }

        public string ToolCategory => "Calculate";

        public string Description => "Calculates the time difference between two columns in a date column. The value is put into a new column in either hours, minutes, seconds or milliseconds.";

        public string Version => "1.0";

        public string Name => "Time Difference Between Columns";

        public Image Icon => Icons.DateDifference;

        public PluginSettings GetSettings(IModel model)
        {
            DateDifferenceColumnSettings columnSettings = new DateDifferenceColumnSettings(model);
            UpdateSettings(columnSettings, model);
            return columnSettings;
        }
        private string _prevSelect = "";
        private bool _xmlInit=false;

        public void UpdateSettings(PluginSettings _settings, IModel model)
        {
            if (!_xmlInit)
            {
                _xmlInit = true;
                return;
            }
            DateDifferenceColumnSettings columnSettings = _settings as DateDifferenceColumnSettings;

            if (model.SelectedTable == null) return;
            else columnSettings.TableName = model.SelectedTable.TableName;

            _col = !_col;
            _prevSelect = model.SelectedColumnName;
        }

        public Type GetSettingsType()
        {
            return typeof(DateDifferenceColumnSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            DateDifferenceColumnSettings columnSettings = pluginSettings as DateDifferenceColumnSettings;
            DataTable table = model.GetTable(columnSettings.TableName);
            status.InitStatus("Time Difference...", table.Rows.Count);
            table.Columns.Add(columnSettings.NewColumn, typeof(double));

            for (int i = 0; i>=0 && i < table.Rows.Count; i += 1)
            {
                DataRow row = table.Rows[i];
                if (table.Columns[columnSettings.FirstDateColumn].DataType == typeof(DateTime))
                {
                    var previous = row[columnSettings.FirstDateColumn] as DateTime?;
                    var first = row[columnSettings.SecondDateSecondColumn] as DateTime?;
                     double result = CalcDateDifference(previous.Value, first.Value, columnSettings.TimeFormat);
                    row[columnSettings.NewColumn] = result;
                }
                else if (table.Columns[columnSettings.FirstDateColumn].DataType == typeof (String))
                {
                    try
                    {
                        var previous = DateTime.Parse((string) table.Rows[i][columnSettings.FirstDateColumn]);
                        var first = DateTime.Parse((string) table.Rows[i][columnSettings.SecondDateSecondColumn]);
                        double result = CalcDateDifference(previous, first, columnSettings.TimeFormat);
                        row[columnSettings.NewColumn] = result;
                    }
                    catch (Exception)
                    {
                        log.Add(LogType.Error, "Conversion Error");

                    }
                }
                else if (table.Columns[columnSettings.FirstDateColumn].DataType == typeof (TimeSpan))
                {
                    var previous = (table.Rows[i][columnSettings.FirstDateColumn] as TimeSpan?);
                    var first = table.Rows[i][columnSettings.SecondDateSecondColumn] as TimeSpan?;
                    double result = CalcDateDifference(previous.Value, first.Value, columnSettings.TimeFormat);
                    row[columnSettings.NewColumn] = result;
                }

            }

            log.Add(LogType.Success, "Operation performed on " + table.Rows.Count + " unique entries");
        }

        private static double CalcDateDifference(DateTime prev, DateTime current, DateDifferenceColumnSettings.TimeUnitColumn timeUnitColumn)
        {
            TimeSpan ts = current.Subtract(prev);

            switch (timeUnitColumn)
            {
                case DateDifferenceColumnSettings.TimeUnitColumn.Hours:
                    return ts.TotalHours;
                case DateDifferenceColumnSettings.TimeUnitColumn.Milliseconds:
                    return ts.TotalMilliseconds;
                case DateDifferenceColumnSettings.TimeUnitColumn.Seconds:
                    return ts.TotalSeconds;
                case DateDifferenceColumnSettings.TimeUnitColumn.Minutes:
                    return ts.TotalMinutes;
            }
            return 0;
        }
        private static double CalcDateDifference(TimeSpan prev, TimeSpan current,DateDifferenceColumnSettings.TimeUnitColumn timeUnit)
        {
            TimeSpan ts = current.Subtract(prev);

            switch (timeUnit)
            {
                case DateDifferenceColumnSettings.TimeUnitColumn.Hours:
                    return ts.TotalHours;
                case DateDifferenceColumnSettings.TimeUnitColumn.Milliseconds:
                    return ts.TotalMilliseconds;
                case DateDifferenceColumnSettings.TimeUnitColumn.Seconds:
                    return ts.TotalSeconds;
                case DateDifferenceColumnSettings.TimeUnitColumn.Minutes:
                    return ts.TotalMinutes;
            }
            return 0;
        }
    }
}
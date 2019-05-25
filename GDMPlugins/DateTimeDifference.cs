using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    /// <summary>
    /// Made by Jon Andersson
    /// </summary>
    public class DateTimeDifference : ITool
    {
        private Hashtable _lookUpTable;

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return t == typeof(DateTime) || t == typeof(TimeSpan);
        }

        public string ToolCategory => "Calculate";

        public string Description => "Calculates the time difference between two rows in a date column. The value is put into a new column in either hours, minutes, seconds or milliseconds.";

        public string Version => "1.0";

        public string Name => "Date or Time Difference";

        public Image Icon => Icons.DateDifference;

        public PluginSettings GetSettings(IModel model)
        {
            DateTimeDifferenceSettings settings = new DateTimeDifferenceSettings(model);
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings _settings, IModel model)
        {
            DateTimeDifferenceSettings settings = _settings as DateTimeDifferenceSettings;

            if (model.SelectedTable == null) return;
            else settings.TableName = model.SelectedTable.TableName;

            if (model.SelectedColumnType == typeof(DateTime)||model.SelectedColumnType == typeof(TimeSpan))
            {
                settings.DateColumn = model.SelectedColumnName;
            }
            else
            {
                settings.DateColumn = "";
            }
        }

        public Type GetSettingsType()
        {
            return typeof(DateTimeDifferenceSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            DateTimeDifferenceSettings settings = pluginSettings as DateTimeDifferenceSettings;
            DataTable table = model.GetTable(settings.TableName);
            status.InitStatus("Time Difference...", table.Rows.Count);
            table.Columns.Add(settings.NewColumn, typeof (double));

            int offset = 0, inc = 1; // Sorting = Increasing
            if (settings.Sorting == DateTimeDifferenceSettings.SortingOption.Decreasing)
            {
                offset = table.Rows.Count - 1;
                inc = -1;
            }
            if (table.Columns[settings.DateColumn].DataType == typeof (DateTime))
            {
                if (settings.GroupingColumns == null) // CalculateRows time difference between every row
                {

                    DateTime? previous = table.Rows[offset][settings.DateColumn] as DateTime?;
                    while (previous == null)
                    {
                        previous = table.Rows[offset][settings.DateColumn] as DateTime?;
                        table.Rows.RemoveAt(offset);
                    }
                    for (int i = offset; i >= 0 && i < table.Rows.Count; i += inc)
                    {
                        DataRow row = table.Rows[i];
                        DateTime? current = row[settings.DateColumn] as DateTime?;
                        if (current != null)
                        {
                            double result = CalcDateDifference(previous.Value, current.Value, settings.TimeFormat);
                            if(i>0)
                                table.Rows[i-1][settings.NewColumn] = result;
                            previous = current;
                        }
                        else
                        {
                            table.Rows.RemoveAt(i);
                            i -= inc;
                        }
                    }
                }
                else // CalculateRows time difference between every unique entry in grouping columns
                {
                    _lookUpTable = new Hashtable();

                    for (int i = offset; i >= 0 && i < table.Rows.Count; i += inc)
                    {
                        DataRow row = table.Rows[i];
                        var date = row[settings.DateColumn] as DateTime?;
                        if (date == null)
                        {
                            table.Rows.Remove(row);
                            i -= inc;
                        }
                        else
                        {
                            string entry = "";

                            foreach (string column in settings.GroupingColumns)
                            {
                                entry += row[column].ToString();
                            }

                            if (!_lookUpTable.Contains(entry))
                            {

                                _lookUpTable.Add(entry, date.Value);
                                row[settings.NewColumn] = 0;
                            }
                            else
                            {
                                DateTime previous = (DateTime) _lookUpTable[entry];
                                DateTime current = date.Value;
                                double result = CalcDateDifference(previous, current, settings.TimeFormat);
                                if (i > 0)
                                    table.Rows[i - 1][settings.NewColumn] = result;
                                _lookUpTable[entry] = (DateTime) row[settings.DateColumn];
                                // Update with newest date
                            }
                        }
                        status.Increment();

                    }
                }
            }
            else if (table.Columns[settings.DateColumn].DataType == typeof(TimeSpan))
            {
                if (settings.GroupingColumns == null) // CalculateRows time difference between every row
                {

                    var previous = table.Rows[offset][settings.DateColumn] as TimeSpan?;
                    while (previous == null)
                    {
                        previous = table.Rows[offset][settings.DateColumn] as TimeSpan?;
                        table.Rows.RemoveAt(offset);
                    }
                    for (int i = offset; i >= 0 && i < table.Rows.Count; i += inc)
                    {
                        DataRow row = table.Rows[i];
                        var current = row[settings.DateColumn] as TimeSpan?;
                        if (current != null)
                        {
                            int result = CalcDateDifference(previous.Value, current.Value, settings.TimeFormat);
                            row[settings.NewColumn] = result;
                            previous = current;
                        }
                        else
                        {
                            table.Rows.RemoveAt(i);
                            i -= inc;
                        }
                    }
                }
                else // CalculateRows time difference between every unique entry in grouping columns
                {
                    _lookUpTable = new Hashtable();

                    for (int i = offset; i >= 0 && i < table.Rows.Count; i += inc)
                    {
                        DataRow row = table.Rows[i];
                        var date = row[settings.DateColumn] as TimeSpan?;
                        if (date == null)
                        {
                            table.Rows.Remove(row);
                            i -= inc;
                        }
                        else
                        {
                            string entry = "";

                            foreach (string column in settings.GroupingColumns)
                            {
                                entry += row[column].ToString();
                            }

                            if (!_lookUpTable.Contains(entry))
                            {

                                _lookUpTable.Add(entry, date.Value);
                                row[settings.NewColumn] = 0;
                            }
                            else
                            {
                                var previous = (TimeSpan) _lookUpTable[entry];
                                var current = date.Value;
                                int result = CalcDateDifference(previous, current, settings.TimeFormat);
                                row[settings.NewColumn] = result;
                                _lookUpTable[entry] = (TimeSpan) row[settings.DateColumn];
                                // Update with newest date
                            }
                        }
                        status.Increment();

                    }
                }
            }

            log.Add(LogType.Success, "Operation performed on " + _lookUpTable?.Count + " unique entries");
            

        }

        private static double CalcDateDifference(DateTime prev, DateTime current, DateTimeDifferenceSettings.TimeUnit timeUnit)
        {
            TimeSpan ts = current.Subtract(prev);

            switch (timeUnit)
            {
                case DateTimeDifferenceSettings.TimeUnit.Hours:
                    return Math.Abs(ts.TotalHours);
                case DateTimeDifferenceSettings.TimeUnit.Milliseconds:
                    return Math.Abs(ts.TotalMilliseconds);
                case DateTimeDifferenceSettings.TimeUnit.Seconds:
                    return Math.Abs(ts.TotalSeconds);
                case DateTimeDifferenceSettings.TimeUnit.Minutes:
                    return Math.Abs(ts.TotalMinutes);
            }
            return 0;
        }
        private static int CalcDateDifference(TimeSpan prev, TimeSpan current, DateTimeDifferenceSettings.TimeUnit timeUnit)
        {
            TimeSpan ts = current.Subtract(prev);

            switch (timeUnit)
            {
                case DateTimeDifferenceSettings.TimeUnit.Hours:
                    return (int)ts.TotalHours;
                case DateTimeDifferenceSettings.TimeUnit.Milliseconds:
                    return (int)ts.TotalMilliseconds;
                case DateTimeDifferenceSettings.TimeUnit.Seconds:
                    return (int)ts.TotalSeconds;
                case DateTimeDifferenceSettings.TimeUnit.Minutes:
                    return (int)ts.TotalMinutes;
            }
            return 0;
        }
    }
}
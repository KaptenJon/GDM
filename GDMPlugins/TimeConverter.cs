using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class TimeConverter:ITool
    {
        private List<DateConverter.TimeAtomBlock> _blockList;
        // Variables for datetime locality
        private string _previousDate = "";
        private DateTime _previousDateTime;

        public TimeConverter()
        {
            _blockList = new List<DateConverter.TimeAtomBlock>();
            Initialize();
        }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return t == typeof(string);
        }

        public string ToolCategory => "Converters";

        public string Description => "Converts a string to a TimeSpan object";

        public string Version => "1.0";

        public string Name => "Time Converter";

        public Image Icon => Icons.DateConverter;

        public PluginSettings GetSettings(IModel model)
        {
            return new DateConverterSettings(_blockList);
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            DateConverterSettings settings = (DateConverterSettings)pluginSettings;
            if (model.SelectedColumnName != null) settings.ColumnName = model.SelectedColumnName;
            if (model.SelectedTable != null) settings.TableName = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(DateConverterSettings);
        }
        
        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            DateConverterSettings settings = (DateConverterSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            status.InitStatus("Converting column...", table.Rows.Count);
            List<DataRow> removeList = new List<DataRow>();
            int j = 0;

            while (table.Columns.Contains("Table" + j)) { j++; }
            var tempname = "Table" + j;
            table.Columns.Add(tempname, Type.GetType("System.TimeSpan"));
            var i = 0;
            while (i < table.Columns.Count && table.Columns[i++].ColumnName != settings.ColumnName)
            {
                
            }

            table.Columns[tempname].SetOrdinal(i);

            foreach (DataRow row in table.Rows)
            {
                var datetime = new DateTime();

                if (row[settings.ColumnName].ToString() == _previousDate)
                    datetime = _previousDateTime;
                else
                {
                    try
                    {
                        datetime = ConvertToDateTime(new DateTime(),
                            row[settings.ColumnName].ToString().TrimStart(new char[] { ' ' }),
                            table.TableName, status.CurrentStatus, 0, settings.Patterns, log);

                        _previousDate = row[settings.ColumnName].ToString();
                        _previousDateTime = datetime;
                    }
                    catch
                    {
                        switch (settings.Action)
                        {
                            case DateConverterSettings.DateErrorAction.Abort:
                                table.Columns.Remove(tempname);
                                return;
                            case DateConverterSettings.DateErrorAction.Delete:
                                removeList.Add(row);
                                break;
                        }
                    }
                }

                row[tempname] = datetime.TimeOfDay;
                status.Increment();
            }

            // Remove rows that could not be converted
            if (settings.Action == DateConverterSettings.DateErrorAction.Delete)
            {
                foreach (DataRow row in removeList)
                {
                    table.Rows.Remove(row);
                }
            }

            table.Columns.Remove(settings.ColumnName);
            table.Columns["Table" + j].ColumnName = settings.ColumnName;
        }

        public DateTime ConvertToDateTime(DateTime dateTimeObject, string dateTimeString, string tableName, int rowNumber, int index, string[] patterns, ILog log)
        {
            if (index >= patterns.Length) throw new Exception("Out of patterns.");  // Out of patterns to use

            bool someMatch = false; // Says wheter some atom match occurred at all

            foreach (DateConverter.TimeAtomBlock block in _blockList)
            {
                foreach (string atom in block.Atoms)
                {
                    if (patterns[index].Contains(atom)) // Check if pattern contains some atom
                    {
                        try
                        {
                            dateTimeObject = block.Converter(dateTimeObject, dateTimeString, patterns[index], block);
                            someMatch = true;
                            break;
                        }
                        catch
                        {
                            log.Add(LogType.Warning, tableName + ", Row " + rowNumber + ", Could not be converted using pattern " + patterns[index]);
                            // Try next pattern
                            return ConvertToDateTime(new DateTime(), dateTimeString, tableName, rowNumber, index + 1, patterns, log);
                        }
                    }
                }
            }
            if (someMatch) return dateTimeObject;
            else throw new Exception("No DateTimeAtom matches.");
        }
        public void Initialize()
        {
            // Important that the longest atom (for example yyyy) comes first
            DateConverter.TimeAtomBlock year = new DateConverter.TimeAtomBlock("Year", new string[] { "yyyy", "yyy", "yy" });
            DateConverter.TimeAtomBlock month = new DateConverter.TimeAtomBlock("Month", new string[] { "MM", "M" });
            DateConverter.TimeAtomBlock day = new DateConverter.TimeAtomBlock("Day", new string[] { "dd", "d" });
            DateConverter.TimeAtomBlock hour = new DateConverter.TimeAtomBlock("Hour", new string[] { "hh", "h" });
            DateConverter.TimeAtomBlock minute = new DateConverter.TimeAtomBlock("Minute", new string[] { "mm", "m" });
            DateConverter.TimeAtomBlock second = new DateConverter.TimeAtomBlock("Second", new string[] { "ss", "s" });
            DateConverter.TimeAtomBlock millisecond = new DateConverter.TimeAtomBlock("MilliSecond", new string[] { "fff", "ff", "f" });
            DateConverter.TimeAtomBlock week = new DateConverter.TimeAtomBlock("Week", new string[] { "ww", "w" });

            // Methods that will be run on a TimeAtom hit
            year.Converter += YearConverter;
            month.Converter += MonthConverter;
            day.Converter += DayConverter;
            hour.Converter += HourConverter;
            minute.Converter += MinuteConverter;
            second.Converter += SecondConverter;
            millisecond.Converter += MilliSecondConverter;
            week.Converter += WeekConverter;

            // A TimeAtom may exclude or require some other DateTimeAtom
            month.Excludes.Add(week);
            week.Excludes.Add(month);
            week.Requires.Add(year);
            week.Requires.Add(day);

            _blockList.AddRange(new DateConverter.TimeAtomBlock[] { year, month, day, hour, minute, second, millisecond, week });
        }

        // ------ Delegates for handling different TimeAtom objects follow ------

        private int GeneralConverter(string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            foreach (string atom in block.Atoms)
            {
                if (pattern.Contains(atom))
                {
                    int startPos = pattern.IndexOf(atom);
                    int length = atom.Length;
                    return Convert.ToInt32(dateTimeString.Substring(startPos, length));
                }
            }
            throw new Exception("No TimeAtom match");
        }

        private DateTime YearConverter(DateTime dateTimeObject, string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            int year = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime MonthConverter(DateTime dateTimeObject, string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            int month = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime DayConverter(DateTime dateTimeObject, string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            int day = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime HourConverter(DateTime dateTimeObject, string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            int hour = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime MinuteConverter(DateTime dateTimeObject, string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            int minute = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime SecondConverter(DateTime dateTimeObject, string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            int second = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, second, dateTimeObject.Millisecond);
        }

        private DateTime MilliSecondConverter(DateTime dateTimeObject, string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            int milliSecond = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, milliSecond);
        }

        // Week conversion according the ISO 8601 standard
        private DateTime WeekConverter(DateTime dateTimeObject, string dateTimeString, string pattern, DateConverter.TimeAtomBlock block)
        {
            int year = GeneralConverter(dateTimeString, pattern, _blockList[0]);
            int week = GeneralConverter(dateTimeString, pattern, block);
            int day = GeneralConverter(dateTimeString, pattern, _blockList[2]);

            Calendar calendar = CultureInfo.InvariantCulture.Calendar;

            DateTime datetime = GetWeekOne(year);
            int weekCount = 0;

            while (weekCount < week)
            {
                weekCount = calendar.GetWeekOfYear(datetime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                datetime = datetime.AddDays(7);
            }

            if (day > 1) datetime = datetime.AddDays(day - 1);

            return datetime;
        }

        // Source: http://www.boyet.com/Articles/PublishedArticles/CalculatingtheISOweeknumb.html, 2008-04-06
        private DateTime GetWeekOne(int year)
        {
            DateTime dt = new DateTime(year, 1, 4);
            int dayNumber = (int)dt.DayOfWeek;
            if (dayNumber == 0) dayNumber = 7;
            return dt.AddDays(1 - dayNumber);
        }

        
    }
    public static class DataTableExtensions
    {
        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
            {
                table.Columns[columnNames[columnIndex]].SetOrdinal(columnIndex);
            }
        }
    }
}


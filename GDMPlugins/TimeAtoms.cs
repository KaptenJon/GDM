using System;
using System.Globalization;
using GDMInterfaces;

namespace GDMPlugins
{
    public partial class DateConverter : ITool
    {
        public void Initialize()
        {
            // Important that the longest atom (for example yyyy) comes first
            TimeAtomBlock year = new TimeAtomBlock("Year", new string[] { "yyyy", "yyy", "yy" });
            TimeAtomBlock month = new TimeAtomBlock("Month", new string[] { "MM", "M" });
            TimeAtomBlock day = new TimeAtomBlock("Day", new string[] { "dd", "d" });
            TimeAtomBlock hour = new TimeAtomBlock("Hour", new string[] { "hh", "h" });
            TimeAtomBlock minute = new TimeAtomBlock("Minute", new string[] { "mm", "m" });
            TimeAtomBlock second = new TimeAtomBlock("Second", new string[] { "ss", "s" });
            TimeAtomBlock millisecond = new TimeAtomBlock("MilliSecond", new string[] { "fff", "ff", "f" });
            TimeAtomBlock week = new TimeAtomBlock("Week", new string[] { "ww", "w" });

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

            _blockList.AddRange(new TimeAtomBlock[] { year, month, day, hour, minute, second, millisecond, week });
        }

        // ------ Delegates for handling different TimeAtom objects follow ------

        private int GeneralConverter(string dateTimeString, string pattern, TimeAtomBlock block)
        {
            foreach(string atom in block.Atoms)
            {
                if(pattern.Contains(atom))
                {
                    int startPos = pattern.IndexOf(atom);
                    int length = atom.Length;
                    return Convert.ToInt32(dateTimeString.Substring(startPos, length));
                }
            }
            throw new Exception("No TimeAtom match");
        }

        private DateTime YearConverter(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block)
        {
            int year = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime MonthConverter(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block)
        {
            int month = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime DayConverter(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block)
        {
            int day = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime HourConverter(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block)
        {
            int hour = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime MinuteConverter(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block)
        {
            int minute = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, minute, dateTimeObject.Second, dateTimeObject.Millisecond);
        }

        private DateTime SecondConverter(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block)
        {
            int second = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, second, dateTimeObject.Millisecond);
        }

        private DateTime MilliSecondConverter(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block)
        {
            int milliSecond = GeneralConverter(dateTimeString, pattern, block);
            return new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, milliSecond);
        }

        // Week conversion according the ISO 8601 standard
        private DateTime WeekConverter(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block)
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
}

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class PiDataValueCorrector : ITool
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Other Tools";

        public string Description => "Corrects a value in seconds by looking at a start time and comparing that to a \"PI shift schedule\" and witdraws the number of seconds that was break-time.";

        public string Version => "1.0";

        public string Name => "Pi Data Value Corrector";

        public Image Icon => Icons.DateConverter;

        public PluginSettings GetSettings(IModel model)
        {
            return new PiDataValueCorrectorSettings(model);
        }

        public Type GetSettingsType()
        {
            return typeof(PiDataValueCorrectorSettings);
        }

        public void Apply(IModel model, PluginSettings settings_, ILog log, IStatus status)
        {
            PiDataValueCorrectorSettings settings = settings_ as PiDataValueCorrectorSettings;
            DataTable piTable = model.GetTable(settings.PiTable);
            DataTable shiftTable = model.GetTable(settings.ShiftTable);
            
            int corrected = 0;
            int total = piTable.Rows.Count;
            int notFound = 0;
            status.InitStatus("Correcting values...", total);
            bool foundShift = false;
            foreach (DataRow pirow in piTable.Rows)
            {
                DateTime errorStart = (DateTime)pirow[settings.PiDateColumn];
                string line = pirow[settings.PiLineColumn].ToString();
                double ttr = Convert.ToDouble(pirow[settings.PittrColumn]);
                
                for (int i = 0; i < shiftTable.Rows.Count; i++)
                {
                    DataRow shiftrow = shiftTable.Rows[i];
                    if (line != shiftrow[settings.ShiftLine].ToString()) continue;
                    DateTime shiftDay = (DateTime)shiftrow[settings.DayColumn];
                    DateTime shiftStart = (DateTime)shiftrow[settings.StartTime];
                    DateTime shiftStop = (DateTime)shiftrow[settings.StopTime];
                    
                    DateTime start = GetStartTime(shiftDay, shiftStart);
                    DateTime stop = GetStopTime(shiftDay, shiftStart, shiftStop);
                    
                    if (errorStart > start && errorStart < stop) 
                    {
                        // Error during this shift
                        double errorTimeInShift = stop.Subtract(errorStart).TotalSeconds;
                        if (errorTimeInShift < ttr)
                        {
                            // Error remains after this shift
                            // Start counting break time, and correct TTR value
                            double errorTimeLeft = ttr - errorTimeInShift;
                            double correctedValue = RemoveBreakTimes(shiftTable.Rows, i, line,
                                errorTimeInShift, errorTimeLeft, stop, settings);
                            corrected++;
                            pirow[settings.PittrColumn] = correctedValue;
                        }
                        foundShift = true;
                        break;
                    }
                }
                if (!foundShift)
                {
                    notFound++;
                }
                foundShift = false;
                status.Increment();
            }
            if (notFound > 0) log.Add(LogType.Warning, notFound + " values could not be checked because the shift was not found");
            log.Add(LogType.Success, "Corrected " + corrected + "/" + total + " values");
        }

        private double RemoveBreakTimes(DataRowCollection rows, int rownumber, string line,
            double errorTimeInShift, double errorTimeLeft, DateTime lastShiftEnd, PiDataValueCorrectorSettings settings)
        {
            while (true)
            {
                DataRow row = rows[++rownumber];
                if (line != row[settings.ShiftLine].ToString()) continue;
                DateTime shiftDay = (DateTime)row[settings.DayColumn];
                DateTime shiftStart = (DateTime)row[settings.StartTime];
                DateTime shiftStop = (DateTime)row[settings.StopTime];

                DateTime start = GetStartTime(shiftDay, shiftStart);
                DateTime stop = GetStopTime(shiftDay, shiftStart, shiftStop);

                double breakTime = start.Subtract(lastShiftEnd).TotalSeconds;
                double shiftLength = stop.Subtract(start).TotalSeconds;

                if (breakTime > errorTimeLeft)
                {
                    // Problem fixed during the break;
                    return errorTimeInShift;
                }
                else
                {
                    errorTimeLeft -= breakTime;

                    if (errorTimeLeft > shiftLength)
                    {
                        // Error is still not fixed during this shift
                        // Continue withdraw break time.
                        errorTimeInShift += shiftLength;
                        errorTimeLeft -= shiftLength;
                    }
                    else
                    {
                        // Error fixed in this shift
                        errorTimeInShift += errorTimeLeft;
                        return errorTimeInShift;
                    }
                }
            }
        }

        private DateTime GetStopTime(DateTime date, DateTime start, DateTime stop)
        {
            if (start > stop)
            {
                date = date.AddDays(1);
            }
            return new DateTime(date.Year, date.Month, date.Day, stop.Hour, stop.Minute, stop.Second);
        }

        private DateTime GetStartTime(DateTime date, DateTime start)
        {
            return new DateTime(date.Year, date.Month, date.Day, start.Hour, start.Minute, start.Second);
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            return;
        }
    }
}
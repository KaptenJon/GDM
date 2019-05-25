using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public partial class DateConverter : ITool
    {
        private List<TimeAtomBlock> _blockList;
        // Variables for datetime locality


        public DateConverter()
        {
            _blockList = new List<TimeAtomBlock>();
            Initialize();   // See TimeAtoms.cs
        }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return t == typeof(string);
        }

        public string ToolCategory => "Converters";

        public string Description => "Converts a string to a DateTime object";

        public string Version => "1.0";

        public string Name => "Date Converter";

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
            table.Columns.Add("Table" + j, Type.GetType("System.DateTime"));
            var formater = new DateTimeFormatInfo();
            formater.SetAllDateTimePatterns(settings.Patterns,'d');
            Parallel.ForEach(table.AsEnumerable(), d =>
            {
                DateTime datetime = new DateTime();
                
                var s = d[settings.ColumnName].ToString().TrimStart(new char[] {' '});
                if (!DateTime.TryParse(s, formater, DateTimeStyles.None, out datetime))
                    //datetime = this.ConvertToDateTime(new DateTime(),
                    //    d[settings.ColumnName].ToString().TrimStart(new char[] { ' ' }),
                    //    table.TableName, status.CurrentStatus, 0, settings.Patterns, log);   
                {
                    switch (settings.Action)
                    {
                        case DateConverterSettings.DateErrorAction.Abort:
                            table.Columns.Remove("Table" + j);
                            return;
                        case DateConverterSettings.DateErrorAction.Delete:
                            removeList.Add(d);
                            break;
                    }
                }

                else 
                    lock(this)
                        d["Table" + j] = datetime;
                status.Increment();
            });

            //    foreach (DataRow row in table.Rows)
            //    {
            //        DateTime datetime = new DateTime();

            //        if (row[settings.ColumnName].ToString() == this._previousDate)
            //            datetime = this._previousDateTime;
            //        else
            //        {
            //            try
            //            {
            //                datetime = this.ConvertToDateTime(new DateTime(),
            //                    row[settings.ColumnName].ToString().TrimStart(new char[] { ' ' }),
            //                    table.TableName, status.CurrentStatus, 0, settings.Patterns, log);

            //                this._previousDate = row[settings.ColumnName].ToString();
            //                this._previousDateTime = datetime;
            //            }
            //            catch
            //            {
            //                switch (settings.Action)
            //                {
            //                    case DateConverterSettings.DateErrorAction.Abort:
            //                        table.Columns.Remove("Table" + j);
            //                        return;
            //                    case DateConverterSettings.DateErrorAction.Delete:
            //                        removeList.Add(row);
            //                        break;
            //                }
            //            }
            //        }

            //        row["Table" + j] = datetime;
            //        status.Increment();
            //    }

            // Remove rows that could not be converted
            if (settings.Action == DateConverterSettings.DateErrorAction.Delete)
            {
                foreach (DataRow row in removeList)
                {
                    if(row != null)
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

            foreach (TimeAtomBlock block in _blockList)
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

        public class TimeAtomBlock
        {
            public delegate DateTime ConverterDelegate(DateTime dateTimeObject, string dateTimeString, string pattern, TimeAtomBlock block);

            private string _category;    // For example Year, Month...
            private string[] _atoms;     // For example yyyy, yy, yy for Year

            // A TimeAtom may require or exclude som other TimeAtom object
            private List<TimeAtomBlock> _excludes = new List<TimeAtomBlock>();
            private List<TimeAtomBlock> _requires = new List<TimeAtomBlock>();

            // Delegate for handling the TimeAtom instance
            private ConverterDelegate _converter;

            public TimeAtomBlock(string category, string[] atoms)
            {
                _category = category;
                _atoms = atoms;
            }

            public string Category
            {
                get { return _category; }
                set { _category = value; }
            }

            public string[] Atoms
            {
                get { return _atoms; }
                set { _atoms = value; }
            }

            public List<TimeAtomBlock> Excludes
            {
                get { return _excludes; }
                set { _excludes = value; }
            }

            public List<TimeAtomBlock> Requires
            {
                get { return _requires; }
                set { _requires = value; }
            }

            public ConverterDelegate Converter
            {
                get { return _converter; }
                set { _converter = value; }
            }
        }
    }
}

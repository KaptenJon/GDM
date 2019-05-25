using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    class TimeToDecimal:ITool
    {
        public TimeToDecimal()
        {
          
        }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return t == typeof(TimeSpan);
        }

        public string ToolCategory => "Converters";

        public string Description => "Converts a TimeSpan to String";

        public string Version => "1.0";

        public string Name => "Time to decimal";

        public Image Icon => Icons.DateConverter;

        public PluginSettings GetSettings(IModel model)
        {
            return new TimeToDecimalSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var settings = (TimeToDecimalSettings)pluginSettings;
            if (model.SelectedColumnName != null) settings.ColumnName = model.SelectedColumnName;
            if (model.SelectedTable != null) settings.TableName = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(TimeToDecimalSettings);
        }
        
        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            TimeToDecimalSettings settings = (TimeToDecimalSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            status.InitStatus("Converting column...", table.Rows.Count);
            int j = 0;
            while (table.Columns.Contains("Table" + j)) { j++; }
            var tempname = "Table" + j;
            table.Columns.Add(tempname, Type.GetType("System.Decimal"));
            var i = 0;
            while (i < table.Columns.Count && table.Columns[i].ColumnName != settings.ColumnName)
            {
                i++;
            }
            table.Columns[tempname].SetOrdinal(i);

            foreach (DataRow row in table.Rows)
            {
                switch (settings.Conversion)
                {
                    case TimeToDecimalSettings.ConversionFactor.Days:
                        row[tempname] = ((TimeSpan)row[settings.ColumnName]).TotalDays;
                        break;
                    case TimeToDecimalSettings.ConversionFactor.Hours:
                        row[tempname] = ((TimeSpan)row[settings.ColumnName]).TotalHours;
                        break;
                    case TimeToDecimalSettings.ConversionFactor.Minutes:
                        row[tempname] = ((TimeSpan)row[settings.ColumnName]).TotalMinutes;
                        break;
                    case TimeToDecimalSettings.ConversionFactor.Seconds:
                        row[tempname] = ((TimeSpan)row[settings.ColumnName]).TotalSeconds;
                        break;
                }
                
                status.Increment();
            }

            
            table.Columns.Remove(settings.ColumnName);
            table.Columns["Table" + j].ColumnName = settings.ColumnName;
        }
    

        
    }

    public  class TimeToDecimalSettings:PluginSettings
    {
        public enum ConversionFactor { Minutes, Seconds, Hours, Days };
        private string _tableName;
        private string _columnName;

        public TimeToDecimalSettings() { }

        [Browsable(false)]
        [XmlIgnore]
        public List<DateConverter.TimeAtomBlock> TimeAtoms { get; set; }

        [DisplayName("Convert to")]
        [Description("")]
        public ConversionFactor Conversion { get; set; }

        [DisplayName("Column Name")]
        [Description("The name of the column to apply the plugin onto.")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value;}
        }

        [DisplayName("Table Name")]
        [Description("The name of the table to apply the plugin onto.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value;}
        }
        [DisplayName("Column Type")]
        [Description("")]
        [ReadOnly(true)]
        public Type ColumnType { get; set; }

        public override async Task<bool> IsValid()
        {

            return await Factory.StartNew(() => { return true; });
            
        }

        public override string ToString()
        {
            return "";
        }

     

    }
}



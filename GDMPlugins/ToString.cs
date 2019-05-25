using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Threading.Tasks;

using GDMInterfaces;

namespace GDMPlugins
{
    public class ToString : ITool
    {
        public string Description => "Convert any column to string";

        public string Version => "1.0";

        public string Name => "String Converter";

        public Image Icon => Icons.NumericConverter;

        public PluginSettings GetSettings(IModel model)
        {
            return new StringConverterSettings();
        }

        public Type GetSettingsType()
        {
            return typeof(StringConverterSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            StringConverterSettings settings = (StringConverterSettings) pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            List<DataRow> toRemove = new List<DataRow>();
            
            // Add temporary column to existing table
            int j = 0;
            while (table.Columns.Contains("Col" + j))
            {
                j++;
            }
            table.Columns.Add("Col" + j, typeof(string));
            status.InitStatus("Converting column...", table.Rows.Count);

            foreach (DataRow row in table.Rows)
            {
                string stringData = row[settings.ColumnName].ToString();
                row["Col" + j] = stringData;
                status.Increment();
            }

            table.Columns.Remove(settings.ColumnName);
            table.Columns["Col" + j].ColumnName = settings.ColumnName;
        }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Converters";

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            if(!PluginSettings.IsInUIMode)
                return;
            StringConverterSettings settings = (StringConverterSettings)pluginSettings;
            if (model.SelectedColumnName != null) settings.ColumnName = model.SelectedColumnName;
            if (model.SelectedTable != null) settings.TableName = model.SelectedTable.TableName;
        }

    }
    public class StringConverterSettings : PluginSettings
    {
        private string _tableName = null;
        private string _columnName = null;
        
        [DisplayName("Table Name")]
        [Description("The name of the table to apply the plugin onto.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("Column Name")]
        [Description("The name of the column to apply the plugin onto.")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public override async Task<bool> IsValid()
        {
            var errorMsg = "";

            if (_tableName == null)
                errorMsg += "Table name not set. ";
            if (_columnName == null)
                errorMsg += "Column name not set. ";
            if (errorMsg != "")
                throw new PluginException(errorMsg);
            return true;
        }
    }
}

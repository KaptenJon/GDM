using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using GDMInterfaces;

namespace GDMPlugins
{
    public class DataTableToTextFile : IOutput
    {
        private Hashtable _predefinedCharacters;

        public DataTableToTextFile()
        {
            _predefinedCharacters = new Hashtable();
            _predefinedCharacters.Add(DataTableToTextFileSettings.PredefinedCharacters.NewLine, '\n');
            _predefinedCharacters.Add(DataTableToTextFileSettings.PredefinedCharacters.Space, ' ');
            _predefinedCharacters.Add(DataTableToTextFileSettings.PredefinedCharacters.Tab, '\t');
        }

        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Output";

        public string Description => "Exports a DataTable to a text file";

        public string Version => "1.0";

        public string Name => "Save to TextFile";

        public Image Icon => Icons.TextFile;

        public PluginSettings GetSettings(IModel model)
        {
            return new DataTableToTextFileSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            DataTableToTextFileSettings settings = (DataTableToTextFileSettings)pluginSettings;
            if (model.SelectedTable != null) settings.TableName = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(DataTableToTextFileSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            DataTableToTextFileSettings settings = (DataTableToTextFileSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);
            FileStream fs = null; StreamWriter sw = null; char separatorCharacter;
            status.InitStatus("Exporting DataTable...", table.Rows.Count);

            if (settings.SeparatorCharacter != DataTableToTextFileSettings.PredefinedCharacters.Other)
                separatorCharacter = (char)_predefinedCharacters[settings.SeparatorCharacter];
            else separatorCharacter = settings.OtherSeparatorCharacter;

            fs = new FileStream(settings.FilePath, FileMode.Create);
            sw = new StreamWriter(fs, Encoding.Unicode);
            string rowStr = "";
            foreach (DataColumn column in table.Columns)
            {
                // Remove spaces, if any, at the beginning and the end
                string cell = column.ColumnName.TrimStart(new char[] { ' ' }).TrimEnd(new char[] { ' ' });

                if (string.IsNullOrEmpty(cell)) rowStr += "null";
                else rowStr += cell;

                rowStr += separatorCharacter;
                
            }
            sw.WriteLine(rowStr);
            sw.Flush();


            foreach (DataRow row in table.Rows)
            {
                rowStr = "";

                foreach (DataColumn column in table.Columns)
                {
                    // Remove spaces, if any, at the beginning and the end
                    string cell = row[column].ToString().TrimStart(new char[] { ' ' }).TrimEnd(new char[] { ' ' });

                    if (cell == null || cell == "") rowStr += "null";
                    else rowStr += cell;

                    rowStr += separatorCharacter;
                }

                status.Increment();
                sw.WriteLine(rowStr);
                sw.Flush();
            }

            sw.Close();
            log.Add(LogType.Success, table.Rows.Count + " rows written to file");
        }

        public Tag Tags => null;

        public string GetJobDescription(PluginSettings s)
        {
            return ((DataTableToTextFileSettings)s).Description;
        }

        public object GetDynamicSettings(PluginSettings s)
        {
            return ((DataTableToTextFileSettings)s).GetDynamicSettings();
        }
    }
}

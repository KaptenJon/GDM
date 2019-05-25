using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using GDMInterfaces;

namespace GDMPlugins
{
    public class TextFile : IInput
    {
        private Hashtable _predefinedCharacters;

        public TextFile()
        {
            _predefinedCharacters = new Hashtable();
            _predefinedCharacters.Add(TextFileSettings.RowEndChar.NewLine, '\r');
            _predefinedCharacters.Add(TextFileSettings.RowEndChar.Tab, '\t');
            _predefinedCharacters.Add(TextFileSettings.RowEndChar.Space, ' ');
        }

        public string Description => "Convert a plain text file into a DataTable";

        public string Version => "1.0";

        public string Name => "Text file";

        public Image Icon => Icons.TextFile;

        public Type GetSettingsType()
        {
            return typeof(TextFileSettings);
        }

        public PluginSettings GetSettings(IModel model)
        {

            return new TextFileSettings();
        }

        

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            TextFileSettings settings = (TextFileSettings)pluginSettings;
            string fileRow = ""; int rowsAdded = 0; DataTable datatable = null;

            // Encoding 1252 supports å, ä and ö
            StreamReader sr = new StreamReader(settings.FileName, Encoding.GetEncoding(1252));

                datatable = model.CreateTable();
                datatable.Columns.Add("Col1", typeof(string));

            // Read every row from file
                while ((fileRow = sr.ReadLine()) != null)
                {
                    string[] rows;

                    // Split every file row into subrows
                    if (settings.RowEndCharSelection == TextFileSettings.RowEndChar.Other)
                        rows = fileRow.Split(settings.OwnEndChar);
                    else rows = fileRow.Split((char)_predefinedCharacters[settings.RowEndCharSelection]);

                    // Add every subrow to the newly created table
                    foreach (string row in rows)
                    {
                        datatable.Rows.Add(new object[] { row });
                        rowsAdded++;
                    }
                }   

            sr.Close();
            log.Add(LogType.Success, rowsAdded + " rows read");
        }

        public string GetJobDescription(PluginSettings s)
        {
            return ((TextFileSettings)s).Description;
        }

        public object GetDynamicSettings(PluginSettings s)
        {
            return ((TextFileSettings)s).GetDynamicSettings();
        }
    }
}
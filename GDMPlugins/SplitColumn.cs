using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class SplitColumn : ITool
    {
        private Hashtable _predefinedCharacters;

        public SplitColumn()
        {
            _predefinedCharacters = new Hashtable();
            _predefinedCharacters.Add(SplitColumnSettings.SeparatorChar.NewLine, '\r');
            _predefinedCharacters.Add(SplitColumnSettings.SeparatorChar.Space, ' ');
            _predefinedCharacters.Add(SplitColumnSettings.SeparatorChar.Tab, '\t');
        }

        public string Description => "Split a column into several columns";

        public string Version => "1.0";

        public string Name => "Split Column";

        public PluginSettings GetSettings(IModel model)
        {
            return new SplitColumnSettings();
        }

        public Type GetSettingsType()
        {
            return typeof(SplitColumnSettings);
        }

        public Image Icon => Icons.SplitColumn;

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool ShowSettingsObject
        {
            get { throw new NotImplementedException(); }
        }

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Other Tools";

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            SplitColumnSettings settings = (SplitColumnSettings)pluginSettings;
            if (model.SelectedTable != null) settings.TableName = model.SelectedTable.TableName;
            if (model.SelectedColumnName != null) settings.ColumnName = model.SelectedColumnName;
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            SplitColumnSettings settings = (SplitColumnSettings)pluginSettings;
            DataTable datatable = model.GetTable(settings.TableName);

            status.InitStatus("Splitting column...", datatable.Rows.Count);
            List<string> newColumns = new List<string>();
            int nbrOfColumns = 0;

            foreach (DataRow row in datatable.Rows)
            {
                string rowString = row[settings.ColumnName].ToString(); string[] rowSubStrings;

                if (settings.SeparatorCharSelection == SplitColumnSettings.SeparatorChar.Other)
                    rowSubStrings = rowString.Split(settings.OwnSeparatorChar);
                else rowSubStrings = rowString.Split((char)_predefinedCharacters[settings.SeparatorCharSelection]);

                status.Increment();

                // Adds columns after hand if some row would have a different amount of separator chars
                if (rowSubStrings.Length > nbrOfColumns)
                {
                    int nbrOfLoops = rowSubStrings.Length - nbrOfColumns;

                    for (int i = 0; i < nbrOfLoops; i++)
                    {
                        int nbr = 1;
                        while (datatable.Columns.Contains("Col" + nbr)) { nbr++; }

                        DataColumn newColumn = new DataColumn("Col" + nbr, typeof(string));
                        datatable.Columns.Add(newColumn);
                        newColumns.Add(newColumn.ColumnName);
                        nbrOfColumns++;
                    }
                }

                for (int i = 0; i < rowSubStrings.Length; i++)
                    row[newColumns[i]] = rowSubStrings[i];
            }

            // Remove the column which was split
            datatable.Columns.Remove(settings.ColumnName);
            log.Add(LogType.Success, "the column was split into " + nbrOfColumns + " columns");
        }
    }
}

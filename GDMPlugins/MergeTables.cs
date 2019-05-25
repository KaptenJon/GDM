using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using GDMInterfaces;

namespace GDMPlugins
{
    public class MergeTables : ITool
    {
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

        public string Description => "Merge two tables";

        public string Version => "1.0";

        public string Name => "Merge Tables";

        public Image Icon => Icons.MergeTables;

        public PluginSettings GetSettings(IModel model)
        {
            return new MergeTablesSettings();
        }

        public Type GetSettingsType()
        {
            return typeof(MergeTablesSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            MergeTablesSettings settings = (MergeTablesSettings)pluginSettings;
            DataTable table1 = model.GetTable(settings.Table1Name);
            DataTable table2 = model.GetTable(settings.Table2Name);

            if (table1.Equals(table2))
            {
                log.Add(LogType.Warning, "No merge operation executed because tables are equal");
                return;
            }

            // Table2 column name as key returns the new corresponding DataColumn in Table1
            Hashtable columnsFromTable2 = new Hashtable();

            // Adds all columns except key column from table2
            foreach (DataColumn column in table2.Columns)
            {
                if (column.ColumnName != settings.Table2KeyColumn)
                {
                    // Find suitable name
                    string columnName = null;
                    if (!table1.Columns.Contains(column.ColumnName)) columnName = column.ColumnName;
                    else
                    {
                        int i = 1;
                        while (table1.Columns.Contains("Col" + i)) i++;
                        columnName = "Col" + i;
                    }

                    DataColumn newColumn = new DataColumn(columnName, column.DataType);
                    columnsFromTable2.Add(column.ColumnName, newColumn);
                    table1.Columns.Add(newColumn);
                }
            }

            Regex table1Reg = new Regex(settings.Table1Regex);
            Regex table2Reg = new Regex(settings.Table2Regex);

            foreach (DataRow table2Row in table2.Rows)
            {
                DataRow corrRow = null;  // corresponding row in table1

                foreach (DataRow table1Row in table1.Rows)
                {
                    Match p = table1Reg.Match(table1Row[settings.Table1KeyColumn].ToString());
                    Match c = table2Reg.Match(table2Row[settings.Table2KeyColumn].ToString());

                    if (p.Success && c.Success) // key match
                    {
                        if (p.Value == c.Value)
                        {
                            corrRow = table1Row;
                            break;
                        }
                    }
                }

                if (corrRow == null)  // no key match so add the key instead
                {
                    corrRow = table1.NewRow();
                    corrRow[settings.Table1KeyColumn] = table2Row[settings.Table2KeyColumn];
                    table1.Rows.Add(corrRow);
                }

                foreach (DataColumn column in table2.Columns)
                {
                    if (column.ColumnName != settings.Table2KeyColumn)
                        corrRow[(DataColumn)columnsFromTable2[column.ColumnName]] = table2Row[column];
                }
            }
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
        }
    }
}

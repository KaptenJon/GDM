using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using GDMInterfaces;

namespace GDMPlugins
{
    public class CreateStringData : ITool
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Create Data";

        public string Description => "Plugin for creating a new data column in a table by combining other column values and simple arithmetic.";

        public string Version => "1.0";

        public string Name => "Create String Data Column";

        public Image Icon => Icons.CreateData;

        public PluginSettings GetSettings(IModel model)
        {
            return new CreateStringDataSettings(model);
        }

        public Type GetSettingsType()
        {
            return typeof(CreateStringDataSettings);
        }

        private string GetRowString(DataRowCollection rows, List<CreateStringDataSettings.StringBase> list, int rowindex)
        {
            StringBuilder sb = new StringBuilder();
            foreach (CreateStringDataSettings.StringBase s in list)
            {
                CreateStringDataSettings.ColumnValue column = s as CreateStringDataSettings.ColumnValue;
                if (column == null)
                {
                    sb.Append(((CreateStringDataSettings.StringValue)s).String);
                }
                else
                {
                    sb.Append(GetColumnValue(column,rows,rowindex));
                }
            }
            return sb.ToString();
        }

        private string GetColumnValue(CreateStringDataSettings.ColumnValue col, DataRowCollection rows, int rowindex)
        {
            switch (col.Row)
            {
                case CreateStringDataSettings.RowType.CurrentRow:
                    return rows[rowindex][col.Column].ToString();
                case CreateStringDataSettings.RowType.PreviousRow:
                    if (rowindex - 1 >= 0) return rows[rowindex - 1][col.Column].ToString();
                    else return "";
                case CreateStringDataSettings.RowType.NextRow:
                    if (rowindex + 1 < rows.Count) return rows[rowindex + 1][col.Column].ToString();
                    else return "";
            }
            return "";
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            CreateStringDataSettings settings = (CreateStringDataSettings)pluginSettings;
            DataTable table = model.GetTable(settings.Table);
            status.InitStatus("Creating column: " + settings.Columnname, table.Rows.Count);

            table.Columns.Add(settings.Columnname, typeof(string));

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string rowresult = GetRowString(table.Rows, settings.StringList, i);
                table.Rows[i][settings.Columnname] = rowresult;
                status.Increment();
            }
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
        }
    }
}


using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RowsToColumns : ITool
    {
        public RowsToColumns()
        {
        }

        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Other Tools";

        public string Description => "Goes through each row and looks at the specified a column";

        public string Version => "1.0";

        public string Name => "Rows to columns";

        public Image Icon => Icons.RowsToColumns;

        public PluginSettings GetSettings(IModel model)
        {
            return new RowsToColumnsSettings(model);
        }

        public void UpdateSettings(PluginSettings settings, IModel model)
        {
            ((RowsToColumnsSettings)settings).TableName = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(RowsToColumnsSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RowsToColumnsSettings settings = pluginSettings as RowsToColumnsSettings;
            DataTable oldtable = model.GetTable(settings.TableName);
            Type type = oldtable.Columns[settings.DataColumn].DataType;
            Type keyType = oldtable.Columns[settings.KeyColumn].DataType;
            DataTable newTable = model.CreateTable();
            newTable.PrimaryKey = new DataColumn[] { newTable.Columns.Add(settings.KeyColumn, keyType) };

            status.InitStatus("Rows to columns...", oldtable.Rows.Count);

            foreach (DataRow row in oldtable.Rows)
            {
                string name = row[settings.NameColumn].ToString();

                if (!newTable.Columns.Contains(name)) newTable.Columns.Add(name, type);

                if (newTable.Rows.Contains(row[settings.KeyColumn])) newTable.Rows.Find(row[settings.KeyColumn])[name] = row[settings.DataColumn];
                else
                {
                    DataRow newrow = newTable.NewRow();
                    newrow[settings.KeyColumn] = row[settings.KeyColumn];
                    newrow[name] = row[settings.DataColumn];
                    newTable.Rows.Add(newrow);
                }

                status.Increment();
            }
        }
    }
}
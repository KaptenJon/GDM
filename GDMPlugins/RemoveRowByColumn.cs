using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveRowByColumn : ITool
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t) => true;

        public string ToolCategory => "Filter";

        public string Description => "Removes a row by specifying a pattern that matches a certain column value";

        public string Version => "1.0";

        public string Name => "Text Filter";
        public Image Icon
        {
            get { return Icons.RemoveRow; }
        }

        public PluginSettings GetSettings(IModel model)
        {
            RemoveRowByColumnSettings settings = new RemoveRowByColumnSettings(model);
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings settings, IModel model)
        {
            if (model.SelectedTable != null)
                ((RemoveRowByColumnSettings)settings).Table = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(RemoveRowByColumnSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RemoveRowByColumnSettings settings = pluginSettings as RemoveRowByColumnSettings;
            DataTable table = model.GetTable(settings.Table);

            if (settings.TableAction == RemoveRowByColumnSettings.Action.Keep)
            {
                table = table.Copy();
                model.AddTable(table);
            }

            Regex regex = new Regex(settings.RegularExpression);
            List<DataRow> toDelete = new List<DataRow>();

            status.InitStatus("Removing rows by column values...", table.Rows.Count);

            foreach (DataRow row in table.Rows)
            {
                Match m = regex.Match(row[settings.Column].ToString());

                if (m.Success)
                {
                    if (settings.RowAction == RemoveRowByColumnSettings.Action.Delete)
                        toDelete.Add(row);
                }
                else
                {
                    if (settings.RowAction == RemoveRowByColumnSettings.Action.Keep)
                        toDelete.Add(row);
                }

                status.Increment();
            }

            log.Add(LogType.Success, toDelete.Count + " rows were removed");
            foreach (DataRow d in toDelete) table.Rows.Remove(d);
        }
    }
}
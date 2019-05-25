using System;
using System.Data;
using System.Drawing;
using GDMInterfaces;

namespace GDMPlugins
{
    internal class RemoveColumnRange : ITool
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;



        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Remove";

        public string Description => "Removes a range of Column";

        public string Version => "1.0";

        public string Name => "Remove Column Range";

        public Image Icon => Icons.trash;

        private bool ThumbnailCallback()
        {
            return true;
        }

        public PluginSettings GetSettings(IModel model)
        {
            RemoveColumnsSettings settings = new RemoveColumnsSettings(model);
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings settings, IModel model)
        {
            RemoveColumnsSettings s = settings as RemoveColumnsSettings;
            if (s != null) s.Update(model);
            return;
        }

        public Type GetSettingsType()
        {
            return typeof(RemoveColumnsSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RemoveColumnsSettings rSettings = pluginSettings as RemoveColumnsSettings;
            if (rSettings == null)
                return;
            if (rSettings.ColumnList?.Length > 0)
            {
                DataTable dataTable = model.GetTable(rSettings.TableName);
                foreach (var s in rSettings.ColumnList)
                {
                    dataTable.Columns.Remove(s);
                }
                return;
            }

            if (rSettings.StartColumn == null) return;

            DataTable dt = model.GetTable(rSettings.TableName);
            int startcolumn = dt.Columns.IndexOf(rSettings.StartColumn);


            if (rSettings.NumberOfColumns == -1)
                rSettings.NumberOfColumns = dt.Columns.Count - startcolumn;
            if (rSettings.NumberOfColumns > dt.Columns.Count - startcolumn)
                rSettings.NumberOfColumns = dt.Columns.Count - startcolumn;
            status.InitStatus("Removing Columns", rSettings.NumberOfColumns);
            for (int i = startcolumn; i < startcolumn + rSettings.NumberOfColumns; i++)
            {
                dt.Columns.RemoveAt(startcolumn);
                status.Increment();
            }

        }
    }
}

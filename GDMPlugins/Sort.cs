using System;
using System.Data;
using System.Drawing;
using GDMInterfaces;

namespace GDMPlugins
{
    public class Sort:ITool
    {
        #region Implementation of IPlugin

        private string _description = "Sorts a column";

        private string _version = "0.5";

        private string _name = "Sorter";

        private bool _needColumnSelected = true;
        private bool _needTableSelected = true;

        private string _toolCategory = "Other Tools";

        public PluginSettings GetSettings(IModel model)
        {
            var settings = new Settings(model);
            return settings;
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            Settings settings = (Settings)pluginSettings;
            settings.ColumnName = model.SelectedColumnName;
            settings.TableName = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(Settings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {

            log.Add(LogType.Note, "Sort Starting");
            
            Settings.Direction sortdir = Settings.Direction.Descending;
            Settings s = pluginSettings as Settings;
            if (s == null  )
            {
                log.Add(LogType.Error, "error plugin _columnSettings");
                return;
            }
            DataTable table =  model.GetTable(s.TableName);
            
            if(table == null || table.Columns[s.ColumnName] == null)
            {
                log.Add(LogType.Error, "No tabel or column selected");
                return;
            }
            status.InitStatus("Sorts", table.Rows.Count);
            sortdir = s.SortDirection;

            DataTable temp = table.Copy();
            
            temp.DefaultView.Sort = s.ColumnName + (sortdir == Settings.Direction.Descending?" DESC":"") ;
            table.Rows.Clear();
            foreach (DataRowView row in temp.DefaultView)
            {
                table.ImportRow(row.Row);
                status.Increment();
            }
            
            
        }

        public string Description => _description;

        public string Version => _version;

        public string Name => _name;

        public Image Icon => Icons.sort;

        private bool ThumbnailCallback()
        {
            return true;
        }


        #endregion

        #region Implementation of ITool

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public bool NeedColumnSelected => _needColumnSelected;

        public bool NeedTableSelected => _needTableSelected;

        public string ToolCategory => _toolCategory;

        #endregion
    }
}

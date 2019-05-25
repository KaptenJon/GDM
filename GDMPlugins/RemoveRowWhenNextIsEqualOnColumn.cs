using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveRowWhenNextIsEqualOnColumn :ITool
    {
        public string Description { get; } = "Removes Duplicates";
        public string Version { get; } = "1.0";
        public string Name { get; } = "Remove Row If Previous Is Equal";
        public Image Icon { get; }
        public PluginSettings GetSettings(IModel model)
        {
          return new RemoveRowWhenNextIsEqualOnColumnSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var settings = pluginSettings as RemoveRowWhenNextIsEqualOnColumnSettings;
            if(settings == null)
                return;
            settings.Column = model.SelectedColumnName;
            settings.Table = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(RemoveRowWhenNextIsEqualOnColumnSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var settings = pluginSettings as RemoveRowWhenNextIsEqualOnColumnSettings;
            var table = model.GetTable(settings.Table);
            for(int i=1; i<table.Rows.Count;i++)
            {
                if(table.Rows[i][settings.Column].ToString() == table.Rows[i-1][settings.Column].ToString())
                    table.Rows.RemoveAt(i);
                
            }
        }

        public bool NeedColumnSelected { get; } = true;
        public bool NeedTableSelected { get; } = true;
        public bool AcceptsDataType(Type t)
        {
            if (typeof(string) == t)
                return true;
            return false;
        }

        public string ToolCategory { get; } = "Filter";

    }

    public class RemoveRowWhenNextIsEqualOnColumnSettings : PluginSettings
    {
        public override Task<bool> IsValid()
        {
            return Factory.StartNew(()=> Column!=null&&Table!=null);
        }

        public string Column { get; set; }
        public string Table { get; set; }
    }
}

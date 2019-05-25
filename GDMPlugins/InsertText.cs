using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDMInterfaces;

namespace GDMPlugins
{
    public class InsertText:ITool
    {
        public string Description => "Insert text anywhere";

        public string Version => "1.0";

        public string Name => "Insert Text";

        public Image Icon => Icons.insert;

        public PluginSettings GetSettings(IModel model)
        {
            return new InsertTextSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            InsertTextSettings settings = (InsertTextSettings)pluginSettings;
            if (model.SelectedTable != null) settings.Table = model.SelectedTable.TableName;
            if (model.SelectedColumnName != null) settings.Column = model.SelectedColumnName;
        }

        public Type GetSettingsType()
        {
            return typeof(InsertTextSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var settings = pluginSettings as InsertTextSettings;
            if (settings == null)
                return;
            var table =  model.GetTable(settings.Table);
            foreach (DataRow row in table.Rows)
            {
                string t = row[settings.Column] as string;
                if(t !=null)
                try
                {
                    var pos = t.Length > settings.InsertPosition ? settings.InsertPosition : t.Length;

                    row[settings.Column] = t.Insert(pos, settings.Text);
                }
                catch
                { }
            }
        }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return t == typeof(string);
        }

        public string ToolCategory => "Modify";
    }

    public class InsertTextSettings : PluginSettings
    {

        [Description("Starting at 0 counting from the start of the string.")]
        [DisplayName("1.1 Table")]
        [ReadOnly(true)]
        public string Table { get; set; }

        [Description("Starting at 0 counting from the start of the string.")]
        [DisplayName("1.2 Column")]
        [ReadOnly(true)]
        public string Column { get; set; }

        [Description("Starting at 0 counting from the start of the string.")]
        [DisplayName("2 Insert Position")]
        public int InsertPosition { get; set; }

        [DisplayName("3 Text to insert")]
        public string Text { get; set; }

        public override Task<bool> IsValid()
        {
            return Factory.StartNew(()=> Table != null && Column != null && InsertPosition > -1);
        }
    }
}

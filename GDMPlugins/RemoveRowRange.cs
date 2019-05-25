using System;
using System.Data;
using System.Drawing;
using GDMInterfaces;

namespace GDMPlugins
{
    internal class RemoveRowRange : ITool
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Remove";

        public string Description => "Removes a range of row";

        public string Version => "1.0";

        public string Name => "Remove Row Range";

        public Image Icon => Icons.trash;

        private bool ThumbnailCallback()
        {
            return true;
        }

        public PluginSettings GetSettings(IModel model)
        {
            RemoveRowsSettings settings = new RemoveRowsSettings();
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings settings, IModel model)
        {
            RemoveRowsSettings s = settings as RemoveRowsSettings;
            if (s != null) s.Update(model);
            return;
        }

        public Type GetSettingsType()
        {
            return typeof (RemoveRowsSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RemoveRowsSettings rSettings = pluginSettings as RemoveRowsSettings;

            if (rSettings == null) return;
            status.InitStatus("Removing rows", rSettings.EndRow +1 - rSettings.StartRow);
            DataTable d = model.GetTable(rSettings.TableName);
            for (int i = rSettings.StartRow; i <= rSettings.EndRow; i++)
            {
                d.Rows.RemoveAt(rSettings.StartRow);
                status.Increment();
            }
            
        }
    }
}

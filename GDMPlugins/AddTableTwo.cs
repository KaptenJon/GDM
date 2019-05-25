using System;
using System.Data;
using System.Drawing;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class AddTableTwo : ITool
    {
        
        [XmlIgnore]
        private bool _firstcolumnturn = true;

        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Other Tools";

        public string Description => "Add a table to the other table";

        public string Version => "1.0";

        public string Name => "Add two tables";

        public Image Icon => Icons.Merge;

        private bool ThumbnailCallback()
        {
            return true;
        }

        
        public PluginSettings GetSettings(IModel model)
        {
            AddTableTwoSettings settings = new AddTableTwoSettings();
            UpdateSettings(settings, model);
            return settings;
        }

        public void UpdateSettings(PluginSettings settings, IModel model)
        {
            AddTableTwoSettings s = settings as AddTableTwoSettings;
            if (s != null)
            {
                if (_firstcolumnturn && s.Table2Name != model.SelectedTable.TableName)
                {
                    s.Table1Name = model.SelectedTable.TableName;
                    _firstcolumnturn = !_firstcolumnturn;
                }
                else if (!_firstcolumnturn && s.Table1Name != model.SelectedTable.TableName)
                {
                    s.Table2Name = model.SelectedTable.TableName;
                    _firstcolumnturn = !_firstcolumnturn;
                }

            }
        }

        public Type GetSettingsType()
        {
            return typeof(AddTableTwoSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            AddTableTwoSettings rSettings = pluginSettings as AddTableTwoSettings;

            if (rSettings == null) return;
            DataTable d1 = model.GetTable(rSettings.Table1Name);
            DataTable d2 = model.GetTable(rSettings.Table2Name);
            if (d2 == null || d1 == null || d2 == d1)
            {
                log.Add(LogType.Error, "did not find tables");
                return;
            }
            int columns = d2.Columns.Count < d1.Columns.Count ? d2.Columns.Count : d1.Columns.Count;

            status.InitStatus("Adding tables", d2.Rows.Count);
            
            foreach(DataRow dr2 in d2.Rows)
            {
                DataRow r = d1.NewRow();

                for (int i = 0; i < columns; i++)
                {
                    if (d1.Columns[i].DataType == d2.Columns[i].DataType)
                    {
                        r[i] = dr2[i];
                    }
                    else
                    {
                        try
                        {
                            r[i] = dr2[i].ToString();
                        }
                        catch
                        {}
                       
                    }
                    
                }

                d1.Rows.Add(r);
                status.Increment();
            }
        }
    }
}


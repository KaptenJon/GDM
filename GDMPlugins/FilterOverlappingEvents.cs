using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDMInterfaces;
using GDMPlugins;

namespace Statistics
{
    public class FilterOverlappingEvents:ITool
    {

        

        

        public string Description => "Filter rows for time that overlapps";

        public string Version => "1.0";

        public string Name => "Filter Overlapping Date and Time";

        public Image Icon => Icons.overlappingtime;

        public PluginSettings GetSettings(IModel model)
        {
            return new FilterOverlappingEventsSettings() {Model = model};
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var setting = pluginSettings as FilterOverlappingEventsSettings;
            if (setting == null)
                return;
            setting.Model = model;
            setting.TargetTable = model.SelectedTable.TableName;

        }

        public Type GetSettingsType()
        {
            return  typeof(FilterOverlappingEventsSettings); ;
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var setting = pluginSettings as FilterOverlappingEventsSettings;
            if(setting == null)
                return;
            DataTable table =  model.GetTable(setting.TargetTable);
            var groups = from p in table.AsEnumerable() orderby p[setting.StartDateColumn] group p by p[setting.GroupColumn] into g select g;
            var newtable = table.Clone();

            foreach (var g in groups)
            {
                DateTime start = DateTime.MinValue;
                DateTime end = DateTime.MaxValue;
                DataRow currow = null;
                foreach (var d in g)
                {
                    if (d[setting.EndDateColumn] is DateTime && d[setting.StartDateColumn] is DateTime)
                    {
                        var rowend = (DateTime) d[setting.EndDateColumn];
                        var rowstart = (DateTime) d[setting.EndDateColumn];
                        if (start == DateTime.MinValue)
                        {
                            start = rowstart;
                            end = rowend;
                            currow = d;

                        }
                        else
                        {
                            if (start == rowstart && end == rowend)
                            {
                                
                            }
                            else if (start== rowstart && end < rowend)
                            {
                                end = rowend;
                                currow = d;
                            }
                            else if (end >= rowstart && end < rowend)
                            {
                                end = rowend;
                                currow[setting.EndDateColumn] = rowend;
                            }

                            else if (end < rowstart)
                            {
                                start = rowstart;
                                end = rowend;
                                newtable.ImportRow(currow);
                                currow = d;
                            }
                            
                        }

                    }
                } 
            }
            model.AddTable(newtable);

        }

        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Filter";
    }
}

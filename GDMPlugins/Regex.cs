using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;


namespace GDMPlugins
{
    public class RegexTool: ITool
    {
        public string Description { get; } = "Changes string values using Regex";

        public string Version { get; } = "1.0";

        public string Name { get; } = "Modify Text With Regex";

        public Image Icon { get; } = Icons.RenameColumn;

        public PluginSettings GetSettings(IModel model)
        {
            var set = new RegexSetting();
            UpdateSettings(set,model);
            return set;
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var set = pluginSettings as RegexSetting;
            if(set == null || model.SelectedTable ==null)
                return;

            set.Column = model.SelectedColumnName;

           set.Table = model.SelectedTable.TableName;
            set.Model = model;
        }

        public Type GetSettingsType()
        {
            return typeof(RegexSetting);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var set = pluginSettings as RegexSetting;
            if(set == null ) return;
            var i = model?.GetTable(set.Table).Columns[set.Column]?.Ordinal;
            if (i == null) return;
            var rows = model?.GetTable(set.Table)?.Rows;
            if(rows == null )return;

            if (set.RegexMode == RegexSetting.RegexModes.Remove)
            {
                foreach (DataRow row in rows)
                {
                    if(row[i.Value] != DBNull.Value)
                        row[i.Value] = Regex.Replace(row[i.Value].ToString(), set.FindRegex, "",RegexOptions.CultureInvariant);

                }
                
            }
            else
            {
                foreach (DataRow row in rows)
                {
                    if (row[i.Value] != DBNull.Value)
                        row[i.Value] = Regex.Replace((string)row[i.Value], set.FindRegex, set.ReplacementTextRegex??"", RegexOptions.CultureInvariant);

                }
            }
        }

        public bool NeedColumnSelected { get; } = true;

        public bool NeedTableSelected { get; } = true;

        public bool AcceptsDataType(Type t)
        {
            return typeof(String).IsAssignableFrom(t);
        }

        public string ToolCategory { get; } = "Modify";
    }
    public class RegexSetting:PluginSettings
    {
        public enum RegexModes { Replace, Remove}
        public string Table { get;set; }
        public string Column { get; set; }
        public string FindRegex { get; set; }
        public string ReplacementTextRegex { get; set; }
        public RegexModes RegexMode { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public IModel Model { get; set; }

        public override Task<bool> IsValid()
        {
            return Factory.StartNew(() =>
            {
                var tab = Model?.GetTable(Table);
                if (tab == null)
                    return false;
                return tab.Columns[Column]?.DataType.IsAssignableFrom(typeof (string)) ?? false;
            });
        }
    }
}

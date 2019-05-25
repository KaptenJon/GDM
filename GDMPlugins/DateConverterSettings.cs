using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class DateConverterSettings : PluginSettings
    {
        public enum DateErrorAction { Delete, Abort };
        private string[] _patterns;
        private string _tableName;
        private string _columnName;
        private DateErrorAction _action;
        private List<DateConverter.TimeAtomBlock> _timeAtoms;

        public DateConverterSettings() { }

        public DateConverterSettings(List<DateConverter.TimeAtomBlock> timeAtoms)
        {
            _timeAtoms = timeAtoms;
        }

        [DisplayName("Settings")]
        [Description("Click the button to bring forth the settings panel.")]
        [Editor(typeof(DateIntervalEditor_), typeof(UITypeEditor))]
        [ReadOnly(true)]
        [XmlIgnore]
        public DateConverterSettings Settings
        {
            get { return this; }
            set { }
        }

        [Browsable(false)]
        [XmlIgnore]
        public List<DateConverter.TimeAtomBlock> TimeAtoms
        {
            get { return _timeAtoms; }
            set { _timeAtoms = value; }
        }

        [DisplayName("Action on Error")]
        [Description("Abort: the data conversion of the specific column is terminated.\nDelete: values that cannot be converted to specified datatype will be removed.")]
        public DateErrorAction Action
        {
            get { return _action; }
            set
            {
                    _action = value;
            }
        }

        [DisplayName("Column Name")]
        [Description("The name of the column to apply the plugin onto.")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value;}
        }

        [DisplayName("Table Name")]
        [Description("The name of the table to apply the plugin onto.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value;}
        }

        [Browsable(false)]
        public string[] Patterns
        {
            get { return _patterns; }
            set
            {
                    _patterns = value;
            }
        }

        public override async Task<bool> IsValid()
        {
            if (_patterns == null)
            {
                throw new PluginException("No patterns specified. ");
            }
            
            return true;
        }

        public override string ToString()
        {
            string s = "";
            if(Patterns!=null)
                s = Patterns.Aggregate(s, (current, pattern) => current + (pattern + "; "));
            return s;
        }

        private class DateIntervalEditor_ : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                DateConverterSettings settings = (DateConverterSettings)value;
                DateConverterEditor editor = new DateConverterEditor(settings);
                editor.ShowDialog();
                return base.EditValue(context, provider, settings);
            }
        }
    }
}

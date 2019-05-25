using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class DateIntervalSettings : PluginSettings
    {
        public enum DateFilterMode { LowerBound, UpperBound, Interval };

        private string _tableName = "";
        private string _columnName = "";
        private string _minimum;
        private string _maximum;
        private DateFilterMode _mode;

        [DisplayName("Settings")]
        [Description("Click the button to bring forth the settings panel.")]
        [Editor(typeof(DateIntervalEditor_), typeof(UITypeEditor))]
        [ReadOnly(true)]
        [XmlIgnore]
        public DateIntervalSettings Settings
        {
            get { return this; }
            set { }
        }

        [DisplayName("Mode of Operation")]
        [Description("Specifies how the filtering will be executed.")]
        [Browsable(false)]
        public DateFilterMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        [DisplayName("Table Name")]
        [Description("The name of the table to apply the plugin onto.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("Column Name")]
        [Description("The name of the column to apply the plugin onto.")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        [Browsable(false)]
        public String Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
            }
        }

        [XmlIgnore]
        public DateTime MaximumDateTime
        {
            get
            {
                DateTime t;
                if(DateTime.TryParse(Maximum, out t))
                    return t;
                return DateTime.MaxValue;
            }
        } 

        [Browsable(false)]
        public String Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
            }
        }
        [XmlIgnore]
        public DateTime MinimumDateTime
        {
            get
            {
                DateTime t;
                if (DateTime.TryParse(Minimum, out t))
                    return t;
                return DateTime.MinValue;
            }
        }

        public override async Task<bool> IsValid()
        {
            var errorMsg = "";
            if (Mode == DateFilterMode.LowerBound && MinimumDateTime == DateTime.MinValue)
            {
                errorMsg = "Lower bound not specified. ";
                
            }
            else if (Mode == DateFilterMode.Interval && (MinimumDateTime == DateTime.MinValue || MaximumDateTime == DateTime.MaxValue))
            {
                errorMsg = "Lower and/or upper bound not specified. ";
               
            }
            else if (Mode == DateFilterMode.UpperBound && MaximumDateTime == DateTime.MinValue)
            {
                errorMsg = "Upper bound not specified. ";
                
            }
            if (errorMsg != "")
                throw new PluginException(errorMsg);
            return true;
            
        }

        public override string ToString()
        {
            return "";
        }

        private class DateIntervalEditor_ : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                DateIntervalSettings settings = (DateIntervalSettings)value;
                DateIntervalEditor editor = new DateIntervalEditor(settings);
                editor.ShowDialog();
                return base.EditValue(context, provider, settings);
            }
        }
    }
}

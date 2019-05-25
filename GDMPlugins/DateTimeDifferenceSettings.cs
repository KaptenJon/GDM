using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    /// <summary>
    /// MAde by Jon Andersson
    /// </summary>
    public class DateTimeDifferenceSettings : PluginSettings
    {
        public enum TimeUnit { Hours, Minutes, Seconds, Milliseconds };
        public enum SortingOption {Increasing, Decreasing}
        private string _table;
        private SortingOption _sorting;
        private string _dateColumn;
        private List<string> _groupingColumns;
        private string _newColumn;
        private TimeUnit _timeFormat;
        private IModel _model;

        public DateTimeDifferenceSettings(){}

        public DateTimeDifferenceSettings(IModel model)
        {
            _model = model;
            _groupingColumns = null;
        }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        [DisplayName("Sorting")]
        [Description("Specifies how the dates appear. Choose 'Increasing' if the first row of the table holds the smallest date and the last row holds the largest date. Choose 'Decreasing' for the other way around.")]
        public SortingOption Sorting
        {
            get { return _sorting; }
            set { _sorting = value; }
        }

        [DisplayName("Table Name")]
        [Description("The name of the table to calculate a difference upon.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _table; }
            set { _table = value; }
        }

        [DisplayName("Date Column")]
        [Description("The Column to calculate the date difference upon.")]
        [ReadOnly(true)]
        public string DateColumn
        {
            get { return _dateColumn; }
            set { _dateColumn = value; }
        }

        [DisplayName("Grouping Columns")]
        [Description("Click the button to bring forth the settings panel.")]
        [Editor(typeof(DateDifferenceEditor_), typeof(UITypeEditor))]
        [ReadOnly(true)]
        [XmlIgnore]
        public DateTimeDifferenceSettings Settings => this;

        [Browsable(false)]
        public List<string> GroupingColumns
        {
            get { return _groupingColumns; }
            set { _groupingColumns = value; }
        }

        [DisplayName("New Column Name")]
        [Description("The name of the new column where the result will be placed.")]
        public string NewColumn
        {
            get { return _newColumn; }
            set { _newColumn = value; }
        }

        [DisplayName("Time Unit")]
        [Description("The unit to be used when calculating the difference.")]
        [ReadOnly(false)]
        public TimeUnit TimeFormat
        {
            get { return _timeFormat; }
            set { _timeFormat = value; }
        }

        public override async Task<bool> IsValid()
        {
            

            if (_dateColumn == null)
               throw new PluginException("No date column specified. ");
            if (_newColumn == null)
                throw new PluginException("No name for new column specified. ");
            if (TableName == null)
                throw new PluginException("No table specified. ");

            return true;
        }

        public override string ToString()
        {
            if (_groupingColumns == null)
                return "";

            string str = ""; bool firstRun = true;
            foreach (string column in _groupingColumns)
            {
                if (firstRun)
                {
                    str += column;
                    firstRun = false;
                }
                else str += ", " + column;
            }
            return str;
        }

        private class DateDifferenceEditor_ : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                DateTimeDifferenceSettings settings = (DateTimeDifferenceSettings)value;
                DateDifferenceEditor editor = new DateDifferenceEditor(settings);
                editor.ShowDialog();
                return base.EditValue(context, provider, null);
            }
        }
    }
}


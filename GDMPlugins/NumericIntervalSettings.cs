using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class NumericIntervalSettings : PluginSettings
    {
        public enum NumericFilterMode { LowerBound, UpperBound, Interval };

        private string _tableName = "";
        private string _columnName = "";
        private double _minimum = 0;
        private double _maximum = 0;
        private NumericFilterMode _mode;

        [DisplayName("Mode of Operation")]
        [Description("Specifies how the filtering will be executed.")]
        public NumericFilterMode Mode
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

        [DisplayName("Lower Bound")]
        [Description("Either Integer or Double.")]
        public double Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        [DisplayName("Upper Bound")]
        [Description("Either Integer or Double.")]
        public double Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        public override async Task<bool> IsValid()
        {
            var errorMsg = "";

            if (_tableName == null)
                errorMsg += "The table name was not set. ";
            if (_columnName == null)
                errorMsg += "The column name was not set. ";
            if (_mode == NumericFilterMode.Interval && _minimum > _maximum)
                errorMsg += "The interval is not valid. ";
            if (errorMsg != "")
                throw new PluginException(errorMsg);
            return !(errorMsg.Length > 0);
        }
    }
}

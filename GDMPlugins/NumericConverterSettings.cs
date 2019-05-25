using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class NumericConverterSettings : PluginSettings
    {
        public enum DataType { Double, Integer, String };
        public enum NumericErrorAction {Delete, Abort};
        private string _tableName = null;
        private string _columnName = null;
        private DataType _type;
        private NumericErrorAction _action;
      
        [DisplayName("Action on Error")]
        [Description("Abort: the data conversion of the specific column is terminated.\nDelete: values that cannot be converted to specified datatype will be removed.")]
        public NumericErrorAction Action
        {
            get { return _action; }
            set { _action = value;}
        }
         
        [DisplayName("Table Name")]
        [Description("The name of the table to apply the plugin onto.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value;}
        }

        [DisplayName("Column Name")]
        [Description("The name of the column to apply the plugin onto.")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value;}
        }

        [DisplayName("Convert to")]
        [Description("The datatype to convert into.")]
        public DataType Type
        {
            get { return _type; }
            set { _type = value;}
        }

        public override async Task<bool> IsValid()
        {
            var errorMsg = "";

            if (_tableName == null)
                errorMsg += "Table name not set. ";
            if (_columnName == null)
                errorMsg += "Column name not set. ";
            if (errorMsg != "")
                throw new PluginException(errorMsg);
            return true;
        }
    }
}
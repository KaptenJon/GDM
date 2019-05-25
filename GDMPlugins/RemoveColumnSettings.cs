using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveColumnSettings : PluginSettings
    {
        private string _columnName;
        private string _tableName;

        [DisplayName("Table name")]
        [Description("The name of the table to remove column from.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("Column to remove")]
        [Description("The name of column to remove.")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public override async Task<bool> IsValid()
        {
            if (_columnName.Length > 0 && _tableName.Length > 0)
            {

                return true;
            }
            else
            {
                throw new PluginException("Undefined error occured");

            }
        }
    }
}


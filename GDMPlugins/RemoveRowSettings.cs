using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveRowSettings : PluginSettings
    {
        private string _tableName;
        private int _rowNumber;

        [DisplayName("Table name")]
        [Description("The name of the table to remove a row from.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("The row number to remove")]
        [Description("The zero based row number to remove")]
        [ReadOnly(false)]
        public int RowNumber
        {
            get { return _rowNumber; }
            set { if (value >= 0) _rowNumber = value; else _rowNumber = 0; }
        }

        public override async Task<bool> IsValid()
        {
            if (_tableName.Length>0)
            {
               
                return true;
            }
            else
            {
                return false;

            }
        }
    }
}


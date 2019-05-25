using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RenameTableSettings : PluginSettings
    {
        private string _tableName;
        private string _newTableName;


        [DisplayName("Table name")]
        [Description("The name of the table to rename.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("New name of table")]
        [Description("The new name of the table.")]
        public string NewTableName
        {
            get { return _newTableName; }
            set { _newTableName = value; }
        }

        public override async Task<bool> IsValid()
        {
            if (_newTableName.Length > 0)
            {
                
                return true;
            }
            else
            {
               throw new PluginException("The new table name is not valid");
               
            }
        }
    }
}


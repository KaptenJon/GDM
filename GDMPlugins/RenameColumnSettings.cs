using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RenameColumnSettings : PluginSettings
    {
        private string _tableName = null;
        private string _newName = null;
        private string _columnName = null;

        [DisplayName("Table name")]
        [Description("The name of the table to apply the plugin onto. ")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("Column name")]
        [Description("The name of the column to apply the plugin onto. ")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        [DisplayName("New column name")]
        [Description("The new name of the specified column. ")]
        public string NewName
        {
            get { return _newName; }
            set { _newName = value; }
        }

        public override async Task<bool> IsValid()

        {
            
            if (_tableName == null)
                throw new PluginException("No table given. ");
            if (_columnName == null)
               throw new PluginException("No column given. ");
            if (_newName == null)
                throw new PluginException("No new name given. ");

           
           
             return true;
        }
    }
}

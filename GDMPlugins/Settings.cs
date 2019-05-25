using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class Settings:PluginSettings
    {
        public enum Direction { Descending, Ascending };
        private string _tableName = null;
        private string _columnName = null;
        #region Overrides of PluginSettings

        public Settings()
        {
            
        }
        public Settings(IModel m)
        {
            TableName = m.SelectedTable.TableName;
            ColumnName = m.SelectedColumnName;
        }

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

        public override async Task<bool> IsValid()
        {
            
            if (ColumnName != null && TableName != null)
                return true;
            return false;
        }

        [DisplayName("Sort Direction")]
        [Description(
            "Specify the sort direction"
            )]
        public Direction SortDirection { get; set; }

        #endregion
    }
}

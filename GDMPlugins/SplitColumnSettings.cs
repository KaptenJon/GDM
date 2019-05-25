using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class SplitColumnSettings : PluginSettings
    {
        public enum SeparatorChar {NewLine, Other, Space, Tab};

        private string _tableName = null;
        private string _columnName = null;
        // Only used when separatorCharSelection is Other
        private char _ownSeparatorChar;
        private SeparatorChar _separatorCharSelection;

        [DisplayName("Separator Character")]
        [Description("Choose \"Other\" to define a separator character not listed.")]
        public SeparatorChar SeparatorCharSelection
        {
            get { return _separatorCharSelection; }
            set { _separatorCharSelection = value; }
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
        [Description("The name of the column to apply the plugin onto. ")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        [DisplayName("Other Separator Character")]
        [Description("Create a new separator character not already defined.")]
        public char OwnSeparatorChar
        {
            get { return _ownSeparatorChar; }
            set
            {
                _ownSeparatorChar = value;
            }
        }

        public override async Task<bool> IsValid()
        {
          

            if (_tableName == null)
               throw new PluginException("No table name given. ");
            if (_columnName == null)
                throw new PluginException("No column name given. ");
            if (_separatorCharSelection==SeparatorChar.Other && _ownSeparatorChar == '\0')
                throw new PluginException("No separator character defined. ");

            return true;
        }
    }
}


using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class MergeTablesSettings : PluginSettings
    {
        private string _table1Name = null;
        private string _table2Name = null;
        private string _table1KeyColumn = null;
        private string _table2KeyColumn = null;
        private string _table1Regex;
        private string _table2Regex;

        [DisplayName("Table1 Regular Expression")]
        public string Table1Regex
        {
            get { return _table1Regex; }
            set { _table1Regex = value; }
        }

        [DisplayName("Table2 Regular Expression")]
        public string Table2Regex
        {
            get { return _table2Regex; }
            set { _table2Regex = value; }
        }

        [DisplayName("Table1 Key Column")]
        public string Table1KeyColumn
        {
            get { return _table1KeyColumn; }
            set { _table1KeyColumn = value; }
        }

        [DisplayName("Table2 Key Column")]
        public string Table2KeyColumn
        {
            get { return _table2KeyColumn; }
            set { _table2KeyColumn = value; }
        }

        [DisplayName("Table1 Name")]
        public string Table1Name
        {
            get { return _table1Name; }
            set { _table1Name = value; }
        }
        [DisplayName("Table2 Name")]
        public string Table2Name
        {
            get { return _table2Name; }
            set { _table2Name = value; }
        }

        public override async Task<bool> IsValid()
        {
            
            return true;
        }
    }
}

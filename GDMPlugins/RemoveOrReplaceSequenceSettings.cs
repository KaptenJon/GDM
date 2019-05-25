using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveOrReplaceSequenceSettings : PluginSettings
    {
        public enum Selection { All, Specific };
        public enum Option { AllOccurences, BetweenTags}
        private Selection _rowSelection;
        private Option _removeOption;
        private string _startTag;
        private string _endTag;
        private string _tableName;
        private string _columnName;
        private int _rowNumber = 1;
        private string _sequence;
        private string _newSequence;

        [DisplayName("Replacement Sequence")]
        [Description("The sequence to replace the old sequence with. Optional and if left out the old sequence will be removed.")]
        public string NewSequence
        {
            get { return _newSequence; }
            set { _newSequence = value; }
        }

        [DisplayName("Column Name")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        
        [DisplayName("Remove Option")]
        [Description("AllOccurences: all sequences in the column will be removed/replaced. BetweenTags: only sequences that occur between the start and end tag will be removed/replaced.")]
        public Option RemoveOption
        {
            get { return _removeOption; }
            set { _removeOption = value; }
        }

        [DisplayName("Sequence")]
        [Description("The character sequence to replace/remove.")]
        public string Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        [DisplayName("Row Selection")]
        [Description("All: the plugin will be applied on all rows. Specific: the plugin will only be applied on given row.")]
        public Selection RowSelection
        {
            get { return _rowSelection; }
            set { _rowSelection = value; }
        }

        [DisplayName("Table Name")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("Row Number")]
        [Description("The row number to apply the plugin onto. Optional, but compulsory if 'Row Selection' is set to 'Specific'.")]
        public int RowNumber
        {
            get { return _rowNumber; }
            set { _rowNumber = value; }
        }

        [DisplayName("Start String")]
        [Description("Optional, but compulsory if 'Row Option' is set to 'BetweenTags'.")]
        public string StartTag
        {
            get { return _startTag; }
            set { _startTag = value; }
        }

        [DisplayName("End String")]
        [Description("Optional, but compulsory if 'Row Option' is set to 'BetweenTags'.")]
        public string EndTag
        {
            get { return _endTag; }
            set { _endTag = value; }
        }

        public override async Task<bool> IsValid()
        {
            string localErrorMsg = "";
            if (_tableName == null)
                localErrorMsg += "No table name given. ";
            if (_removeOption == Option.BetweenTags && _startTag == null)
                localErrorMsg += "No start tag given. ";
            if (_removeOption == Option.BetweenTags && _endTag == null)
                localErrorMsg += "No end tag given. ";
            if (_rowSelection == Selection.Specific && _rowNumber < 1)
                localErrorMsg += "Non valid row number. ";

            
            if (localErrorMsg != "")
                throw new PluginException(localErrorMsg);
            else return true;
        }
    }
}

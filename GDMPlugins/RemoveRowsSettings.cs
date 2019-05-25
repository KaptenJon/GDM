using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    
    public class RemoveRowsSettings : PluginSettings
    {
        private int _rowNumber = 0;
        private int _endRowNumber = 0;

        public RemoveRowsSettings()
        {
        }

        public RemoveRowsSettings(IModel model)
        {
            Update(model);
        }
        public void Update(IModel m)
        {
            TableName = m.SelectedTable.TableName;
        }

        [DisplayName("Table name")]
        [Description("The name of the table to apply the plugin onto. ")]
        [ReadOnly(true)]
        public string TableName
        {
            get; set; }

        [DisplayName("The first row to remove")]
        [Description("The zero based start row number to remove")]
        [ReadOnly(false)]
        public int StartRow
        {
            get { return _rowNumber; }
            set { if (value >= 0) _rowNumber = value; else _rowNumber = 0; }
        }
        [DisplayName("The last row to remove")]
        [Description("The zero based end row number to remove")]
        [ReadOnly(false)]
        public int EndRow
        {
            get { return _endRowNumber; }
            set { if (value >= 0) _endRowNumber = value; else _endRowNumber = 0; }
        }

        public override  Task<bool> IsValid()
        {
            
            return Factory.StartNew(() => TableName != null);
        }
    }
}

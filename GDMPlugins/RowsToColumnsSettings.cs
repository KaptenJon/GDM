using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RowsToColumnsSettings : PluginSettings
    {
        private string _tableName;
        private string _keyColumn;
        private string _dataColumn;
        private string _nameColumnn;
        private IModel _model;
        public RowsToColumnsSettings(){}

        public RowsToColumnsSettings(IModel model) { _model = model; }


        [DisplayName("Table name")]
        [Description("The name of the original table.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("Key column")]
        [Description("The column of the original table whos values are made distinct in the new table.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string KeyColumn
        {
            get { return ListBoxEditor.GetColumns(_model, TableName); }
            set { ListBoxEditor.SetColumns(_model, value, ref _keyColumn);}
        }

        [DisplayName("Data column")]
        [Description("The value in this column is put into the current name column.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DataColumn
        {
            get { return ListBoxEditor.GetColumns(_model, TableName); }
            set { ListBoxEditor.SetColumns(_model, value, ref _dataColumn); }
        }
        
        [DisplayName("Name column")]
        [Description("All distinct values of this column is made into columns in the new table.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string NameColumn
        {
            get { return ListBoxEditor.GetColumns(_model, TableName); }
            set { ListBoxEditor.SetColumns(_model, value, ref _nameColumnn); }
        }

        public override async Task<bool> IsValid()
        {
            

            if (_dataColumn == null)
                throw new PluginException("Data column not set. ");
            if(_tableName== null)
                throw new PluginException("Table name not set. ");
            if(_nameColumnn==null)
                throw new PluginException("Name column not set. ");
            if(_keyColumn== null)
                throw new PluginException("Key column not set. ");

            return true;
        }
    }
}
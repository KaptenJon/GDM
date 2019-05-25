using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveRowByColumnSettings : PluginSettings
    {
        public enum Action { Keep, Delete };

        private string _columnName;
        private IModel _model;
        public RemoveRowByColumnSettings() { }
        public RemoveRowByColumnSettings(IModel model) {_model = model;}

        [DisplayName("Table name")]
        [Description("The name of the table to remove a row from.")]
        [ReadOnly(true)]
        public string Table { get; set; }

        [Description("The column to match against the regular expression.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string Column
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, Table);
                }
                return _columnName;
            }
            set
            {
                if (_model == null)
                {
                    _columnName = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, Table, _model))
                    {
                        _columnName = value;
                    }
                }
            }
        }

        [DisplayName("Row Action")]
        [Description("Action to take if regular expression match the column value. Keep: keep all matching columns and delete all other rows. Delete: delete all rows matching the column value.")]
        public Action RowAction { get; set; }

        [DisplayName("Table Action")]
        [Description("Keep: keep the original table. Delete: the original table will be replaced by the outcome of current plugin.")]
        public Action TableAction { get; set; }

        [Description("The Regular Expression that is used to match against the column value.")]
        [DisplayName("Regular Expression")]
        public string RegularExpression { get; set; }

        public override async Task<bool> IsValid()
        {
            

            if (Table == null)
                throw new PluginException("No table name given. ");
            if(_columnName == null)
                throw new PluginException("No column name given. ");
            if (RegularExpression == null)
                throw new PluginException("No regular expression specified. ");

            return true;
        }
    }
}


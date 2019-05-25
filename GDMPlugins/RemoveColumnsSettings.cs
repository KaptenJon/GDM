using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{

    public class RemoveColumnsSettings : PluginSettings
    {

        private int _endColumnNumber = -1;
        private string[] _columnList = new string[0];
        private IModel _model;

        public RemoveColumnsSettings()
        {
        }

        public RemoveColumnsSettings(IModel model)
        {
            Update(model);
            _model = model;
        }
        public void Update(IModel m)
        {
            TableName = m.SelectedTable.TableName;
            StartColumn = m.SelectedColumnName;
            _model = m;
        }

        [DisplayName("Table name")]
        [Description("The name of the table to apply the plugin onto. ")]
        [ReadOnly(true)]
        public string TableName
        {
            get;
            set;
        }

        [DisplayName("The first Column to remove")]
        [Description("The zero based start Column number to remove")]
        [ReadOnly(false)]
        public String StartColumn { get; set; }

        [DisplayName("The last Column to remove, -1 gives all after first")]
        [Description("The zero based end Column number to remove")]
        [ReadOnly(false)]
        public int NumberOfColumns
        {
            get { return _endColumnNumber; }
            set { if (value >= 0) _endColumnNumber = value; else _endColumnNumber = 0; }
        }

        [DisplayName("Select Columns")]
        [Description("If Columns selected. Only these are deleted")]
        [ReadOnly(false)]

        [Editor(typeof(SelectManyEditorDropdown), typeof(UITypeEditor))]
        public string[] ColumnList
        {
            get
            {
                SelectManyEditorDropdown.InitColumns(_model, _model?.SelectedTable?.TableName);
                return _columnList;
            }
            set { _columnList = value; }
        }

        public override Task<bool> IsValid()
        {
            return Factory.StartNew(() => { return TableName != null || StartColumn != null; });
        }
    }
}

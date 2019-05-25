using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins.Statistics
{
    public class StatisticRowOperationSettings : PluginSettings
    {
        private IModel _model;
        private string _dataSourceTable;
        private string _dataColumn;
        private string _targetTable;
        private string _operationNameColumn;
        private string _targetColumn;


        public StatisticRowOperationSettings() { }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model
        {
            get { return _model; }
            set { _model = value; }
        }



        public StatisticRowOperationSettings(IModel model)
        {
            _model = model;
        }

        [Browsable(true)]
        [DisplayName("1 Data Source Table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DataSourceTable
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _dataSourceTable;
            }
            set
            {

                if (_model == null || ListBoxEditor.IsTable(value, _model))
                    _dataSourceTable = value;
            }
        }


        [Browsable(true)]
        [DisplayName("2 Target table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string TargetTable
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _targetTable;
            }
            set
            {
                _targetTable = value;
            }
        }

        [Browsable(true)]
        [DisplayName("2.1 Target column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string TargetColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, TargetTable);
                return _targetColumn;
            }
            set { _targetColumn = value; }
        }

        [Browsable(true)]
        [DisplayName("1.1 Data source operation name column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string OperationNameColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceTable);
                return _operationNameColumn;
            }
            set
            {
                if (_model == null || ListBoxEditor.IsColumn(value, DataSourceTable, _model))
                    _operationNameColumn = value;
            }
        }


        [Browsable(true)]
        [DisplayName("1.2 Data source column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DataColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceTable);
                return _dataColumn;
            }
            set
            {
                if (_model == null || ListBoxEditor.IsColumn(value, DataSourceTable, _model))
                    _dataColumn = value;
            }
        }


        public override string ToString()
        {
            return "";
        }


        public override async Task<bool> IsValid()
        {


            if (_dataColumn == null)
                throw new PluginException("Data Column not specified. ");
            if (_dataSourceTable == null)
                throw new PluginException("Data DataSourceCtCtTable not specified. ");

            return true;
        }

    }
}

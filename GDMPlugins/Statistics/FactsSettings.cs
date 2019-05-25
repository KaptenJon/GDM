using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins.Statistics
{
    public class FactsSettings : PluginSettings
    {
        private IModel _model;
        private List<string> _groupingColumns = new List<string>();
        private string _dataSourceCTTable;
        private string _targetTable;
        private string _operationNameColumnCT;
        private string _CTColumn;
        private string _unitCT;
  
        private string _decimal = ".";
        private string _dataSourceMTBFTable;
        private string _operationNameColumnMTBF;
        private string _MTBFColumn;
        private string _unitMTBF;
        private string _dataSourceMTTRTable;
        private string _operationNameColumnMTTR;
        private string _MTTRColumn;
        private string _unitMTTR;
        private string _choosenDistributionCT = "Best Fit";
        private string _choosenDistributionMTBF = "Best Fit";
        private string _choosenDistributionMTTR="Best Fit";
        private string[] _choosenDistributionsCT;
        private string[] _choosenDistributionsMTBF;
        private string[] _choosenDistributionsMTTR;

        public FactsSettings() { }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model
        {
            get { return _model; }
            set { _model = value; }
        }



        public FactsSettings(IModel model)
        {
            _model = model;
        }

#region CT

        [Browsable(true)]
        [DisplayName("1 Data Source CT Table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DataSourceCTTable
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _dataSourceCTTable;
            }
            set
            {
                
                if (_model == null || ListBoxEditor.IsTable(value, _model)) 
                    _dataSourceCTTable = value;
            }
        }

        [Browsable(true)]
        [DisplayName("1.1 Data source CT operation name column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string OperationNameColumnCT
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceCTTable);
                return _operationNameColumnCT;
            }
            set
            {
                if (_model == null || ListBoxEditor.IsColumn(value, DataSourceCTTable, _model))
                    _operationNameColumnCT = value;
            }
        }

        [Browsable(true)]
        [DisplayName("1.2 Data Source CT Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string CTColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceCTTable);
                return _CTColumn;
            }
            set { _CTColumn = value; }
        }
        [Browsable(true)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        [DisplayName("1.3 Unit CT")]
        public String UnitCT
        {
            get
            {
                ListBoxEditor.List = new List<object>(new[] { "s", "min", "hr" });

                return _unitCT;
            }
            set
            {
                _unitCT = value;
            }
        }
        [Browsable(false)]
        [Obsolete]
        public string DistributionCT
        {
            get
            {
                ListBoxEditor.List = new List<object> {Facts._distributions.Select(t => t.GetType().Name).ToList()};
                ListBoxEditor.List.Add("Best Fit");

                return _choosenDistributionCT;
            }
            set
            {
                _choosenDistributionCT = value;
            }
        }

        [Browsable(true)]
        [DisplayName("1.4 Choose Distribution For CT")]
        [Editor(typeof(SelectManyEditorDropdown), typeof(UITypeEditor))]
        public string[] DistributionsCT
        {
            get
            {
                SelectManyEditorDropdown.List = Facts._distributions.Select(t => t.GetType().Name).ToList();
                SelectManyEditorDropdown.List.Add("All");

                return _choosenDistributionsCT;
            }
            set
            {
                _choosenDistributionsCT = value;
            }
        }

        #endregion
        #region MTBF
        [Browsable(true)]
        [DisplayName(@"2 Data Source MTBF Table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DataSourceMTBFTable
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _dataSourceMTBFTable;
            }
            set
            {

                if (_model == null || ListBoxEditor.IsTable(value, _model))
                    _dataSourceMTBFTable = value;
            }
        }

        [Browsable(true)]
        [DisplayName("2.1 Data source MTBF operation name column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string OperationNameColumnMTBF
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceMTBFTable);
                return _operationNameColumnMTBF;
            }
            set
            {
                if (_model == null || ListBoxEditor.IsColumn(value, DataSourceMTBFTable, _model))
                    _operationNameColumnMTBF = value;
            }
        }

        [Browsable(true)]
        [DisplayName("2.2 Data Source MTBF Data Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string MTBFColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceMTBFTable);
                return _MTBFColumn;
            }
            set { _MTBFColumn = value; }
        }
        [Browsable(true)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        [DisplayName("2.3 Unit MTBF")]
        public String UnitMTBF
        {
            get
            {
                ListBoxEditor.List = new List<object>(new[] { "s", "min", "hr" });

                return _unitMTBF;
            }
            set
            {
                _unitMTBF= value;
            }
        }
        [Browsable(false)]
    [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DistributionMTBF
        {
            get
            {
                ListBoxEditor.List = new List<object> {Facts._distributions.Select(t => t.GetType().Name).ToList()};
                ListBoxEditor.List.Add("Best Fit");

                return _choosenDistributionMTBF;
            }
            set
            {
                _choosenDistributionMTBF = value;
            }
        }


        [Browsable(true)]
        [DisplayName("2.4 Choose Distribution For MTBF")]
        [Editor(typeof(SelectManyEditorDropdown), typeof(UITypeEditor))]
        public string[] DistributionsMTBF
        {
            get
            {
                SelectManyEditorDropdown.List = Facts._distributions.Select(t => t.GetType().Name).ToList();
                SelectManyEditorDropdown.List.Add("All");

                return _choosenDistributionsMTBF;
            }
            set
            {
                _choosenDistributionsMTBF = value;
            }
        }

        #endregion
        #region MTTR
        [Browsable(true)]
        [DisplayName(@"3 Data Source MTTR Table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DataSourceMTTRTable
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _dataSourceMTTRTable;
            }
            set
            {

                if (_model == null || ListBoxEditor.IsTable(value, _model))
                    _dataSourceMTTRTable = value;
            }
        }

        [Browsable(true)]
        [DisplayName("3.1 Data source MTTR operation name column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string OperationNameColumnMTTR
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceMTTRTable);
                return _operationNameColumnMTTR;
            }
            set
            {
                if (_model == null || ListBoxEditor.IsColumn(value, DataSourceMTTRTable, _model))
                    _operationNameColumnMTTR = value;
            }
        }

        [Browsable(true)]
        [DisplayName("3.2 Data Source MTTR Data Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string MTTRColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceMTTRTable);
                return _MTTRColumn;
            }
            set { _MTTRColumn = value; }
        }
        [Browsable(true)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        [DisplayName("3.3 Unit MTTR")]
        public String UnitMTTR
        {
            get
            {
                ListBoxEditor.List = new List<object>(new[] { "s", "min", "hr" });

                return _unitMTTR;
            }
            set
            {
                _unitMTTR = value;
            }
        }
        [Browsable(false)]
        public string DistributionMTTR
        { 
            get
            {
                ListBoxEditor.List = new List<object> {Facts._distributions.Select(t => t.GetType().Name).ToList()};
                ListBoxEditor.List.Add("Best Fit");

                return _choosenDistributionMTTR;
            }
            set
            {
                _choosenDistributionMTTR = value;
            }
        }

        [Browsable(true)]
        [DisplayName("3.4 Choose Distribution For MTTR")]
        [Editor(typeof(SelectManyEditorDropdown), typeof(UITypeEditor))]
        public string[] DistributionsMTTR
        {
            get
            {
                SelectManyEditorDropdown.List = Facts._distributions.Select(t => t.GetType().Name).ToList();
                SelectManyEditorDropdown.List.Add("All");

                return _choosenDistributionsMTTR;
            }
            set
            {
                _choosenDistributionsMTTR = value;
            }
        }

        #endregion
        [Browsable(true)]
        [DisplayName("4 Target table")]
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



       

        [DisplayName("6 Decimal point symbol")]
        [Description("The symbol to use for Decimal seperation")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string Decimal
        {
            get
            {
                ListBoxEditor.List = new List<object>(new[] { ".", "," });
                return _decimal;
            }
            set { _decimal = value; }
        }



        public override async Task<bool> IsValid()
        {
            
            
            if (_targetTable == null)
                throw new PluginException("Target Table not specified. ");
            return true;
        }

    }
}

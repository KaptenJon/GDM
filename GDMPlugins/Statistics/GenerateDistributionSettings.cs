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
    public class GenerateDistributionSettings : PluginSettings
    {
        private IModel _model;
        private List<string> _groupingColumns = new List<string>();
        private string _dataSourceTable;
        private string _dataColumn;
        private bool _confirmation;
        private string _targetTable;
        private string _operationNameColumn;
        private string _targetColumn;


        [DisplayName("Confirmation")]
        [Description("True: enables the user to manually confirm and/or change the model (by reviewing PP plots) for every grouping entry. False: no confirmation needed.")]
        public bool Confirmation
        {
            get { return _confirmation; }
            set { _confirmation = value; }
        }

        public GenerateDistributionSettings() { }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model
        {
            get { return _model; }
            set { _model = value; }
        }



        public GenerateDistributionSettings(IModel model)
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

        private string _choosenDistribution = "Best Fit";
        private string _filterColumn;
        private string _filter;
        private string _unit;
        private string _decimal =".";
        private string[] _distributions;

        [Browsable(false)]
        [Obsolete("Use distributions instead")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string Distribution
        {
            get
            {
                ListBoxEditor.List = new List<object> {DistributionEvaluator.GetAllDistributionNames().ToList()};
                ListBoxEditor.List.Add("Best Fit");

                return _choosenDistribution;
            }
            set
            {
                _choosenDistribution = value;
            }
        }

        [Browsable(true)]
        [DisplayName("3 Choose Distribution")]
        [Editor(typeof(SelectManyEditorDropdown), typeof(UITypeEditor))]
        public string[] Distributions
        {
            set { _distributions = value; }

            get
            {
                SelectManyEditorDropdown.List = DistributionEvaluator.GetAllDistributionNames();
                SelectManyEditorDropdown.List.Add("All");
                
                return _distributions;
            }
        }

        [Browsable(false)]
        public List<string> GroupingColumns
        {
            get
            {
                
                return _groupingColumns;
            }
            set
            {
                _groupingColumns = value;
            }
        }

        [Browsable(true)]
        [DisplayName("5.1 Filter On Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string FilterColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, DataSourceTable);
                return _filterColumn;
            }
            set
            {
                _filterColumn = value;
            }
        }
        [DisplayName("Decimal seperator")]
        [Description("The name of the sheet to extract from the Excel file.")]
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


        [Browsable(true)]
        [DisplayName("5.2 Filter")]
        public String Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
            }
        }

        [Browsable(true)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        [DisplayName("4 Unit")]
        public String Unit
        {
            get
            {
                ListBoxEditor.List=new List<object>(new []{"s", "min", "hr"});
                
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }

        [Browsable(true)]
        [DisplayName("1.2 Data source column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DataColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model,DataSourceTable);
                return _dataColumn;
            }
            set
            {
                if (_model == null || ListBoxEditor.IsColumn(value, DataSourceTable, _model))
                    _dataColumn = value;
            }
        }

        [DisplayName("Analyze Data")]
        [Description("Click the button to bring forth the settings panel.")]
        [Editor(typeof(GroupingAndPreviewEditor_), typeof(UITypeEditor))]
        [ReadOnly(true)]
        [XmlIgnore]
        public GenerateDistributionSettings GroupingSettings => this;


        public override string ToString()
        {
            return "";
        }


        private class GroupingAndPreviewEditor_ : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                GenerateDistributionSettings settings = (GenerateDistributionSettings)value;
                GroupingAndPreviewEditor editor = new GroupingAndPreviewEditor(settings);
                editor.ShowDialog();
                return base.EditValue(context, provider, null);
            }
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

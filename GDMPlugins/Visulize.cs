using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Accord;
using GDMInterfaces;
using GDMPlugins.SQL;

namespace GDMPlugins
{
    public class Visulize:IOutput
    {
        public string Description { get; } = "Create a report and visulizemean and standarddeviation in a diagram.";
        public string Version { get; } = "1.0";
        public string Name { get; } = "Report Mean Std";
        public Image Icon { get; }
        public PluginSettings GetSettings(IModel model)
        {
            return new VisulizePluginSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var settings = pluginSettings as VisulizePluginSettings;
            if(settings == null)
                return;
            settings._model = model;
        }

        public Type GetSettingsType()
        {
            return typeof (VisulizePluginSettings);

        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var settings = pluginSettings as VisulizePluginSettings;
            if(settings == null)
                return;
            var form = new VisulizeFormcs(model.GetTable(settings.DataSourceTable));

            form._model = model?.GetTable(settings.DataSourceTable);
            VisulizeFormcs._columnstd = settings.StdColumn;
            VisulizeFormcs._columnmean = settings.MeanColumn;
            VisulizeFormcs._columnresource = settings.ResourceColumn;

            
            form.Export(settings.ReportFile);
        }

        public string GetJobDescription(PluginSettings s)
        {
            return (s as VisulizePluginSettings)?.GetDescription();
        }

        public object GetDynamicSettings(PluginSettings s)
        {
           // throw new NotImplementedException();
            return null;
        }

        public Tag Tags { get; }
    }

    public class VisulizePluginSettings: PluginSettings
    {
        private string _table;
        private string _dataSourceTable;
        [XmlIgnore]
        public IModel _model;
        private string _resourceColumn;
        private string _meanColumn;
        private string _stdColumn;


        public async override Task<bool> IsValid()
        {
            return
                !(String.IsNullOrWhiteSpace(DataSourceTable) && String.IsNullOrWhiteSpace(MeanColumn) &&
                  String.IsNullOrWhiteSpace(StdColumn));
        }

        [DisplayName("4 Preview Report")]

        [Editor(typeof(ShowReportEditor), typeof(UITypeEditor))]
        public string ShowReport
        {
            get
            {
                return _table;
                
            }
            set { _table = value; }
        }
        [DisplayName("3 Export Report PDF File")]
        [Editor(typeof(SaveFileNameEditor), typeof(UITypeEditor))]
        public string ReportFile { get; set; }

        [Browsable(true)]
        [DisplayName("1 Data Source FromTable")]
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
                if (PluginSettings.IsInUIMode)
                    ShowReportEditor.Table = _model?.GetTable(value);


            }
        }

        [DisplayName("2.1 Resource Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ResourceColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, _dataSourceTable);
                return _resourceColumn;
            }
            set
            {
                _resourceColumn = value;
             if (PluginSettings.IsInUIMode)
                    VisulizeFormcs._columnresource = value;
            }
        }


        [Browsable(true)]
        [DisplayName("2.2 Mean Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string MeanColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, _dataSourceTable);
                return _meanColumn;
            }
            set
            {
                _meanColumn = value;
                if (PluginSettings.IsInUIMode)
                    VisulizeFormcs._columnmean = value;
            }
        }


        [Browsable(true)]
        [DisplayName("2.3 Error Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string StdColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, _dataSourceTable);
                return _stdColumn;
            }
            set
            {
                _stdColumn = value;
                if (PluginSettings.IsInUIMode)
                    VisulizeFormcs._columnstd = value;
            }
        }
    }

    public class ShowReportEditor:UITypeEditor
    {


        public ShowReportEditor()
        {
            
        }

        public static DataTable Table { get; set; }


        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {

            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var form = new VisulizeFormcs(Table);

            form.ShowDialog();

            return base.EditValue(context, provider, value);
        }
    }

    public class Report
    {
    }
}

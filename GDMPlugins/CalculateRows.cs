using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    /// <summary>
    /// Made by Jon Andersson
    /// </summary>
    public class CalculateRows : ITool
    {


        public string Description => "Calculate Rows based on expression eg. * / ( ) + - and column names add '[ ]' around columnnames and seperate by space ";

        public string Version => "vnext";

        public string Name => "Calculate Rows Formula";

        public Image Icon => Icons.CreateData;

        public PluginSettings GetSettings(IModel model)
        {

            var rowsSettings = new CalculateRowsSettings();
            UpdateSettings(rowsSettings, model);
            return rowsSettings;
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {

            var _rowsSettings = pluginSettings as CalculateRowsSettings;
            _rowsSettings.Table = model.SelectedTable.TableName;
        }

        public Type GetSettingsType()
        {
            return typeof(CalculateRowsSettings);
        }
        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {

            var settings = pluginSettings as CalculateRowsSettings;
            var table = model.GetTable(settings.Table);
            status.InitStatus("Starting Formula Calculations", table.Rows.Count);

            if (!table.Columns.Contains(settings.NewRow))
                table.Columns.Add(settings.NewRow, typeof(double));
            List<string> columns = new List<string>();
            
            foreach (DataColumn col in table.Columns)
            {
                if (settings.Formula.Contains('['+ col.ColumnName + ']'))
                    columns.Add(col.ColumnName);
            }
            foreach (DataRow row in table.Rows)
            {
                var temp = settings.Formula;
                foreach (var column in columns)
                {
                    temp = temp.Replace('[' + column + ']', row[column].ToString());
                }
                EvaluateFormula evaluator = new EvaluateFormula();
                if (settings.DecPoint != "Default")
                    evaluator = new EvaluateFormula(settings.DecPoint[0]);
                row[settings.NewRow] = evaluator.Parse(temp);
                status.Increment();
            }

        }

        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Calculate";
    }
    public class CalculateRowsSettings : PluginSettings
    {
        private string _decPoint = "Default";

        [DisplayName("Table Name")]
        [Description("The name of the table to calculate a difference upon.")]
        [ReadOnly(true)]
        public string Table
        {
            get;
            set;
        }

        [DisplayName("Formula")]
        [Description("Use Column Names surrounded by '[ ]' (eg. [Columnname]) together with numerics and *,/,-,+,(,) use (,) as decimal seperator, sqrt(a) — square root of a number, exp(x), (a)log(b), ln(x), abs(x) — absolute value")]
        [ReadOnly(false)]
        public string Formula
        {
            get;
            set;
        }
        [DisplayName("New Row Name")]
        [Description("The name of the table to calculate a difference upon.")]
        [ReadOnly(false)]
        public string NewRow { get; set; }

        public override async Task<bool> IsValid()
        {

            return Table != null && NewRow != null;
        }

        [DisplayName("Decimal Point Character")]
        [Description("default is always the system default")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DecPoint
        {
            get
            {
                ListBoxEditor.List = new List<object>(new[] { "Default", ",", "." });
                return _decPoint;
            }
            set { _decPoint = value; }
        }

    }
}

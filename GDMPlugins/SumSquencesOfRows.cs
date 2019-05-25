using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class SumSquencesOfRows : ITool
    {
        public string Description { get; } =
            "Sumerize, or calculate avearge for all rows in a sequenze starting with a certan column value. E.g. en a sequence when Machine 5 and caluculate the sum in a particular column"
            ;

        public string Version { get; } = "0.5";
        public string Name { get; } = "Sum or average of a sequence";
        public Image Icon { get; } = Icons.Statistics;
        public PluginSettings GetSettings(IModel model)
        {
            return new SumSquencesOfRowsSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var settings = pluginSettings as SumSquencesOfRowsSettings;
            if(settings == null)
                return;
            if(model?.SelectedTable != null)
                settings.Table = model.SelectedTable.TableName;
            settings._model = model;
        }

        public Type GetSettingsType()
        {
            return typeof(SumSquencesOfRowsSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var settings = pluginSettings as SumSquencesOfRowsSettings;
            if (settings == null)
                return;
            DataTable table = model.GetTable(settings.Table);
            
            if(table == null)
                return;

            var resultTable = model.GetTable(settings.ResultTable);
            if (resultTable == null)
            {
                resultTable = model.CreateTable();
                resultTable.TableName = settings.ResultTable;
                if(settings.CopyRestOfColumnValues)
                    foreach (DataColumn dataColumn in table.Columns)
                    {
                        if(!resultTable.Columns.Contains(dataColumn.ColumnName))
                            resultTable.Columns.Add(new DataColumn(dataColumn.ColumnName, dataColumn.DataType));
                    }
                else
                {
                    if (!resultTable.Columns.Contains(settings.SequenzeColumn))
                        resultTable.Columns.Add(new DataColumn(settings.SequenzeColumn, table.Columns[settings.SequenzeColumn].DataType));
                    if (!resultTable.Columns.Contains(settings.CalculateColumn))
                        resultTable.Columns.Add(settings.CalculateColumn, typeof(double));
                    if(!String.IsNullOrWhiteSpace(settings.GroupBy) && !resultTable.Columns.Contains(settings.GroupBy))
                        resultTable.Columns.Add(new DataColumn(settings.GroupBy, table.Columns[settings.GroupBy].DataType));
                }
               
            }
            if (!String.IsNullOrWhiteSpace(settings.GroupBy))
            {
                var enumarabl = from i in table.AsEnumerable() group i by i[settings.GroupBy];
                foreach (var group in enumarabl)
                {
                    CalculateGroup(group.ToArray(), settings,  resultTable, group.Key.ToString());
                }
            }
            else
            {
                CalculateGroup(table.AsEnumerable().ToArray(), settings,  resultTable, null);
            }
            
        }

        private void CalculateGroup(DataRow[] rows, SumSquencesOfRowsSettings settings, DataTable resultTable, string groupBy)
        {
           if(rows == null)
                return;
           
            int count = 0;
            double sum = 0;
            if (settings.Overlapps)
            {
                var dataRows = rows.ToList();
                var used = new Dictionary<string,int>();
                for (int i = 0; i < dataRows.Count(); i++)
                {
                    if (!used.ContainsKey(dataRows[i][settings.SequenzeColumn].ToString()))
                    {
                        sum += dataRows[i][settings.CalculateColumn] as double? ?? 0;
                        count++;
                        used.Add(dataRows[i][settings.SequenzeColumn].ToString(), i);
                        if (dataRows[i][settings.SequenzeColumn].ToString() == (string) settings.SequenceStopper)
                        {

                            var newrow = resultTable.NewRow();
                            if (settings.CopyRestOfColumnValues)
                                for (int index = 0; index < dataRows[i].ItemArray.Length; index++)
                                {
                                    newrow[index] = dataRows[i][index];
                                }
                            newrow[settings.SequenzeColumn] = settings.SequenceStopper;
                            newrow[settings.CalculateColumn] = settings.Operation == "Sum" ? sum : sum/count;
                            if (groupBy != null)
                                newrow[settings.GroupBy] = groupBy;
                            resultTable.Rows.Add(newrow);
                            sum = 0;
                            count = 0;
                            foreach (var del in used.Values.OrderByDescending(t=>t ))
                            {
                                dataRows.RemoveAt(del);
                                
                            }
                            i = -1;
                            used.Clear();

                        }
                        
                    }
                }
            }
            else
            {


                for (int i = 0; i < rows.Count(); i++)
                {
                    sum += rows[i][settings.CalculateColumn] as double? ?? 0;
                    count++;
                    if (rows[i][settings.SequenzeColumn].ToString() == (string) settings.SequenceStopper)
                    {

                        var newrow = resultTable.NewRow();
                        if (settings.CopyRestOfColumnValues)
                            for (int index = 0; index < rows[i].ItemArray.Length; index++)
                            {
                                newrow[index] = rows[i][index];
                            }
                        newrow[settings.SequenzeColumn] = settings.SequenceStopper;
                        newrow[settings.CalculateColumn] = settings.Operation == "Sum" ? sum : sum/count;
                        if (groupBy != null)
                            newrow[settings.GroupBy] = groupBy;
                        resultTable.Rows.Add(newrow);
                        sum = 0;
                        count = 0;
                    }
                }

            }
        }

        public bool NeedColumnSelected { get; } = false;
        public bool NeedTableSelected { get; } = true;
        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory { get; } = "Statistics";
    }

    public class SumSquencesOfRowsSettings : PluginSettings
    {
        internal IModel _model;
        private string _table;
        private string _groupBy;
        private string _resultTable;
        private string _sequenzeColumn;
        private object _sequenceStopper;
        private string _calculateColumn;
        private string _operation;
        private bool _copyRestOfColumnValues = false;
        private bool _overlapps = false;

        public override Task<bool> IsValid()
        {
            return Task.Factory.StartNew(()=> true);
        }

        [DisplayName("1 Table")]
        [Description("Table for the operation")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string Table
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _table;
            }
            set { _table = value; }
        }
        [DisplayName("2.4 Group By Column")]
        [Description("Colculate the sum every time the sequenze stopper comes for each value in this column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string GroupBy
        {
            get
            {
                ListBoxEditor.InitColumns(_model, Table);
                return _groupBy;
            }
            set { _groupBy = value; }
        }

        [DisplayName("3.1 Name of the new table")]
        [Description("If an existing table is used make sure the columns are correct.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ResultTable

        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _resultTable;
            }
            set { _resultTable = value; }
        }

        [DisplayName("2.1 Column to end sequence")]
        [Description("Calculate the sum every time the sequenze stopper comes for each value in this column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string SequenzeColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model,Table);
                return _sequenzeColumn;
            }
            set { _sequenzeColumn = value; }
        }

        [DisplayName("2.2 Entity ending a sequence")]
        [Description("The name in the sequence column that ends a sequens")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public object SequenceStopper
        {
            get
            {
                ListBoxEditor.InitUniqeRowEntries(_model, Table, SequenzeColumn);
                return _sequenceStopper;
            }
            set { _sequenceStopper = value; }
        }

        [DisplayName("2.3 Column to calculate")]
        [Description("Which column to sum or calculate average")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string CalculateColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, Table);
                return _calculateColumn;
            }
            set { _calculateColumn = value; }
        }

        [DisplayName("2.4 Operation")]
        [Description("Which column to sum or calculate average")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string Operation
        {
            get
            {
                ListBoxEditor.List= new List<object>(new[]{"Sum", "Average"});
                return _operation;
            }
            set { _operation = value; }
        }

        [DisplayName("2.5 Overlapping Sequences")]
        [Description("Sequenses overlapps, Detect overlapps by summerize unique enteties in sequence column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public bool Overlapps
        {
            get
            {
                var l = new List<object>();
                l.Add(true);
                l.Add(false);
                ListBoxEditor.List = l;
                return _overlapps;
            }
            set { _overlapps = value; }
        }

        [DisplayName("3.2 Copy Schema")]
        [Description("Copy all column values beside the one that is summerized?")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public bool CopyRestOfColumnValues
        {
            get
            {
                var l = new List<object>();
                l.Add(true);
                l.Add(false);
                ListBoxEditor.List =l;
                return _copyRestOfColumnValues;
            }
            set { _copyRestOfColumnValues = value; }
        }
    }
}

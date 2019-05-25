using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace GDMPlugins.Statistics
{
    public partial class GroupingAndPreviewEditor : Form
    {
        private GenerateDistributionSettings _settings;
        private GraphPane _histogramPane;
        private GraphPane _scatterPlotPane;
        private GraphPane _ppPlotPane;
        private int _nbrOfIntervals;
        private Dictionary<string, string[]> _keys = new Dictionary<string, string[]>();

        // Holds the observations respective the estimated parameters for every distribution given a preview group
        private List<double> _observations;
        private List<DistributionData> _distributionsData = new List<DistributionData>();

        private List<string> _dataColumns;
        private List<string> _groupingColumns;

        public GroupingAndPreviewEditor(GenerateDistributionSettings settings)
        {
            InitializeComponent();
            InitializeZedGraphs();
            _settings = settings;
            intervalsNumericUpDown.Minimum = 1;
            intervalsNumericUpDown.ValueChanged += IntervalNumericUpDown_ValueChanged;
            string tableSelected;

            if (_settings.DataSourceTable != null)
            {
                tableSelected = _settings.DataSourceTable;
                InitializeColumns(tableSelected);
            }
            else
            {
                tableSelected = _settings.Model.SelectedTable.TableName;
                tableComboBox.SelectedIndexChanged += TableComboBox_SelectedIndexChanged;
            }

            foreach (DataTable table in _settings.Model.GetTables())
            {
                string tableName = table.TableName;
                tableComboBox.Items.Add(tableName);

                if (tableName == tableSelected)
                    tableComboBox.SelectedIndex = tableComboBox.Items.Count - 1;
            }

            if (_settings.DataSourceTable != null)
                tableComboBox.SelectedIndexChanged += TableComboBox_SelectedIndexChanged;
        }

        // Non volatile properties for the ZedGraphs
        private void InitializeZedGraphs()
        {
            _histogramPane = histogramZedGraphControl.GraphPane;
            _histogramPane.Title.IsVisible = false;
            _histogramPane.XAxis.Title.IsVisible = false;
            _histogramPane.YAxis.Title.IsVisible = false;
            _histogramPane.XAxis.Type = AxisType.Text;
            _histogramPane.XAxis.Scale.FontSpec.Angle = 45;
            _histogramPane.BarSettings.MinClusterGap = 0;
            _histogramPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 45);
            _histogramPane.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 225), 45);

            _ppPlotPane = InitializePPplot(ppPlotZedGraphControl);

            _scatterPlotPane = scatterPlotZedGraphControl.GraphPane;
            _scatterPlotPane.Title.IsVisible = false;
            _scatterPlotPane.XAxis.Title.IsVisible = false;
            _scatterPlotPane.YAxis.Title.IsVisible = false;
            _scatterPlotPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 45);
            _scatterPlotPane.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 225), 45);
        }
        
        internal static GraphPane InitializePPplot(ZedGraphControl zedGraphControl)
        {
            GraphPane pane = zedGraphControl.GraphPane;
            pane.Title.IsVisible = false;
            pane.XAxis.Title.IsVisible = false;
            pane.YAxis.Title.IsVisible = false;
            pane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 45);
            pane.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 225), 45);
            return pane;
        }

        private void InitializeColumns(string table)
        {
            _groupingColumns = new List<string>();
            _dataColumns = new List<string>();

            if (dataColumnComboBox.Items.Count > 0)
                dataColumnComboBox.Items.Clear();

            if (groupingColumnsCheckedListBox.Items.Count > 0)
                groupingColumnsCheckedListBox.Items.Clear();

            int index = 0;

            foreach(DataColumn d in _settings.Model.GetColumns(table))
            {
                if (d.DataType == typeof(int) || d.DataType == typeof(double))
                {
                    _dataColumns.Add(d.ColumnName);
                    int i = dataColumnComboBox.Items.Add(d.ColumnName);

                    if (_settings.DataColumn == d.ColumnName)
                        index = i;
                }

                _groupingColumns.Add(d.ColumnName);
                if (_settings.OperationNameColumn == d.ColumnName)
                {
                    groupingColumnsCheckedListBox.Items.Add(d.ColumnName, CheckState.Checked);
                    groupingColumnsCheckedListBox.SelectedItem = groupingColumnsCheckedListBox.CheckedItems[0];
                    index = groupingColumnsCheckedListBox.SelectedIndex;
               }
                else
                    groupingColumnsCheckedListBox.Items.Add(d.ColumnName, false);
            }

            dataColumnComboBox.SelectedIndex = index;
        }

        private void TableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tableName = (string)tableComboBox.SelectedItem;
            _settings.GroupingColumns = new List<string>();
            InitializeColumns(tableName);
            UpdatePreview();
        }

        private void StatisticsEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetSettings();
        }

        private void SetSettings()
        {
            _settings.DataSourceTable = (string)tableComboBox.SelectedItem;

            if (dataColumnComboBox.SelectedItem != null)
                _settings.DataColumn = dataColumnComboBox.SelectedItem.ToString();
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DataColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (groupingColumnsCheckedListBox.Items.Count > 0)
                groupingColumnsCheckedListBox.Items.Clear();

            foreach (string g in _groupingColumns)
            {
                if (g != dataColumnComboBox.SelectedItem.ToString())
                {
                    bool check = _settings.GroupingColumns.Contains(g);
                    groupingColumnsCheckedListBox.Items.Add(g, check);
                }
            }

            /*object remove = null;

            foreach (string g in this.groupingColumns)
            {
                bool exists = false;

                foreach (object item in this.groupingColumnsCheckedListBox.Items)
                {
                    if (g == item.ToString()) exists = true;
                    if (this.dataColumnComboBox.SelectedItem.ToString() == item.ToString()) remove = item;
                }

                if (!exists) this.groupingColumnsCheckedListBox.Items.Add(g);
            }

            this.groupingColumnsCheckedListBox.Items.Remove(remove);*/
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            SetSettings();

            if (_settings.DataColumn == null || _settings.DataSourceTable == null)
            {
                tabControl.Enabled = false;
                return;
            }
            else tabControl.Enabled = true;

            DataTable table = _settings.Model.GetTable(_settings.DataSourceTable);
            previewGroupComboBox.Items.Clear();

            if (_settings.GroupingColumns.Count == 0)
            {
                previewGroupComboBox.Enabled = false;
                PreviewData(null);
                return;
            }

            _keys.Clear();

            foreach (DataRow row in table.Rows)
            {
                int colCount = _settings.GroupingColumns.Count;
                StringBuilder key = new StringBuilder();
                string[] value = new string[colCount];

                for (int i = 0; i < colCount; i++)
                {
                    string v = row[_settings.GroupingColumns[i]].ToString();
                    value[i] = v;
                    key.Append(v);

                    if (i + 1 != colCount) key.Append(" - ");
                }
                
                string k = key.ToString();

                if (!_keys.ContainsKey(k))
                {
                    previewGroupComboBox.Items.Add(k);
                    _keys.Add(k, value);
                }
            }

            previewGroupComboBox.SelectedIndex = 0;
            previewGroupComboBox.Enabled = true;
        }

        private void GroupingColumnsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string column = (sender as CheckedListBox)?.Items[e?.Index??0]?.ToString();

            if (column == null) return;
            else if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
            {
                Type type = _settings.Model.GetTable(_settings.DataSourceTable).Columns[column].DataType;
                if (type == typeof(int) || type == typeof(double)) dataColumnComboBox.Items.Add(column);
                _settings.GroupingColumns.Remove(column);
                UpdatePreview();
            }
            else if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
            {
                if(!_settings.GroupingColumns.Contains(column))
                    _settings.GroupingColumns.Add(column);

                object remove = null;
                foreach (object item in dataColumnComboBox.Items)
                    if (column == item.ToString())
                    {
                        remove = item;
                        break;
                    }
                if (remove != null) dataColumnComboBox.Items.Remove(remove);

                UpdatePreview();
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)previewGroupComboBox.SelectedItem == null)
            {
                PreviewData(null);
                return;
            }

            string[] keyValues;

            if (_keys.TryGetValue((string)previewGroupComboBox.SelectedItem, out keyValues))
                PreviewData(keyValues);
        }

        private void PreviewData(string[] columnValues)
        {
            if (_settings.DataColumn == null) return;

            DataTable table = _settings.Model.GetTable(_settings.DataSourceTable);
            string[] columnNames = _settings.GroupingColumns.ToArray();

            _observations = GenerateDistributions.ExtractData(table, _settings.DataColumn, columnValues, columnNames);
            _distributionsData = DistributionEvaluator.EvaluateDistributions(_observations,  null,null).ToList();

            // Only run once
            if (distrubutionComboBox.Items.Count <= 0 && _distributionsData != null)
            {
                foreach (DistributionData dData in _distributionsData)
                    distrubutionComboBox.Items.Add(dData.Distribution.GetType().Name);

                distrubutionComboBox.SelectedIndex = 0;
            }

            // sqrt(n) as default
            _nbrOfIntervals = (int)Math.Sqrt(_observations.Count);

            // By-pass the ValueChanged event temporary
            intervalsNumericUpDown.ValueChanged -= IntervalNumericUpDown_ValueChanged;
            intervalsNumericUpDown.Maximum = _observations.Count;
            intervalsNumericUpDown.Value = _nbrOfIntervals;
            intervalsNumericUpDown.ValueChanged += IntervalNumericUpDown_ValueChanged;

            TabControl_SelectedIndexChanged(null, null);
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPage1)
                SummaryStatistics(_observations);
            else if (tabControl.SelectedTab == tabPage2)
                Histogram(_observations, _nbrOfIntervals);
            else if (tabControl.SelectedTab == tabPage3)
            {
                if (_observations.Count >= 1000)
                {
                    DialogResult result = MessageBox.Show("The large amount of observations may take a while to plot and the computer might freeze for some time. Proceed anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No) return;
                }
                ScatterPlot(_observations);
            }
            else if (tabControl.SelectedTab == tabPage4)
            {
                if (_distributionsData == null) return;

                foreach (DistributionData dd in _distributionsData)
                {
                    if (dd.Distribution.GetType().Name == distrubutionComboBox.SelectedItem.ToString())
                    {
                        PPPlot(_observations, dd);
                        break;
                    }
                }
            }
        }

        private void PPPlot(List<double> data, DistributionData dData)
        {
            PPPlot(ppPlotZedGraphControl, _ppPlotPane, data, dData);
        }

        internal static void PPPlot(ZedGraphControl control, GraphPane pane, List<double> data, DistributionData dData)
        {
            pane.CurveList.Clear();
            
            PointPairList pointList = new PointPairList();
            List<double> shadow = data.Distinct().Take(200).ToList();
            shadow.Sort(); int i = 1;
            try
            {
                foreach (double x in shadow)
                {
                    double a = dData.Distribution.DistributionFunction(x);
                    double b = (i - 0.5)/shadow.Count;
                    pointList.Add(b, a);
                    i++;
                }
            }
            catch (Exception)
            {
                pointList = new PointPairList();
            }
            LineItem curve = pane.AddCurve(null, pointList, Color.FromArgb(51, 153, 255), SymbolType.Circle);
            curve.Line.Width = 1.0F;
            curve.Line.IsVisible = false;
            
            
            pointList = new PointPairList(new double[] { 0, 1 }, new double[] { 0, 1 });
            curve = pane.AddCurve(null, pointList, Color.Tomato, SymbolType.None);

            control.RestoreScale(pane);
            control.AxisChange();
            control.Refresh();
        }

        private void ScatterPlot(List<double> data)
        {
            _scatterPlotPane.CurveList.Clear();

            PointPairList pointList = new PointPairList();
            int x = 0;

            foreach (double y in data)
            {
                pointList.Add(x, y);
                LineItem curve = _scatterPlotPane.AddCurve(null, pointList, Color.FromArgb(51, 153, 255), SymbolType.Circle);
                curve.Line.Width = 2.0F;
                curve.Line.IsVisible = false;
                x++;
            }

            scatterPlotZedGraphControl.RestoreScale(_scatterPlotPane);
            scatterPlotZedGraphControl.AxisChange();
            scatterPlotZedGraphControl.Refresh();
        }

        private void Histogram(List<double> data, int intervals)
        {
            List<int> histogram;
            int intervalWidth;
            double minimum, maximum;
            Miscellaneous.Histogram(data, intervals, out histogram, out intervalWidth, out minimum, out maximum);

            PointPairList pointList = new PointPairList();
            int x = 0;

            foreach (int y in histogram)
            {
                pointList.Add(x, y);
                x++;
            }

            string[] labels = new string[_nbrOfIntervals];
            double sum = minimum-1;

            for (int i = 0; i < _nbrOfIntervals; i++)
            {
                labels[i] = "" + (sum + 1) + "-" + (sum + intervalWidth);
                sum += intervalWidth;
            }

            _histogramPane.CurveList.Clear();

            BarItem curve = _histogramPane.AddBar(null, pointList, Color.Blue);
            curve.Bar.Fill = new Fill(Color.FromArgb(51, 153, 255));

            _histogramPane.XAxis.Scale.TextLabels = labels;

            histogramZedGraphControl.RestoreScale(_histogramPane);
            histogramZedGraphControl.AxisChange();
            histogramZedGraphControl.Refresh();
        }

        private void SummaryStatistics(List<double> data)
        {
            double maximum, minimum, mean, variance, skewness;
            Miscellaneous.Summary(data, out maximum, out minimum, out mean, out variance, out skewness);

            maximumResultLabel.Text = maximum.ToString();
            minimumResultLabel.Text = minimum.ToString();
            meanResultLabel.Text = mean.ToString();
            varianceResultLabel.Text = variance.ToString();
            stdDeviationResultLabel.Text = Math.Sqrt(variance).ToString();
            skewnessResultLabel.Text = skewness.ToString();
            sampleSizeResultLabel.Text = _observations.Count.ToString();

            rSquaresListBox.Items.Clear();
            if (_distributionsData == null)
            {
               rSquaresListBox.Items.Add("Error");
                return;
            }
            foreach (DistributionData dd in _distributionsData)
            {
               rSquaresListBox.Items.Add(dd.Distribution.GetType().Name + ": " + dd.RSquare);
            }
        }

        private void IntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
        {           
            _nbrOfIntervals = (int)intervalsNumericUpDown.Value;
            Histogram(_observations, _nbrOfIntervals);
        }
    }
}

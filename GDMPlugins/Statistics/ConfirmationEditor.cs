using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZedGraph;

namespace GDMPlugins.Statistics
{
    public partial class ConfirmationEditor : Form
    {
        private List<DistributionData> _dd;
        private string _grouping;
        private List<double> _data;
        private GraphPane _ppPlotPane;
        private DistributionData _selected;
        private DistributionData _best;
        private const string BestStr = "*";    // indicates the best model according to the P value

        public ConfirmationEditor(string grouping, List<DistributionData> distributionsData, DistributionData best, List<double> data)
        {
            InitializeComponent();
            _dd = distributionsData;
            _grouping = grouping;
            _data = data;
            _best = best;
        }

        private void Confirmation_Load(object sender, EventArgs e)
        {
            groupingLabel2.Text = _grouping;

            _ppPlotPane = GroupingAndPreviewEditor.InitializePPplot(ppPlotZedGraphControl);

            int i = 0;
            foreach (DistributionData dData in _dd)
            {
                string str = "";

                if (_best.Distribution.GetType() == dData.Distribution.GetType())
                    str = BestStr;

                distributionComboBox.Items.Add(dData.Distribution.GetType().Name + str);
                
                if (_best.Distribution.GetType() == dData.Distribution.GetType())
                    distributionComboBox.SelectedIndex = i;
                else i++;
            }
        }

        public void PpPlot(DistributionData dData)
        {
            GroupingAndPreviewEditor.PPPlot(ppPlotZedGraphControl, _ppPlotPane, _data, dData);
        }

        private void DistrubutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dd == null) return;

            foreach (DistributionData d in _dd)
            {
                if (d.Distribution.GetType().Name == distributionComboBox.SelectedItem.ToString().Replace("*", ""))
                {
                    _selected = d;
                    PpPlot(_selected);
                }
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            _best.Distribution = _selected.Distribution;
            _best.RSquare = _selected.RSquare;
            Close();
        }
    }
}

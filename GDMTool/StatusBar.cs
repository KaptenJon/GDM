using System.Windows.Forms;
using System.Windows.Threading;
using GDMCore;

namespace GDMTool
{
    /// <summary>
    /// Listens to the StatusUpdate event from the Controller and updates the progress bar and the progress label.
    /// </summary>
    public class StatusBar
    {
        private Controller _controller;
        private ToolStripProgressBar _progressBar;
        private ToolStripStatusLabel _statusLabel;

        private ToolStripProgressBar overallprogressbar;
        private ToolStripStatusLabel overalllable;
        private bool _firstRunB = true;

        public StatusBar(Controller controller, ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel, ToolStripProgressBar overallprogressbar, ToolStripStatusLabel overalllable)
        {
            this.overallprogressbar = overallprogressbar;
            this.overalllable = overalllable;
            _controller = controller;
            _progressBar = progressBar;
            _progressBar.Minimum = 0;
            _progressBar.Step = 1;
            _statusLabel = statusLabel;
            _controller.StatusUpdate += UpdateBar;
            _controller.BatchStatusUpdate += _controller_BatchStatusUpdate;
        }

        private void _controller_BatchStatusUpdate(int number, int percent)
        {
            const int max = 100;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                overallprogressbar.Visible = percent < max;
                overalllable.Visible = percent < max;
                overallprogressbar.Value = percent;
                overallprogressbar.Maximum = max;
            });
        }
    

        private void UpdateBar(string label, int percent)
        {
            const int max = 100;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                lock (this)
                    try
                    {
                        _progressBar.Visible = percent < max;
                        _statusLabel.Visible =  percent < max;
                        _progressBar.Value = percent;
                        _progressBar.Maximum = max; // 100%
                        _statusLabel.Text = label;
                        
                        }
                    catch { }
                });
        }
    }
}

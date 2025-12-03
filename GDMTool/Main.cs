using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using GDMCore;
using GDMInterfaces;
using Microsoft.Win32;

namespace GDMTool
{
    /// <summary>
    /// The Edit Mode main form. Instantiates all UI-elements that communicate with the controller.
    /// </summary>
    public partial class Main : Form
    {
        private Controller _controller;
        private PluginMenu _pluginmenu;
        private TableViewer _tableview;
        private PluginProperties _pluginProperties;
        private StatusBar _statusBar;
        private ConfigViewer _configView;
        private Dispatcher _dispatcherUi = Dispatcher.CurrentDispatcher;
        public Main(Controller controller)
        {
            _controller = controller;
           
            _controller.ExceptionRaised += delegate(ILog log)
            {
                if (MessageBox.Show("Executed plugin encountered an exception. View log for details." 
                    + "\n\n" + "View log file?", "Plugin Exception", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    LogViewer lw = new LogViewer(this._controller);
                    lw.Visible = true;
                }
                else return;
            };
            
            InitializeComponent();
            
            _pluginmenu = new PluginMenu(controller, pluginMenu);
            _pluginProperties = new PluginProperties(_controller, propertyGrid, applyBtn, logBtn);
            _tableview = new TableViewer(tabControl, _controller);
            _statusBar = new StatusBar(_controller, toolStripProgressBar, toolStripStatusLabel, overallprogressbar, overalllable);
            
            _configView = new ConfigViewer(_controller, treeView, configMenu);
            mnuExport.Click += mnuNewFile_Click;
            _pluginmenu.HideNonAvailible = true;
            if (controller.IsFirstRun)
            {
                //MessageBox.Show(this,"Updated features in this release:\n" +" * You can now choose amoung the distributions in generate distributions.");
                controller.IsFirstRun = false;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void newConfig_Click(object sender, EventArgs e)
        {

        }
        private TreeNode _mOldSelectNode;
        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            var treeView1 = sender as TreeView;
            if (e.Button == MouseButtons.Right && treeView1 != null)
            {
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);
                // Get the node that the user has clicked.
                TreeNode node = treeView1.GetNodeAt(p);
                if (node != null)
                {
                    treeView1.SelectedNode = node;

                    // Find the appropriate ContextMenu depending on the selected node.

                    mnuExport.Show(treeView1, p);
                    
                    
                }
                
            }
        }
        private void mnuNewFile_Click(object sender, EventArgs e)
        {
           Config config =treeView.SelectedNode?.Tag as Config;
            if(config != null)
            _controller.ConfigManager.Export(config, Controller.GdmServicePath +@"\"+treeView.SelectedNode.Text+".xml");
            
        }

        private void showAll_CheckedChanged(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            if (check != null)
                _pluginmenu.HideNonAvailible = !check.Checked;
        }
        private TaskFactory factory = new TaskFactory(); 
        private  void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Operation op = treeView.SelectedNode?.Tag as Operation;
            Config config = treeView.SelectedNode?.Tag as Config;
            if (config!=null || op != null && DialogResult.OK == MessageBox.Show(this,
                "This will take around " + _controller.ConfigManager.CurrentConfig.Time +
                " seconds and could possibly remove operations that are not longer valid after the operation",
                "Undo operation", MessageBoxButtons.OKCancel))
            {
                
                //await factory.StartNew(() =>
                {

                    if (op != null)
                    {
                        _controller.ConfigManager.CurrentConfig.RemoveOperation(op);
                        var ops = _controller.ConfigManager.CurrentConfig;
                        _controller.ResetData();
                        _controller.BatchRun(ops);
                    }
                    if (config != null)
                    {
                        _configView.DeleteConfig(config);
                  }
                }//);
            }
        }

        private  void configMenu_KeyDown(object sender, KeyEventArgs e)
        {
            var presskey = e as KeyEventArgs;
            if (presskey != null)
                if (presskey.KeyCode == Keys.Delete)
                     removeToolStripMenuItem_Click(null,null);

        }

        private void Batch_Click(object sender, EventArgs e)
        {
            if (Batch.Text == "Start new file batch")
            {
                System.Windows.Forms.OpenFileDialog files = new System.Windows.Forms.OpenFileDialog();
                files.Multiselect = true;
                files.Title = "Select all files for the batch operation";
                if (files.ShowDialog() == DialogResult.OK)
                {
                    _controller.StartBatch(files.FileNames);
                   _controller.BatchWorkUpdated += File_BatchWorkUpdated;
                }
            }
            else
            {
                _controller.EndBatch();
            }
        }

        private  void File_BatchWorkUpdated(object sender, Controller.BatchWorkUpdatedArgs args)
        {
            _dispatcherUi.Invoke(() =>
            {
                TableBatch.Visible = !args.active;
                if (args.active)
                {
                    buttonColor = Batch.BackColor;
                    Batch.BackColor = Color.Coral;
                    Batch.Text = "End file batch";
                }
                else if (!args.active)
                {
                    _controller.BatchWorkUpdated -= File_BatchWorkUpdated;
                    Batch.Text = "Start new file batch";
                    Batch.BackColor = buttonColor;
                    ListBoxEditor.CurrentConstant = null;
                }
            });
        }

        private Color buttonColor;
        private  void TableBatch_Click(object sender, EventArgs e)
        {
            _dispatcherUi.Invoke(() =>
            {
                if (TableBatch.Text == "Start new table batch")
                {
                    _controller.StartBatch(_controller.Model.GetAllTableNames().ToArray());
                    _controller.BatchWorkUpdated += Table_BatchWorkUpdated;
                }
                else
                {
                    _controller.EndBatch();
                }
            });
        }

        private void Table_BatchWorkUpdated(object sender, Controller.BatchWorkUpdatedArgs args)
        {
            _dispatcherUi.Invoke(() =>
            {
                Batch.Visible = !args.active;
                if (args.active)
                {
                    buttonColor = TableBatch.BackColor;
                    TableBatch.BackColor = Color.Coral;
                    TableBatch.Text = "End table batch";
                }
                else if (!args.active)
                {
                    _controller.BatchWorkUpdated -= Table_BatchWorkUpdated;
                    TableBatch.Text = "Start new table batch";
                    TableBatch.BackColor = buttonColor;
                    ListBoxEditor.CurrentConstant = null;
                }
            });
        }
        
        private void Configure_Click(object sender, EventArgs e)
        {
            var fold = new FolderBrowserDialog();
            fold.Description = "Set configuration folder for GDM service. Normally a network folder";
            fold.SelectedPath = "Network";
            fold.ShowNewFolderButton = true;
            if (DialogResult.OK == fold.ShowDialog(this))
            {
                Controller.GdmServicePath = fold.SelectedPath;
            }

        }

    }
}
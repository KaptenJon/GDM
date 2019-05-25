using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GDMCore;
using GDMInterfaces;

namespace GDMTool
{
    /// <summary>
    /// The window that is first displayed when starting the program. Handle button events and starting configurations from a list of loaded configurations.
    /// </summary>
    public partial class Start : Form
    {
        private Controller _controller;
        private Label _helptext;
        private Config _selected;
        private string _logfilepath;

        public Start()
        {
            _helptext = new Label();
            _helptext.AutoEllipsis = true;
            _helptext.BackColor = Color.WhiteSmoke;
            _helptext.Name = "helptext";
            _helptext.AutoSize = true;

            string path = Directory.GetCurrentDirectory();
            _controller = new Controller(path);
            InitializeComponent();

            foreach (Config c in _controller.ConfigManager.Configurations)
            {
                listBox1.Items.Add(c.Name);
            }
            _controller.StatusUpdate += controller_StatusUpdate;
            _controller.BatchStatusUpdate += controller_BatchStatusUpdate;
        }

        void controller_BatchStatusUpdate(int number, int percent)
        {
            overallProgressBar.Value = percent;
            overallProgress.Text = "Operation: " + number + "/" + _selected.Operations.Count;

            if (percent == 100 && _logfilepath != null)
                _controller.WriteLog(_logfilepath);
        }

        void controller_StatusUpdate(string label, int percent)
        {
            pluginProgressBar.Value = percent;
            pluginProgress.Text = label;
            pluginProgress.Invalidate();
        }

        private void EditConfig_Click(object sender, EventArgs e)
        {
            _controller.StatusUpdate -= controller_StatusUpdate;
            _controller.BatchStatusUpdate -= controller_BatchStatusUpdate;
            Main main = new Main(_controller);
            main.Show();
            Hide();
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            string sel = (string)listBox1.SelectedItem;
            _selected = null;

            foreach (Config c in _controller.ConfigManager.Configurations)
            {
                if (c.Name == sel)
                {
                    _selected = c; 
                    break;
                }
            }

            if (_selected == null) return;
            logfile.Enabled = true;
            run.Enabled = true;
            overallProgressBar.Enabled = true;
            pluginProgressBar.Enabled = true;

            foreach (Operation op in _selected.Operations)
            {
                IPlugin plugin = Controller.PluginManager.GetPlugin(op.Plugin);
                Type t = plugin.GetType();

                foreach (Type iface in t.GetInterfaces())
                {
                    if (iface == typeof(IIo))
                    {
                        DisplaySettings((IIo)plugin, op.Settings);
                    }
                }
            }
            flowLayoutPanel1.Controls.Add(_helptext);
        }

        private void DisplaySettings(IIo plugin, PluginSettings pluginSettings)
        {
            object dynamicsettings = plugin.GetDynamicSettings(pluginSettings);
            Type t = dynamicsettings.GetType();
            string description = plugin.GetJobDescription(pluginSettings);

            if (description == null) description = plugin.Name;
            if (description.Length == 0) description = plugin.Name;

            PropertyInfo[] properties = t.GetProperties();
            int numberOfProperties = properties.Length;
            foreach (PropertyInfo p in properties)
            {
                object[] customattributes = p.GetCustomAttributes(false);

                foreach (object o in customattributes)
                {
                    BrowsableAttribute b = o as BrowsableAttribute;
                    if (b == null) continue;
                    if (!b.Browsable) numberOfProperties--;
                }
            }

            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label.Margin = new Padding(0,0,0,5);
            label.TabIndex = 4;
            label.Text = description;
            flowLayoutPanel1.Controls.Add(label);

            PropertyGrid propertygrid = new PropertyGrid();
            propertygrid.AutoScaleMode = AutoScaleMode.Dpi;
            propertygrid.CommandsVisibleIfAvailable = false;
            propertygrid.HelpVisible = false;
            propertygrid.Margin = new Padding(0,0,0,5);
            propertygrid.PropertySort = PropertySort.NoSort;
            propertygrid.TabIndex = 0;
            propertygrid.ToolbarVisible = false;
            propertygrid.SelectedObject = dynamicsettings;
            propertygrid.Height = numberOfProperties * 16 + 3;
            propertygrid.SelectedGridItemChanged += propertygrid_SelectedGridItemChanged;
            flowLayoutPanel1.Controls.Add(propertygrid);
            propertygrid.Width = flowLayoutPanel1.ClientSize.Width;
        }

        void propertygrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            _helptext.Text = e.NewSelection.PropertyDescriptor.Description;
        }

        private void flowLayoutPanel1_ClientSizeChanged(object sender, EventArgs e)
        {
            _helptext.Width = flowLayoutPanel1.ClientSize.Width;

            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c.Width > flowLayoutPanel1.ClientSize.Width)
                {
                    c.Width = flowLayoutPanel1.ClientSize.Width;
                }
            }
        }

        private void run_Click(object sender, EventArgs e)
        {
            overallProgressBar.Value = 0;
            pluginProgressBar.Value = 0;
            pluginProgress.Text = "";
            overallProgress.Text = "";
            _controller.ConfigManager.Save(_selected, _selected.FileName);
            _controller.BatchRun(_selected);
        }

        private void logfile_CheckedChanged(object sender, EventArgs e)
        {
            if (logfile.Checked)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Log Files|*.log|All Files|*.*";
                dialog.Title = "Save Log File";
                dialog.DefaultExt = "log";
                dialog.AddExtension = true;
                dialog.FileName = _selected.Name + ".log";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _logfilepath = dialog.FileName;
                }
                else
                {
                    _logfilepath = null;
                    logfile.Checked = false;
                }
            }
            else
            {
                _logfilepath = null;
            }

            logfilepath_label.Text = _logfilepath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }
    }
}

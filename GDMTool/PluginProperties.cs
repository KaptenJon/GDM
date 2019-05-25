using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using GDMCore;
using GDMInterfaces;

namespace GDMTool
{
    /// <summary>
    /// Controls the property view of a plug-in and the validation of made plug-in settings. Instructs the Controlelr to start an operation if the user presses the Apply button. Also starts the LogViewer when the Log button is pressed.
    /// </summary>
    class PluginProperties
    {
        private Controller _controller;
        private PropertyGrid _propertyGrid;
        private Button _applyBtn;

        public PluginProperties(Controller controller, PropertyGrid propertyGrid, Button applyBtn, Button logBtn)
        {
            _controller = controller;
            _propertyGrid = propertyGrid;
            _applyBtn = applyBtn;
            _controller.ColumnSelection += Controller_ColumnSelection;
            _controller.TableSelection += Controller_TableSelection;
            _controller.PluginActivated += Controller_PluginActivated;

            _applyBtn.Click += ApplyBtn_Click;
            logBtn.Click += LogBtn_Click;

            _propertyGrid.Enabled = false;
            _applyBtn.Enabled = false;
        }

        async void ApplyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (BatchPlugin.CurrentBatchPlugin != null || await _controller.ActiveSettings.IsValid() )
                {
                    
                    _controller.ApplyActivePlugin();
                    return;
                }
            }
            catch (PluginException ee)
            {
                MessageBox.Show(ee.ErrorMessage, "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            
        }

        void LogBtn_Click(object sender, EventArgs e)
        {
            if (_controller.IsBusy)
            {
                MessageBox.Show("The log is unavailable right now. Try again in a moment.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LogViewer lw = new LogViewer(_controller);
            lw.Visible = true;
        }

        void Controller_PluginActivated(IPlugin plugin)
        {
            if (plugin != null)
            {
                _propertyGrid.SelectedObject = _controller.ActiveSettings;
                _propertyGrid.Enabled = true;
                _applyBtn.Enabled = true;
            }
            else
            {
                _propertyGrid.SelectedObject = null;
                _propertyGrid.Enabled = false;
                _applyBtn.Enabled = false;
            }
        }

        void Controller_TableSelection(string table)
        {
            ITool tool = _controller.ActivePlugin as ITool;
            if (tool != null) 
            {
                if (table == null && tool.NeedTableSelected)
                {
                    _controller.ActivePlugin = null;
                    return;
                }
            }
            _propertyGrid.SelectedObject = _controller.ActiveSettings;
        }

        void Controller_ColumnSelection(string column, Type type)
        {
            ITool tool = _controller.ActivePlugin as ITool;
            if (tool != null)
            {
                if (column == null && tool.NeedColumnSelected && tool.AcceptsDataType(type))
                {
                    _controller.ActivePlugin = null;
                    return;
                }
            }
            _propertyGrid.SelectedObject = _controller.ActiveSettings;
        }
    }
}

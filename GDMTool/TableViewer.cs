using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GDMCore;

namespace GDMTool
{
    /// <summary>
    /// Controls the view of tables and the selection of tables.
    /// </summary>
    class TableViewer
    {
        private Controller _controller;
        private TabControl _tableControl;
        private DataTableView _dataTableView;

        public TableViewer(TabControl tableControl, Controller controller)
        {
            _controller = controller;
            _controller.ModelChanged += Controller_ModelChanged;
            _controller.ModelChanging += Controller_ModelChanging;
            _tableControl = tableControl;
            _tableControl.Visible = false;
            _tableControl.SelectedIndexChanged += TableControl_SelectedIndexChanged;
            _tableControl.ImageList = new ImageList();
            _tableControl.ImageList.Images.Add(Images.Images.table);
            InitDataTableView();
        }

        private void Controller_ModelChanging()
        {
            _dataTableView.Inactivate();
        }

        private void Controller_ModelChanged()
        {
            _dataTableView.Activate();
            TabPage selected = null;
            _tableControl.Invoke((MethodInvoker)(() =>
            {
            if (_tableControl.TabPages.Count > 0)
            {
                
                    selected = _tableControl.SelectedTab;
                
            }

            List<TabPage> remove = new List<TabPage>();

            foreach (TabPage page in _tableControl.TabPages)
            {
                DataTable table = page.Tag as DataTable;
               
                    page.Text = table.TableName;

                    if (table.DataSet == null)
                        remove.Add(page);
                
            }

            foreach (TabPage t in remove) 
                _tableControl.TabPages.Remove(t);

            if (_tableControl.TabPages.Count == 0) _dataTableView.DataTable = null;

            List<DataTable> tables = _controller.GetTables();

            foreach (DataTable table in tables)
            {
                if (!Contains(table.TableName))
                {
                    TabPage newPage = new TabPage(table.TableName);
                    newPage.Controls.Add(_dataTableView);
                    newPage.Tag = table;
                    newPage.ImageIndex = 0;
                    _tableControl.TabPages.Add(newPage);
                    _tableControl.SelectedTab = newPage;
                }
            }

            _tableControl.Visible = _tableControl.TabPages.Count > 0;

            if (_tableControl.TabPages.Count > 0)
            {
                if (_tableControl.SelectedTab != null)
                {
                    TabPage page = _tableControl.SelectedTab;

                    if (page == selected || _dataTableView.DataTable == null)
                    {
                        _controller.SelectedTable = (DataTable)page.Tag;
                        _dataTableView.DataTable = null;
                        _dataTableView.DataTable = (DataTable)page.Tag;
                    }
                }
            }
            }));
        }

        private bool Contains(string table)
        {
            foreach (TabPage p in _tableControl.TabPages)
            {
                if (p.Text == table) return true;
            }
            return false;
        }

        private void TableControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_controller.IsBusy) return;
            if (_tableControl.SelectedTab == null) return;

            TabPage page = ((TabControl)sender).SelectedTab;
            page.Controls.Add(_dataTableView);

            if (page.Tag != null)
            {
                _controller.SelectedTable = (DataTable)page.Tag;
                _dataTableView.DataTable = (DataTable)page.Tag;
            }
            else
            {
                _dataTableView.DataTable = null;
            }
        }

        private void InitDataTableView() 
        {
            _dataTableView = new DataTableView(_controller);
            _dataTableView.AutoSize = true;
            _dataTableView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _dataTableView.BackColor = Color.White;
            _dataTableView.DataTable = null;
            _dataTableView.Dock = DockStyle.Fill;
            _dataTableView.Location = new Point(3, 3);
            _dataTableView.Margin = new Padding(0);
            _dataTableView.Name = "dataTableView";
            _dataTableView.Size = new Size(474, 445);
            _dataTableView.TabIndex = 0;
            
        }
    }
}

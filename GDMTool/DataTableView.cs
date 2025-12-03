using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Threading;
using System.Windows.Forms;
using GDMCore;
using GDMInterfaces;

namespace GDMTool
{
    /// <summary>
    /// Controls the table view menu, the selection of columns and the Tag menu.
    /// </summary>
    public partial class DataTableView : UserControl
    {
        private Controller _controller;
        private bool _inactive;

        public DataTableView(Controller controller)
        {
            InitializeComponent();
            _controller = controller;
            datagrid.SelectionChanged += Datagrid_SelectionChanged;
            Activate();
            Type dgvType = datagrid.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(datagrid, true, null);
        }

        public void Inactivate()
        {
      
            _inactive = true;
            try
            {
                if(this.IsHandleCreated)
                this.Invoke((System.Windows.Forms.MethodInvoker) (() =>
                {

                    bindingSource.DataSource = null;
                }));
            }
            catch  { }


        }

        public void Activate()
        {
            _inactive = false;
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public DataTable DataTable
        {
            get { return (DataTable)bindingSource.DataSource; }
            set
            {
                selctedValue.Text = "";
                dataTypeValue.Text = "";
                _controller.SetSelectedColumn(null, null);
                datagrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
                while (datagrid.SelectionMode != DataGridViewSelectionMode.CellSelect) { ;}
                try
                {
                    bindingSource.DataSource = value;
                    datagrid.DataSource = bindingSource;
                }
                catch { }
            }
        }

        private void FixColumnSelect()
        {
            if (datagrid.Columns.Count > 0)
            {
                foreach (DataGridViewColumn d in datagrid.Columns)
                {
                    d.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                datagrid.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
            }
        }

        void Datagrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_inactive) return;
            if (datagrid.SelectionMode == DataGridViewSelectionMode.FullColumnSelect)
            {
                DataGridViewSelectedColumnCollection d = datagrid.SelectedColumns;
                
                if (d.Count > 0)
                {
                    selctedValue.Text = d[0].Name;
                    dataTypeValue.Text = d[0].ValueType.ToString();
                    _controller.SetSelectedColumn(d[0].Name, d[0].ValueType);
                }
                else
                {
                    selctedValue.Text = "";
                    dataTypeValue.Text = "";
                    _controller.SetSelectedColumn(null, null);
                }
            }
            else
            {
                FixColumnSelect();
            }
        }

        private void tagButton_DropDownOpening(object sender, EventArgs e)
        {
            string table = _controller.SelectedTable.TableName;
            string column = selctedValue.Text;
            Type columntype = datagrid.SelectedColumns[0].ValueType;

            tableName.Text = "Table: " + table;
            columnName.Text = "Column: " + column;

            int tableIndex = tagButton.DropDownItems.IndexOf(tableName);
            int columnIndex = tagButton.DropDownItems.IndexOf(columnName);
            int toremove = columnIndex - 2;
            while (toremove-- > 0) tagButton.DropDownItems.RemoveAt(2);
            
            toremove = tagButton.DropDownItems.Count - 4;
            while (toremove-- > 0) tagButton.DropDownItems.RemoveAt(4);

            List<Tag> columnTags = _controller.GetColumnTags(column, table);
            List<Tag> tableTags = _controller.GetTableTags(table);

            foreach (Tag t in columnTags)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = t.TagName;
                item.Enabled = false;
                tagButton.DropDownItems.Insert(4, item);
            }

            foreach (Tag t in tableTags)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = t.TagName;
                item.Enabled = false;
                tagButton.DropDownItems.Insert(2, item);
            }

            tableAdd.DropDownItems.Clear();
            tableRemove.DropDownItems.Clear();
            colAdd.DropDownItems.Clear();
            colRemove.DropDownItems.Clear();

            List<Tag> availTablebTags = _controller.GetAvailabeTags(table);
            List<Tag> availColumnTags = _controller.GetAvailabeTags(table, column, columntype);

            tableAdd.Enabled = availTablebTags.Count > 0;
            tableRemove.Enabled = tableTags.Count > 0;
            colAdd.Enabled = availColumnTags.Count > 0;
            colRemove.Enabled = columnTags.Count > 0;

            foreach (Tag t in availTablebTags)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = t.TagName;
                item.Tag = t;
                tableAdd.DropDownItems.Add(item);
                item.Click += AddTag_Click;
            }
            
            foreach (Tag t in availColumnTags)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = t.TagName;
                item.Tag = t;
                colAdd.DropDownItems.Add(item);
                item.Click += AddTag_Click;
            }
            
            foreach (Tag t in columnTags)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = t.TagName;
                item.Tag = t;
                colRemove.DropDownItems.Add(item);
                item.Click += RemoveTag_Click;
            }

            foreach (Tag t in tableTags)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = t.TagName;
                item.Tag = t;
                tableRemove.DropDownItems.Add(item);
                item.Click += RemoveTag_Click;
            }
        }

        private void AddTag_Click(object sender, EventArgs e)
        {
            Tag t = (Tag)((ToolStripMenuItem)sender).Tag;
            _controller.SetTag(t);
        }

        private void RemoveTag_Click(object sender, EventArgs e)
        {
            Tag t = (Tag)((ToolStripMenuItem)sender).Tag;
            _controller.UnsetTag(t);
        }
    }
}
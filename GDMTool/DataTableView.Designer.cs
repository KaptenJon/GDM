using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GDMTool
{
    partial class DataTableView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                try
                {
                    components.Dispose();
                }
                catch (Exception)
                {
                  
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataTableView));
            this.datagrid = new System.Windows.Forms.DataGridView();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tagButton = new System.Windows.Forms.ToolStripSplitButton();
            this.tableName = new System.Windows.Forms.ToolStripMenuItem();
            this.tableAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tableRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.columnName = new System.Windows.Forms.ToolStripMenuItem();
            this.colAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.colRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectedColumnLabel = new System.Windows.Forms.ToolStripLabel();
            this.selctedValue = new System.Windows.Forms.ToolStripLabel();
            this.dataTypeLabel = new System.Windows.Forms.ToolStripLabel();
            this.dataTypeValue = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.datagrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).BeginInit();
            this.bindingNavigator.SuspendLayout();
            this.SuspendLayout();
            // 
            // datagrid
            // 
            this.datagrid.AllowUserToAddRows = false;
            this.datagrid.AllowUserToOrderColumns = true;
            this.datagrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datagrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.datagrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.datagrid.BackgroundColor = System.Drawing.Color.White;
            this.datagrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.datagrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.datagrid.GridColor = System.Drawing.SystemColors.ControlLight;
            this.datagrid.Location = new System.Drawing.Point(0, 0);
            this.datagrid.Margin = new System.Windows.Forms.Padding(0);
            this.datagrid.MultiSelect = false;
            this.datagrid.Name = "datagrid";
            this.datagrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.datagrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.datagrid.Size = new System.Drawing.Size(654, 271);
            this.datagrid.TabIndex = 1;
            // 
            // bindingNavigator
            // 
            this.bindingNavigator.AddNewItem = null;
            this.bindingNavigator.BindingSource = this.bindingSource;
            this.bindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator.DeleteItem = null;
            this.bindingNavigator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.tagButton,
            this.selectedColumnLabel,
            this.selctedValue,
            this.dataTypeLabel,
            this.dataTypeValue});
            this.bindingNavigator.Location = new System.Drawing.Point(0, 271);
            this.bindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator.Name = "bindingNavigator";
            this.bindingNavigator.Padding = new System.Windows.Forms.Padding(0);
            this.bindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator.Size = new System.Drawing.Size(654, 25);
            this.bindingNavigator.Stretch = true;
            this.bindingNavigator.TabIndex = 2;
            this.bindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 21);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tagButton
            // 
            this.tagButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tagButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tableName,
            this.toolStripSeparator1,
            this.columnName,
            this.toolStripSeparator2});
            this.tagButton.Image = ((System.Drawing.Image)(resources.GetObject("tagButton.Image")));
            this.tagButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tagButton.Name = "tagButton";
            this.tagButton.Size = new System.Drawing.Size(32, 22);
            this.tagButton.Text = "toolStripSplitButton1";
            this.tagButton.DropDownOpening += new System.EventHandler(this.tagButton_DropDownOpening);
            // 
            // tableName
            // 
            this.tableName.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tableAdd,
            this.tableRemove});
            this.tableName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableName.Name = "tableName";
            this.tableName.Size = new System.Drawing.Size(148, 22);
            this.tableName.Text = "Table: Table 1";
            // 
            // tableAdd
            // 
            this.tableAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableAdd.Image = ((System.Drawing.Image)(resources.GetObject("tableAdd.Image")));
            this.tableAdd.Name = "tableAdd";
            this.tableAdd.Size = new System.Drawing.Size(139, 22);
            this.tableAdd.Text = "Add Tag";
            // 
            // tableRemove
            // 
            this.tableRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableRemove.Image = ((System.Drawing.Image)(resources.GetObject("tableRemove.Image")));
            this.tableRemove.Name = "tableRemove";
            this.tableRemove.Size = new System.Drawing.Size(139, 22);
            this.tableRemove.Text = "Remove Tag";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // columnName
            // 
            this.columnName.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colAdd,
            this.colRemove});
            this.columnName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.columnName.Name = "columnName";
            this.columnName.Size = new System.Drawing.Size(148, 22);
            this.columnName.Text = "Column: Col4";
            // 
            // colAdd
            // 
            this.colAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colAdd.Image = ((System.Drawing.Image)(resources.GetObject("colAdd.Image")));
            this.colAdd.Name = "colAdd";
            this.colAdd.Size = new System.Drawing.Size(139, 22);
            this.colAdd.Text = "Add Tag";
            // 
            // colRemove
            // 
            this.colRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colRemove.Image = ((System.Drawing.Image)(resources.GetObject("colRemove.Image")));
            this.colRemove.Name = "colRemove";
            this.colRemove.Size = new System.Drawing.Size(139, 22);
            this.colRemove.Text = "Remove Tag";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // selectedColumnLabel
            // 
            this.selectedColumnLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedColumnLabel.Name = "selectedColumnLabel";
            this.selectedColumnLabel.Size = new System.Drawing.Size(116, 22);
            this.selectedColumnLabel.Text = "   Selected Column: ";
            // 
            // selctedValue
            // 
            this.selctedValue.Name = "selctedValue";
            this.selctedValue.Size = new System.Drawing.Size(0, 22);
            // 
            // dataTypeLabel
            // 
            this.dataTypeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTypeLabel.Name = "dataTypeLabel";
            this.dataTypeLabel.Size = new System.Drawing.Size(74, 22);
            this.dataTypeLabel.Text = "  Data Type:";
            // 
            // dataTypeValue
            // 
            this.dataTypeValue.Name = "dataTypeValue";
            this.dataTypeValue.Size = new System.Drawing.Size(0, 22);
            // 
            // DataTableView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.bindingNavigator);
            this.Controls.Add(this.datagrid);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DataTableView";
            this.Size = new System.Drawing.Size(654, 296);
            ((System.ComponentModel.ISupportInitialize)(this.datagrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).EndInit();
            this.bindingNavigator.ResumeLayout(false);
            this.bindingNavigator.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView datagrid;
        private BindingSource bindingSource;
        private BindingNavigator bindingNavigator;
        private ToolStripLabel bindingNavigatorCountItem;
        private ToolStripButton bindingNavigatorMoveFirstItem;
        private ToolStripButton bindingNavigatorMovePreviousItem;
        private ToolStripSeparator bindingNavigatorSeparator;
        private ToolStripTextBox bindingNavigatorPositionItem;
        private ToolStripSeparator bindingNavigatorSeparator1;
        private ToolStripButton bindingNavigatorMoveNextItem;
        private ToolStripButton bindingNavigatorMoveLastItem;
        private ToolStripSeparator bindingNavigatorSeparator2;
        private ToolStripLabel selectedColumnLabel;
        private ToolStripLabel selctedValue;
        private ToolStripLabel dataTypeLabel;
        private ToolStripLabel dataTypeValue;
        private ToolStripSplitButton tagButton;
        private ToolStripMenuItem columnName;
        private ToolStripMenuItem tableName;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tableAdd;
        private ToolStripMenuItem tableRemove;
        private ToolStripMenuItem colAdd;
        private ToolStripMenuItem colRemove;
        private ToolStripSeparator toolStripSeparator2;
    }
}
